using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace BattleReal.BulletGenerator
{
    using IndividualParam = PlayerBombGeneratorParam.LevelIndividualParam;

    /// <summary>
    /// プレイヤーボムのジェネレータ。
    /// </summary>
    public class PlayerBombGenerator : BattleRealBulletGeneratorBase
    {
        #region Param Field

        private string ShotTransformName;
        private Transform ShotTransform;

        private BattleRealPlayerBomb Bomb;
        private BulletParam BombParam;

        private IndividualParam[] IndividualParams;

        #endregion

        #region Field

        private int m_GenerateCount;
        private int m_DestroyCount;
        private Timer m_GenerateTimer;
        private Dictionary<BulletController, int> m_Bombs;
        private List<IDisposable> m_OnDestroyBombs;

        private IndividualParam m_ChargeLevelParam;
        private BattleRealChargeShotLevelData m_ConstantChargeLevelParam;

        #endregion

        protected override void OnSetParam()
        {
            if (Param is PlayerBombGeneratorParam p)
            {
                ShotTransformName = p.ShotTransformName;

                Bomb = p.Bomb;
                BombParam = p.BombParam;

                IndividualParams = p.LevelIndividualParams;
            }
            else
            {
                Debug.LogErrorFormat("渡されたパラメータが適切な型ではありません。 Generator:{0}, GeneratorParam:{1}", GetType().Name, Param.GetType().Name);
            }
        }

        #region Game Cycle

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void OnFinalize()
        {
            m_GenerateTimer?.DestroyTimer();

            if (m_OnDestroyBombs != null)
            {
                m_OnDestroyBombs.ForEach(d => d.Dispose());
                m_OnDestroyBombs = null;
            }

            ShotTransform = null;
            base.OnFinalize();
        }

        public override void OnStart()
        {
            base.OnStart();

            ShotTransform = Owner.transform.Find(ShotTransformName, false);
            if (ShotTransform == null)
            {
                Debug.LogErrorFormat("このキャラには指定されたトランスフォームがありません。 name : {0}", ShotTransformName);
                Destroy();
                return;
            }

            if (IndividualParams == null)
            {
                Debug.LogErrorFormat("レベルごとのパラメータがありません。");
                Destroy();
                return;
            }

            m_GenerateCount = 0;
            m_DestroyCount = 0;
            m_Bombs = new Dictionary<BulletController, int>();
            m_OnDestroyBombs = new List<IDisposable>();

            m_ChargeLevelParam = GetIndividualParam();
            m_ConstantChargeLevelParam = DataManager.Instance.BattleData.GetCurrentChargeLevelParam();

            GenerateData();
        }

        protected override void OnGeneratorUpdate(float deltaTime) { }

        #endregion

        private IndividualParam GetIndividualParam()
        {
            var level = DataManager.Instance.BattleData.ChargeLevel.Value;
            var length = IndividualParams.Length;
            return IndividualParams[Mathf.Min(level, length - 1)];
        }

        private void GenerateData()
        {
            if (m_ChargeLevelParam.ShotNum < 1)
            {
                Destroy();
                return;
            }

            // IntervalCallBackは0sからコールバックしてくれないので、最初の一回は普通に呼ぶ
            Shot();

            if (m_ChargeLevelParam.ShotNum > 1)
            {
                m_GenerateTimer = Timer.CreateIntervalTimer(E_TIMER_TYPE.SCALED_TIMER, m_ChargeLevelParam.ShotInterval);
                m_GenerateTimer.SetIntervalCallBack(Shot);
                BattleRealTimerManager.Instance.RegistTimer(m_GenerateTimer);
            }
        }

        private void Shot()
        {
            var camera = BattleRealCameraManager.Instance.GetCameraController(E_CAMERA_TYPE.FRONT_CAMERA);
            var viewportPos = camera.Camera.WorldToViewportPoint(ShotTransform.position);
            var pos = BattleRealStageManager.Instance.GetPositionFromFieldViewPortPosition(viewportPos.x, viewportPos.y);

            var shotParam = new BulletGeneratorShotParam()
            {
                BulletOwner = Owner,
                Bullet = Bomb,
                BulletParam = BombParam,
                Position = pos,
                Rotation = 0f,
                Scale = Vector3.one
            };
            var bomb = BulletController.ShotBullet(shotParam);

            bomb.SetNowDamage(m_ConstantChargeLevelParam.LaserDamagePerSeconds);
            var destroyBomb = bomb.OnDestroyObservable.Subscribe(_ => OnDestroyBomb(bomb));
            m_Bombs.Add(bomb, m_GenerateCount);
            m_OnDestroyBombs.Add(destroyBomb);

            m_GenerateCount++;
            if (m_GenerateCount >= m_ChargeLevelParam.ShotNum)
            {
                m_GenerateTimer?.DestroyTimer();
            }
        }

        private void OnDestroyBomb(BulletController bomb)
        {
            if (m_Bombs != null && m_OnDestroyBombs != null)
            {
                var index = m_Bombs[bomb];
                m_OnDestroyBombs[index].Dispose();
            }

            m_DestroyCount++;
            if (m_DestroyCount >= m_ChargeLevelParam.ShotNum)
            {
                Destroy();
            }
        }

        public void StopChargeShot()
        {
            if (m_Bombs != null)
            {
                foreach (var b in m_Bombs.Keys)
                {
                    b.DestroyBullet();
                }
                m_Bombs = null;
            }
        }
    }
}

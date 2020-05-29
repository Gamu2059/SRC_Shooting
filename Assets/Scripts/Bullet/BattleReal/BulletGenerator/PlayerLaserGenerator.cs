using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace BattleReal.BulletGenerator
{
    using IndividualParam = PlayerLaserGeneratorParam.LevelIndividualParam;

    /// <summary>
    /// プレイヤーレーザーのジェネレータ。
    /// </summary>
    public class PlayerLaserGenerator : BattleRealBulletGeneratorBase
    {
        #region Param Field

        private string ShotTransformName;
        private Transform ShotTransform;

        private IndividualParam[] IndividualParams;

        #endregion

        #region Field

        private BulletController m_Laser;
        private IDisposable m_OnDestroyLaser;

        #endregion

        protected override void OnSetParam()
        {
            if (Param is PlayerLaserGeneratorParam p)
            {
                ShotTransformName = p.ShotTransformName;
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
            m_OnDestroyLaser?.Dispose();
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

            Debug.Log(11);
            Shot();
        }

        protected override void OnGeneratorUpdate(float deltaTime) { }

        #endregion

        private IndividualParam GetIndividualParam()
        {
            var level = DataManager.Instance.BattleData.ChargeLevel.Value;
            var length = IndividualParams.Length;
            return IndividualParams[Mathf.Min(level, length - 1)];
        }

        private void Shot()
        {
            var levelParam = GetIndividualParam();
            var playerLevelParam = DataManager.Instance.BattleData.GetCurrentLevelParam();

            var camera = BattleRealCameraManager.Instance.GetCameraController(E_CAMERA_TYPE.FRONT_CAMERA);
            var viewportPos = camera.Camera.WorldToViewportPoint(ShotTransform.position);
            var pos = BattleRealStageManager.Instance.GetPositionFromFieldViewPortPosition(viewportPos.x, viewportPos.y);

            var shotParam = new BulletGeneratorShotParam()
            {
                BulletOwner = Owner,
                Bullet = levelParam.Laser,
                BulletParam = levelParam.LaserParam,
                Position = pos,
                Rotation = 0f,
                Scale = Vector3.one
            };
            m_Laser = BulletController.ShotBullet(shotParam);

            // 現状は、レーザータイプの通常弾だけを使う
            m_Laser.SetNowDamage(playerLevelParam.LaserDamagePerSeconds);
            m_OnDestroyLaser = m_Laser.OnDestroyObservable.Subscribe(_ => Destroy());
        }

        public void StopChargeShot()
        {
            m_Laser?.DestroyBullet();
            m_Laser = null;
        }
    }
}

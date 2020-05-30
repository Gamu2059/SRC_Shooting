using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleReal.BulletGenerator
{
    using IndividualParam = PlayerNormalBulletGeneratorParam.LevelIndividualParam;

    /// <summary>
    /// プレイヤー通常弾のジェネレータ。
    /// </summary>
    public class PlayerNormalBulletGenerator : BattleRealBulletGeneratorBase
    {
        #region Param Field

        private string MainShotTransformName;
        private Transform MainShotTransform;
        private string LeftSideShotTransformName;
        private Transform LeftSideShotTransform;
        private string RightSideShotTransformName;
        private Transform RightSideShotTransform;

        private BattleRealPlayerMainBullet MainShotBullet;
        private BattleRealPlayerSideBullet SideShotBullet;

        private BulletParam MainShotParam;
        private BulletParam SideShotParam;

        private IndividualParam[] IndividualParams;

        #endregion

        #region Field

        private bool m_IsStartShot;
        private float m_MainShotTimeCount;
        private float m_SideShotTimeCount;

        #endregion

        protected override void OnSetParam()
        {
            if (Param is PlayerNormalBulletGeneratorParam p)
            {
                MainShotTransformName = p.MainShotTransformName;
                LeftSideShotTransformName = p.LeftSideShotTransformName;
                RightSideShotTransformName = p.RightSideShotTransformName;

                MainShotBullet = p.MainShotBullet;
                SideShotBullet = p.SideShotBullet;

                MainShotParam = p.MainShotParam;
                SideShotParam = p.SideShotParam;

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
            RightSideShotTransform = null;
            LeftSideShotTransform = null;
            MainShotTransform = null;
            base.OnFinalize();
        }

        public override void OnStart()
        {
            base.OnStart();

            MainShotTransform = Owner.transform.Find(MainShotTransformName, false);
            LeftSideShotTransform = Owner.transform.Find(LeftSideShotTransformName, false);
            RightSideShotTransform = Owner.transform.Find(RightSideShotTransformName, false);

            if (
                !CheckExistTransform(MainShotTransform, MainShotTransformName) ||
                !CheckExistTransform(LeftSideShotTransform, LeftSideShotTransformName) ||
                !CheckExistTransform(RightSideShotTransform, RightSideShotTransformName))
            {
                Destroy();
                return;
            }

            if (IndividualParams == null)
            {
                Debug.LogErrorFormat("レベルごとのパラメータがありません。");
                Destroy();
                return;
            }

            m_IsStartShot = false;
            m_MainShotTimeCount = 0;
            m_SideShotTimeCount = 0;
        }

        private bool CheckExistTransform(Transform t, string n)
        {
            if (t == null)
            {
                Debug.LogErrorFormat("このキャラには指定されたトランスフォームがありません。 name : {0}", n);
                return false;
            }

            return true;
        }

        protected override void OnGeneratorUpdate(float deltaTime)
        {
            if (!m_IsStartShot)
            {
                return;
            }

            var levelParam = GetIndividualParam();
            var playerLevelParam = DataManager.Instance.BattleData.GetCurrentPlayerLevelParam();

            if (m_MainShotTimeCount >= levelParam.MainShotInterval)
            {
                var damage = playerLevelParam.MainShotDamage;
                var downDamge = playerLevelParam.MainShotDownDamage;
                m_MainShotTimeCount -= levelParam.MainShotInterval;
                Shot(MainShotTransform, MainShotBullet, MainShotParam, damage, downDamge);

                AudioManager.Instance.Play(E_COMMON_SOUND.PLAYER_SHOT_01);
            }

            if (levelParam.UseSideShot && m_SideShotTimeCount >= levelParam.SideShotInterval)
            {
                var damage = playerLevelParam.SideShotDamage;
                var downDamge = playerLevelParam.SideShotDownDamage;
                Debug.LogFormat("d : {0} dd: {1}", damage, downDamge);
                m_SideShotTimeCount -= levelParam.SideShotInterval;
                Shot(LeftSideShotTransform, SideShotBullet, SideShotParam, damage, downDamge);
                Shot(RightSideShotTransform, SideShotBullet, SideShotParam, damage, downDamge);
            }

            m_MainShotTimeCount += deltaTime;
            if (levelParam.UseSideShot)
            {
                m_SideShotTimeCount += deltaTime;
            }
        }

        #endregion

        private IndividualParam GetIndividualParam()
        {
            var level = DataManager.Instance.BattleData.LevelInChapter.Value;
            var length = IndividualParams.Length;
            return IndividualParams[Mathf.Min(level, length - 1)];
        }

        private void Shot(Transform t, BulletController bulletPrefab, BulletParam bulletParam, float damage, float downDamage)
        {
            var camera = BattleRealCameraManager.Instance.GetCameraController(E_CAMERA_TYPE.FRONT_CAMERA);
            var viewportPos = camera.Camera.WorldToViewportPoint(t.position);
            var pos = BattleRealStageManager.Instance.GetPositionFromFieldViewPortPosition(viewportPos.x, viewportPos.y);

            var shotParam = new BulletGeneratorShotParam()
            {
                BulletOwner = Owner,
                Bullet = bulletPrefab,
                BulletParam = bulletParam,
                Position = pos,
                Rotation = 0f,
                Scale = Vector3.one
            };
            var bullet = BulletController.ShotBullet(shotParam);

            bullet.SetNowDamage(damage);
            if (bullet is IBattleRealPlayerNormalBullet playerBullet)
            {
                playerBullet.SetNowDownDamage(downDamage);
            }
        }

        public void StartShot()
        {
            if (m_IsStartShot)
            {
                return;
            }

            m_IsStartShot = true;
            m_MainShotTimeCount = 0;
        }

        public void StopShot()
        {
            if (!m_IsStartShot)
            {
                return;
            }

            m_IsStartShot = false;
        }
    }
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// 円周上から弾を発射するジェネレータ。
    /// </summary>
    public class CircleNwayBulletGenerator : BattleRealBulletGeneratorBase
    {
        #region Param Field

        private BulletController[] Bullets;
        private BulletParam[] BulletParams;

        private string ShotTransformName;
        private Transform ShotTransform;
        private int ShotPointNum;
        private float ShotPointRadius;
        private float ShotPointOffsetAngleFromTransform;
        private float ShotPointAngleSpeedFromTransform;

        private SequentialEffectParamSet ShotOverallStartEffect;
        private EffectParamSet ShotEffect;

        private int ShotNum;
        private float ShotOffsetTime;
        private float ShotInterval;

        #endregion

        #region Field

        private int m_ShotCount;
        private bool m_IsWaitOffsetTime;
        private float m_ShotTimeCount;
        private float m_CurrentAngleFromTransform;
        private float m_ShotPointAngleUnit;

        #endregion

        protected override void OnSetParam()
        {
            if (Param is CircleNwayBulletGeneratorParam p)
            {
                Bullets = p.Bullets;
                BulletParams = p.BulletParams;

                ShotTransformName = p.ShotTransformName;
                ShotPointNum = p.ShotPointNum;
                ShotPointRadius = p.ShotPointRadius;
                ShotPointOffsetAngleFromTransform = p.ShotPointOffsetAngleFromTransform;
                ShotPointAngleSpeedFromTransform = p.ShotPointAngleSpeedFromTransform;

                ShotOverallStartEffect = p.ShotOverallStartEffect;
                ShotEffect = p.ShotEffect;

                ShotNum = p.ShotNum;
                ShotOffsetTime = p.ShotOffsetTime;
                ShotInterval = p.ShotInterval;
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
            ShotTransform = null;
            base.OnFinalize();
        }

        public override void OnStart()
        {
            base.OnStart();
            if (Bullets == null || Bullets.Length < 1)
            {
                Debug.LogErrorFormat("弾プレハブ配列がnullまたは要素数0です。");
                Destroy();
                return;
            }

            if (BulletParams == null || BulletParams.Length < 1)
            {
                Debug.LogErrorFormat("弾パラメータ配列がnullまたは要素数0です。");
                Destroy();
                return;
            }

            ShotTransform = Owner.transform.Find(ShotTransformName, false);
            if (ShotTransform == null)
            {
                Debug.LogErrorFormat("このキャラには指定されたトランスフォームがありません。 name : {0}", ShotTransformName);
                Destroy();
                return;
            }

            if (ShotNum < 1)
            {
                Destroy();
                return;
            }

            m_ShotCount = 0;
            m_IsWaitOffsetTime = ShotOffsetTime > 0;
            m_ShotTimeCount = 0;
            m_ShotPointAngleUnit = 360f / ShotPointNum;

            var f = ShotTransform.forward;
            m_CurrentAngleFromTransform = Mathf.Atan2(f.z, f.x).RadToDeg() + ShotPointOffsetAngleFromTransform;

            BattleRealEffectManager.Instance.RegisterSequentialEffect(ShotOverallStartEffect, Owner.transform, true);
        }

        protected override void OnGeneratorUpdate(float deltaTime)
        {
            if (m_ShotCount >= ShotNum)
            {
                Destroy();
                return;
            }

            if (m_IsWaitOffsetTime)
            {
                if (m_ShotTimeCount >= ShotOffsetTime)
                {
                    m_IsWaitOffsetTime = false;
                    m_ShotTimeCount -= ShotOffsetTime;
                }
                else
                {
                    m_ShotTimeCount += deltaTime;
                }
            }
            else
            {
                while (m_ShotTimeCount >= ShotInterval && m_ShotCount < ShotNum)
                {
                    Shot();
                    m_ShotTimeCount -= ShotInterval;
                    m_ShotCount++;
                }

                m_ShotTimeCount += deltaTime;
            }

            m_CurrentAngleFromTransform += ShotPointAngleSpeedFromTransform * deltaTime;
        }

        #endregion

        private void Shot()
        {
            var camera = BattleRealCameraManager.Instance.GetCameraController(E_CAMERA_TYPE.FRONT_CAMERA);
            var viewportPos = camera.Camera.WorldToViewportPoint(ShotTransform.position);
            var pos = BattleRealStageManager.Instance.GetPositionFromFieldViewPortPosition(viewportPos.x, viewportPos.y);

            for (var i = 0; i < ShotPointNum; i++)
            {
                var mathAngle = m_ShotPointAngleUnit * i + m_CurrentAngleFromTransform;
                var phase = mathAngle.DegToRad();
                var deltaPos = new Vector3(Mathf.Cos(phase), 0, Mathf.Sin(phase)) * ShotPointRadius;

                var shotParam = new BulletGeneratorShotParam()
                {
                    BulletOwner = Owner,
                    Bullet = Bullets[i % Bullets.Length],
                    BulletParam = BulletParams[i % BulletParams.Length],
                    Position = pos + deltaPos,
                    Rotation = mathAngle.MathAngleToUnityObjectAngle(),
                    Scale = Vector3.one
                };
                BulletController.ShotBullet(shotParam);

                var effect = BattleRealEffectManager.Instance.CreateEffect(ShotEffect, ShotTransform);
                if (effect != null)
                {
                    effect.transform.position = pos + deltaPos;
                    var effectPhase = (m_ShotPointAngleUnit * i + ShotPointOffsetAngleFromTransform).DegToRad();
                    var effectDeltaPos = new Vector3(Mathf.Cos(effectPhase), 0, Mathf.Sin(effectPhase)) * ShotPointRadius;
                    effect.RelatedAllowPos = effectDeltaPos;
                }
            }
        }
    }
}

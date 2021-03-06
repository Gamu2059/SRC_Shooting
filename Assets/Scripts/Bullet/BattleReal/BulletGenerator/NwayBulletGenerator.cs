﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace BattleReal.BulletGenerator
{
    using E_AXIS_TYPE = NwayBulletGeneratorParam.E_AXIS_TYPE;

    /// <summary>
    /// Nway弾のジェネレータ。
    /// </summary>
    public class NwayBulletGenerator : BattleRealBulletGeneratorBase
    {
        #region Define

        private struct ShotPhaseData
        {
            public BulletController Bullet;
            public BulletParam BulletParam;
            public float ShotTime;
            public bool IsCenterShot;
            public float ShotRaduis;
            public float ShotAngle;
        }

        #endregion

        #region Param Field

        private BulletController[] Bullets;
        private BulletParam[] BulletParams;

        private string ShotTransformName;
        private Transform ShotTransform;
        private int ShotNum;
        private E_AXIS_TYPE AxisType;
        private float AxisOffsetAngle;
        private EffectParamSet ShotEffect;

        private float ShotRadius;
        private float ShotDeltaRadius;
        private float ShotDeltaDeltaRadius;

        private float ShotOffsetTime;
        private float ShotInterval;
        private float ShotDeltaInterval;
        private float ShotDeltaDeltaInterval;

        private float ShotAngle;
        private float ShotDeltaAngle;
        private float ShotDeltaDeltaAngle;

        #endregion

        #region Field

        private Vector3 m_ShotBasePosition;
        private float m_ShotBaseAngle;

        private List<ShotPhaseData> m_ShotPhaseDataList;
        private float m_ShotTimeCount;
        private bool m_IsFirstShot;

        #endregion

        protected override void OnSetParam()
        {
            if (Param is NwayBulletGeneratorParam p)
            {
                Bullets = p.Bullets;
                BulletParams = p.BulletParams;

                ShotTransformName = p.ShotTransformName;
                ShotNum = p.ShotNum;
                AxisType = p.AxisType;
                AxisOffsetAngle = p.AxisOffsetAngle;
                ShotEffect = p.ShotEffect;

                ShotRadius = p.ShotRadius;
                ShotDeltaRadius = p.ShotDeltaRadius;
                ShotDeltaDeltaRadius = p.ShotDeltaDeltaRadius;

                ShotOffsetTime = p.ShotOffsetTime;
                ShotInterval = p.ShotInterval;
                ShotDeltaInterval = p.ShotDeltaInterval;
                ShotDeltaDeltaInterval = p.ShotDeltaDeltaInterval;

                ShotAngle = p.ShotAngle;
                ShotDeltaAngle = p.ShotDeltaAngle;
                ShotDeltaDeltaAngle = p.ShotDeltaDeltaAngle;
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

            // 発射データを作成する
            var shotPhaseNum = Mathf.CeilToInt(ShotNum / 2f);

            var shotRadius = ShotRadius;
            var deltaRadius = ShotDeltaRadius;

            var shotTime = ShotOffsetTime;
            var shotInterval = ShotInterval;
            var deltaInterval = ShotDeltaInterval;

            var shotAngle = ShotNum % 2 > 1 ? 0f : ShotAngle / 2f;
            var shotAngleInerval = ShotAngle;
            var deltaAngleInerval = ShotDeltaAngle;

            m_ShotPhaseDataList = new List<ShotPhaseData>();
            for (var i = 0; i < shotPhaseNum; i++)
            {
                var data = new ShotPhaseData();

                data.Bullet = Bullets[i % Bullets.Length];
                data.BulletParam = BulletParams[i % BulletParams.Length];

                data.IsCenterShot = i == 0 && ShotNum % 2 > 0;
                data.ShotRaduis = shotRadius;
                data.ShotTime = shotTime;
                data.ShotAngle = shotAngle;

                shotRadius += deltaRadius;
                deltaRadius += ShotDeltaDeltaRadius;

                shotTime += shotInterval;
                shotInterval += deltaInterval;
                deltaInterval += ShotDeltaDeltaInterval;

                shotAngle += shotAngleInerval;
                shotAngleInerval += deltaAngleInerval;
                deltaAngleInerval += ShotDeltaDeltaAngle;

                m_ShotPhaseDataList.Add(data);
            }

            m_IsFirstShot = true;
        }

        protected override void OnGeneratorUpdate(float deltaTime)
        {
            if (m_ShotPhaseDataList.Count < 1)
            {
                Destroy();
                return;
            }

            foreach (var data in m_ShotPhaseDataList)
            {
                if (m_ShotTimeCount >= data.ShotTime)
                {
                    Shot(data);
                }
            }

            m_ShotPhaseDataList.RemoveAll(d => m_ShotTimeCount >= d.ShotTime);
            m_ShotTimeCount += deltaTime;
        }

        #endregion

        /// <summary>
        /// 発射の基準となる位置と角度を確定させる
        /// </summary>
        private void DecideShotPositionAndAngle()
        {
            var camera = BattleRealCameraManager.Instance.GetCameraController(E_CAMERA_TYPE.FRONT_CAMERA);
            var viewportPos = camera.Camera.WorldToViewportPoint(ShotTransform.position);
            m_ShotBasePosition = BattleRealStageManager.Instance.GetPositionFromFieldViewPortPosition(viewportPos.x, viewportPos.y);

            if (AxisType == E_AXIS_TYPE.TRANSFORM_FORWARD)
            {
                var f = ShotTransform.forward;
                var angle = Mathf.Atan2(f.z, f.x);
                m_ShotBaseAngle = angle.RadToDeg().MathAngleToUnityObjectAngle() + AxisOffsetAngle;
            }
            else
            {
                var player = BattleRealPlayerManager.Instance.Player.transform;
                var f = player.position - ShotTransform.position;
                var angle = Mathf.Atan2(f.z, f.x);
                m_ShotBaseAngle = angle.RadToDeg().MathAngleToUnityObjectAngle() + AxisOffsetAngle;
            }
        }

        private void Shot(ShotPhaseData data)
        {
            if (m_IsFirstShot)
            {
                m_IsFirstShot = false;
                DecideShotPositionAndAngle();
            }

            if (data.IsCenterShot)
            {
                Shot(data, 0);
            }
            else
            {
                Shot(data, data.ShotAngle);
                Shot(data, -data.ShotAngle);
            }
        }

        private void Shot(ShotPhaseData data, float shotAngle)
        {
            var unityAngle = m_ShotBaseAngle + shotAngle;
            var radius = data.ShotRaduis;

            var phase = unityAngle.UnityObjectAngleToMathAngle().DegToRad();
            var deltaPos = new Vector3(Mathf.Cos(phase), 0, Mathf.Sin(phase)) * radius;
            var pos = m_ShotBasePosition + deltaPos;

            var shotParam = new BulletGeneratorShotParam()
            {
                BulletOwner = Owner,
                Bullet = data.Bullet,
                BulletParam = data.BulletParam,
                Position = pos,
                Rotation = unityAngle,
                Scale = Vector3.one
            };
            BulletController.ShotBullet(shotParam);

            var effect = BattleRealEffectManager.Instance.CreateEffect(ShotEffect, ShotTransform);
            if (effect != null)
            {
                effect.transform.position = pos;
                var effectPhase = shotAngle.UnityObjectAngleToMathAngle().DegToRad();
                var effectDeltaPos = new Vector3(Mathf.Cos(effectPhase), 0, Mathf.Sin(effectPhase)) * radius;
                effect.RelatedAllowPos = effectDeltaPos;
            }
        }
    }
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// 座標指定弾のジェネレータ。
    /// </summary>
    public class PointTweenBulletGenerator : BattleRealBulletGeneratorBase
    {
        #region Define

        private struct EffectData
        {
            public float Time;
            public Vector3 Position;
        }

        private struct ShotPhaseData
        {
            public BulletController Bullet;
            public BulletParam BulletParam;
            public float ShotTime;
            public Vector3 ShotPosition;
            public float ShotAngle;
        }

        #endregion

        #region Param Field

        private BulletController[] Bullets;
        private BulletParam[] BulletParams;

        private Vector3 BeginPoint;
        private Vector3 EndPoint;
        private int ShotNum;

        private float ShotOffsetTime;
        private float ShotInterval;
        private float ShotDeltaInterval;
        private float ShotDeltaDeltaInterval;

        private float ShotAngle;
        private float ShotDeltaAngle;
        private float ShotDeltaDeltaAngle;

        private EffectParamSet ShotBeforeEffect;
        private float AppearBeforeTimeShotBeforeEffect;
        private EffectParamSet ShotEffect;

        #endregion

        #region Field

        private List<ShotPhaseData> m_ShotPhaseDataList;
        private List<EffectData> m_EffectDataList;
        private float m_ShotTimeCount;

        #endregion

        protected override void OnSetParam()
        {
            if (Param is PointTweenBulletGeneratorParam p)
            {
                Bullets = p.Bullets;
                BulletParams = p.BulletParams;

                BeginPoint = p.BeginPoint;
                EndPoint = p.EndPoint;
                ShotNum = p.ShotNum;

                ShotOffsetTime = p.ShotOffsetTime;
                ShotInterval = p.ShotInterval;
                ShotDeltaInterval = p.ShotDeltaInterval;
                ShotDeltaDeltaInterval = p.ShotDeltaDeltaInterval;

                ShotAngle = p.ShotAngle;
                ShotDeltaAngle = p.ShotDeltaAngle;
                ShotDeltaDeltaAngle = p.ShotDeltaDeltaAngle;

                ShotBeforeEffect = p.ShotBeforeEffect;
                AppearBeforeTimeShotBeforeEffect = p.AppearBeforeTimeShotBeforeEffect;
                ShotEffect = p.ShotEffect;
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
            base.OnFinalize();
        }

        public override void OnStart()
        {
            base.OnStart();

            if (ShotNum < 1)
            {
                Destroy();
                return;
            }

            // 発射データとエフェクトデータを作成する
            var shotTime = ShotOffsetTime;
            var shotInterval = ShotInterval;
            var deltaInterval = ShotDeltaInterval;

            var shotAngle = ShotAngle;
            var shotAngleInerval = ShotDeltaAngle;

            Vector3 deltaPosition;
            if (ShotNum < 2)
            {
                deltaPosition = (EndPoint - BeginPoint) / 2;
            }
            else
            {
                deltaPosition = (EndPoint - BeginPoint) / (ShotNum - 1);
            }

            var camera = BattleRealCameraManager.Instance.GetCameraController(E_CAMERA_TYPE.FRONT_CAMERA);
            m_ShotPhaseDataList = new List<ShotPhaseData>();
            m_EffectDataList = new List<EffectData>();
            for (var i = 0; i < ShotNum; i++)
            {
                // ショットデータの作成
                var shotData = new ShotPhaseData();

                shotData.Bullet = Bullets[i % Bullets.Length];
                shotData.BulletParam = BulletParams[i % BulletParams.Length];

                shotData.ShotTime = shotTime;
                shotData.ShotAngle = shotAngle;

                var viewportPos = camera.Camera.WorldToViewportPoint(deltaPosition * i + BeginPoint);
                shotData.ShotPosition = BattleRealStageManager.Instance.GetPositionFromFieldViewPortPosition(viewportPos.x, viewportPos.y);

                shotTime += shotInterval;
                shotInterval += deltaInterval;
                deltaInterval += ShotDeltaDeltaInterval;

                shotAngle += shotAngleInerval;
                shotAngleInerval += ShotDeltaDeltaAngle;

                m_ShotPhaseDataList.Add(shotData);

                // エフェクトデータの作成
                var effectData = new EffectData();
                effectData.Time = shotData.ShotTime - AppearBeforeTimeShotBeforeEffect;
                effectData.Position = shotData.ShotPosition;
                m_EffectDataList.Add(effectData);
            }
        }

        protected override void OnGeneratorUpdate()
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

            foreach (var data in m_EffectDataList)
            {
                if (m_ShotTimeCount >= data.Time)
                {
                    var effect = BattleRealEffectManager.Instance.CreateEffect(ShotBeforeEffect, Owner.transform);
                    if (effect != null)
                    {
                        effect.transform.position = data.Position;
                    }
                }
            }

            m_ShotPhaseDataList.RemoveAll(d => m_ShotTimeCount >= d.ShotTime);
            m_EffectDataList.RemoveAll(d => m_ShotTimeCount >= d.Time);
            m_ShotTimeCount += Time.fixedDeltaTime;
        }

        #endregion

        private void Shot(ShotPhaseData data)
        {
            var shotParam = new BulletGeneratorShotParam()
            {
                BulletOwner = Owner,
                Bullet = data.Bullet,
                BulletParam = data.BulletParam,
                Position = data.ShotPosition,
                Rotation = data.ShotAngle,
                Scale = Vector3.one
            };
            BulletController.ShotBullet(shotParam);
            BattleRealEffectManager.Instance.CreateEffect(ShotEffect, Owner.transform);
        }
    }
}

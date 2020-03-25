#pragma warning disable 0649

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

            ShotTransform = Owner.transform.Find(ShotTransformName);
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
            var shotInterval = ShotInterval;
            var shotAngle = ShotAngle;
            var shotRadius = ShotRadius;
            var deltaInterval = ShotDeltaInterval;
            var deltaAngle = ShotDeltaAngle;
            var deltaRadius = ShotDeltaRadius;

            m_ShotPhaseDataList = new List<ShotPhaseData>();
            for (var i = 0; i < shotPhaseNum; i++)
            {
                var data = new ShotPhaseData();

                data.Bullet = Bullets[i % Bullets.Length];
                data.BulletParam = BulletParams[i % BulletParams.Length];

                data.IsCenterShot = i == 0 && ShotNum % 2 > 1;
                data.ShotRaduis = shotRadius;

                if (i == 0)
                {
                    data.ShotTime = ShotOffsetTime;
                    data.ShotAngle = ShotNum % 2 > 1 ? 0f : shotAngle / 2f;
                }
                else
                {
                    data.ShotTime = ShotOffsetTime + shotInterval;
                    data.ShotAngle = shotAngle;
                }

                shotInterval += Mathf.Max(deltaInterval, 0);
                shotAngle = deltaAngle;
                shotRadius = deltaRadius;
                deltaInterval += Mathf.Max(ShotDeltaDeltaInterval, 0);
                deltaAngle += ShotDeltaDeltaAngle;
                deltaRadius += ShotDeltaDeltaRadius;

                m_ShotPhaseDataList.Add(data);
            }

            m_IsFirstShot = true;
        }

        protected override void OnGeneratorUpdate()
        {
            foreach(var data in m_ShotPhaseDataList)
            {
                if (m_ShotTimeCount >= data.ShotTime)
                {
                    Shot(data);
                }
            }

            m_ShotPhaseDataList.RemoveAll( d => m_ShotTimeCount >= d.ShotTime);
            m_ShotTimeCount += Time.fixedDeltaTime;

            if (m_ShotPhaseDataList.Count < 1)
            {
                Destroy();
            }
        }

        #endregion

        /// <summary>
        /// 発射の基準となる位置と角度を確定させる
        /// </summary>
        private void DecideShotPositionAndAngle()
        {
            m_ShotBasePosition = ShotTransform.position;
            // 弾の方を優先的に見せたいため(シェーダーの描画順を変えることで今後は対処すべき)
            m_ShotBasePosition.y += 1;

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
            var angle = m_ShotBaseAngle + shotAngle;
            var radius = data.ShotRaduis;

            var posPhase = angle.UnityObjectAngleToMathAngle().DegToRad();
            var x = radius * Mathf.Cos(posPhase);
            var z = radius * Mathf.Sin(posPhase);
            var pos = m_ShotBasePosition + new Vector3(x, 0, z);

            var shotParam = new BulletGeneratorShotParam()
            {
                BulletOwner = Owner,
                Bullet = data.Bullet,
                BulletParam = data.BulletParam,
                Position = pos,
                Rotation = angle,
                Scale = Vector3.one
            };
            BulletController.ShotBullet(shotParam);
        }
    }
}

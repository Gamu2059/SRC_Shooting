#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.EnemyGenerator
{
    /// <summary>
    /// ある地点を指定してその円周上に敵を生成していくジェネレータ。
    /// </summary>
    [Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGroup/EnemyGenerator/CircleTween", fileName = "param.enemy_generator.asset", order = 20)]
    public class CircleTweenEnemyGenerator : BattleRealEnemyGeneratorBase
    {
        #region Define

        protected enum E_CENTER_POSITION_TYPE
        {
            /// <summary>
            /// 指定した座標を中心にする
            /// </summary>
            SPECIFY,

            /// <summary>
            /// プレイヤーの座標を中心にする
            /// </summary>
            PLAYER,
        }

        protected enum E_GENERATE_TIMING_TYPE
        {
            /// <summary>
            /// 敵を個別に生成する
            /// </summary>
            INDIVIDUAL,

            /// <summary>
            /// GenerateOffsetTimeが過ぎた時点で全て同時に生成する
            /// </summary>
            SAME,
        }

        #endregion

        #region Field Inspector

        [Header("Circle Tween Parameter")]

        [SerializeField, Tooltip("生成したい敵のパラメータ")]
        private BattleRealEnemyParamSetBase m_ParamSet;
        protected BattleRealEnemyParamSetBase ParamSet => m_ParamSet;

        [SerializeField, Tooltip("中心座標のタイプ")]
        private E_CENTER_POSITION_TYPE m_CenterPositionType;
        protected E_CENTER_POSITION_TYPE CenterPositionType => m_CenterPositionType;

        [SerializeField, Tooltip("中心座標を指定する場合の座標")]
        private Vector2 m_SpecifyCenterPosition;
        protected Vector2 SpecifyCenterPosition => m_SpecifyCenterPosition;

        [SerializeField, Tooltip("中心からの生成位置の半径")]
        private float m_Radius;
        protected float Radius => m_Radius;

        [SerializeField, Tooltip("最初に生成する敵の円周上での角度(度数法)。0が奥、90が右、180が前、270が左となる。")]
        private float m_BeginAngle;
        protected float BeginAngle => m_BeginAngle;

        [SerializeField, Tooltip("最後に生成する敵の円周上での角度(度数法)。0が奥、90が右、180が前、270が左となる。")]
        private float m_EndAngle;
        protected float EndAngle => m_EndAngle;

        [SerializeField, Tooltip("基本は中心を向いて生成されるが、そこからの相対的な角度を指定する")]
        private float m_GenerateAngle;
        protected float GenerateAngle => m_GenerateAngle;

        [SerializeField, Tooltip("敵の生成数。1以上でなけれなならない。1の時は開始座標と終了座標の間に生成される。"), Min(1)]
        private int m_GenerateNum;
        protected int GenerateNum => m_GenerateNum;

        [Space()]

        [SerializeField, Tooltip("敵の生成タイミングの種類。INDIVIDUALの場合、GenerateOffsetTimeを開始としてGenerateIntervalの経過ごとに1体ずつ生成ていく。SAMEの場合、GenerateOffsetTimeの経過で全て生成する。")]
        private E_GENERATE_TIMING_TYPE m_GenerateTimingType;
        protected E_GENERATE_TIMING_TYPE GenerateTimingType => m_GenerateTimingType;

        [SerializeField, Tooltip("敵の生成オフセット時間。生成の開始タイミングを遅延させることができる。"), Min(0)]
        private float m_GenerateOffsetTime;
        protected float GenerateOffsetTime => m_GenerateOffsetTime;

        [SerializeField, Tooltip("敵の生成間隔時間。"), Min(0)]
        private float m_GenerateInterval;
        protected float GenerateInterval => m_GenerateInterval;

        #endregion

        #region Field

        private bool m_IsCountOffset;
        private float m_GenerateTimeCount;
        private int m_GeneratedEnemyCount;
        private float m_BeginPhase;
        private float m_EndPhase;
        private float m_DeltaPhase;
        private Vector3 m_CenterPosition;
        private List<BattleRealEnemyBase> m_GeneratedEnemies;

        #endregion

        #region Game Cycle

        protected override void OnStartGenerator()
        {
            base.OnStartGenerator();

            m_IsCountOffset = true;
            m_GenerateTimeCount = 0;
            m_GeneratedEnemyCount = 0;
            m_GeneratedEnemies = new List<BattleRealEnemyBase>();

            m_BeginPhase = BeginAngle.UnityObjectAngleToMathAngle().DegToRad();
            m_EndPhase = EndAngle.UnityObjectAngleToMathAngle().DegToRad();
            if (GenerateNum < 2)
            {
                m_DeltaPhase = (m_EndPhase - m_BeginPhase) / 2;
            }
            else
            {
                m_DeltaPhase = (m_EndPhase - m_BeginPhase) / (GenerateNum - 1);
            }

            if (CenterPositionType == E_CENTER_POSITION_TYPE.SPECIFY)
            {
                m_CenterPosition = SpecifyCenterPosition.ToVector3XZ();
            }
            else
            {
                // プレイヤーと敵のフィールドの座標系が同じことが前提になっている
                m_CenterPosition = BattleRealPlayerManager.Instance.Player.transform.localPosition;
                m_CenterPosition.y = 0;
            }
        }

        protected override void OnLateUpdateGenerator()
        {
            base.OnLateUpdateGenerator();

            if (GenerateNum < 1)
            {
                EnemyGroup.Destory();
                return;
            }

            // 生成すべき敵を全て生成し、かつそれらの敵が全て消滅したならグループを破棄する
            if (m_GeneratedEnemyCount >= GenerateNum && m_GeneratedEnemies.Count < 1)
            {
                EnemyGroup.Destory();
                return;
            }

            m_GeneratedEnemies.RemoveAll(e => e.GetCycle() == E_POOLED_OBJECT_CYCLE.POOLED);
        }

        protected override void OnFixedUpdateGenerator()
        {
            base.OnFixedUpdateGenerator();

            if (m_GeneratedEnemyCount >= GenerateNum)
            {
                return;
            }

            switch (GenerateTimingType)
            {
                case E_GENERATE_TIMING_TYPE.INDIVIDUAL:
                    OnUpdateIndividualType();
                    break;
                case E_GENERATE_TIMING_TYPE.SAME:
                    OnUpdateSameType();
                    break;
            }
        }

        #endregion

        private void OnUpdateIndividualType()
        {
            if (m_IsCountOffset)
            {
                if (m_GenerateTimeCount >= GenerateOffsetTime)
                {
                    m_IsCountOffset = false;
                    m_GenerateTimeCount -= GenerateOffsetTime;
                    m_GenerateTimeCount += GenerateInterval;
                }
                else
                {
                    m_GenerateTimeCount += Time.fixedDeltaTime;
                }
            }
            else
            {
                if (m_GenerateTimeCount >= GenerateInterval)
                {
                    Generate();
                    m_GeneratedEnemyCount++;
                    m_GenerateTimeCount -= GenerateInterval;
                }
                m_GenerateTimeCount += Time.fixedDeltaTime;
            }
        }

        private void OnUpdateSameType()
        {
            if (m_IsCountOffset)
            {
                if (m_GenerateTimeCount >= GenerateOffsetTime)
                {
                    m_IsCountOffset = false;
                    for (var i = 0; i < GenerateNum; i++)
                    {
                        Generate();
                        m_GeneratedEnemyCount++;
                    }
                }
                else
                {
                    m_GenerateTimeCount += Time.fixedDeltaTime;
                }
            }
        }

        private void Generate()
        {
            var enemy = BattleRealEnemyManager.Instance.CreateEnemy(ParamSet);
            if (enemy == null)
            {
                return;
            }

            var enemyT = enemy.transform;
            enemyT.SetParent(EnemyGroup.transform, false);

            var phase = (GenerateNum < 2 ? m_DeltaPhase : m_DeltaPhase * m_GeneratedEnemyCount) + m_BeginPhase;
            var pos = new Vector3(Mathf.Cos(phase), 0, Mathf.Sin(phase)) * Radius + m_CenterPosition;

            // 数学上の角度からUnityのオブジェクトの角度へと変換
            var unityObjectAngle = phase.RadToDeg().MathAngleToUnityObjectAngle();
            var angles = new Vector3(0, unityObjectAngle + 180 + GenerateAngle, 0);

            enemyT.localPosition = pos;
            enemyT.localEulerAngles = angles;

            m_GeneratedEnemies.Add(enemy);
        }
    }
}

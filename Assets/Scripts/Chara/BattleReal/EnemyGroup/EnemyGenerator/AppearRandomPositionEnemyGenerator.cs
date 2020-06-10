#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace BattleReal.EnemyGenerator
{
    /// <summary>
    /// プレイヤーの現在位置から指定の距離離れたランダムな位置に敵を生成していくジェネレータ.
    /// </summary>
    [Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGroup/EnemyGenerator/AppearRandomPosition", fileName = "param.enemy_generator.asset", order = 20)]
    public class AppearRandomPositionEnemyGenerator : BattleRealEnemyGeneratorBase
    {
        #region Define

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

        [Header("Appear Random Position Parameter")]

        [SerializeField, Tooltip("生成したい敵のパラメータ")]
        private BattleRealEnemyParamSetBase m_ParamSet;
        protected BattleRealEnemyParamSetBase ParamSet => m_ParamSet;

        [SerializeField, Tooltip("プレイヤー位置から生成位置の半径")]
        private float m_OffsetRadius;
        protected float OffsetRadius => m_OffsetRadius;

        [SerializeField, Tooltip("敵の生成数")]
        private int m_GenerateNum;
        protected int GenerateNum => m_GenerateNum;

        [Space()]

        [SerializeField, Tooltip("敵の生成タイミングの種類")]
        private E_GENERATE_TIMING_TYPE m_GenerateTimingType;
        protected E_GENERATE_TIMING_TYPE GenerateTimingType => m_GenerateTimingType;

        [SerializeField, Tooltip("敵の生成オフセット時間")]
        private float m_GenerateOffsetTime;
        private float GenerateOffsetTime => m_GenerateOffsetTime;

        [SerializeField, Tooltip("敵の生成間隔時間"), Min(0)]
        private float m_GenerateInterval;
        protected float GenerateInterval => m_GenerateInterval;

        #endregion

        #region Field
        #endregion

        #region Game Cycle
        
        protected override void OnStartGenerator()
        {
            base.OnStartGenerator();
        }

        protected override void OnLateUpdateGenerator()
        {
            base.OnLateUpdateGenerator();
        }

        protected override void OnFixedUpdateGenerator()
        {
            base.OnFixedUpdateGenerator();
        }

        #endregion

    }
}

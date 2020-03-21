#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.EnemyGenerator
{
    /// <summary>
    /// 2点の座標を指定して、その間に敵を生成していくジェネレータ。
    /// </summary>
    [Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGenerator/CoorinateTween", fileName = "coordinate_tween.battle_real_enemy_generator.asset", order = 1)]
    public class CoordinateTweenEnemyGenerator : BattleRealEnemyGenerator
    {
        #region Field Inspector

        [SerializeField, Tooltip("生成したい敵のパラメータ")]
        private BattleRealEnemyParamSetBase m_ParamSet;
        public BattleRealEnemyParamSetBase ParamSet => m_ParamSet;

        [SerializeField, Tooltip("EnemyGroupがある場所に対しての相対的な開始座標")]
        private Vector2 m_BeginPosition;
        public Vector2 BeginPosition => m_BeginPosition;

        [SerializeField, Tooltip("EnemyGroupがある場所に対しての相対的な終了座標")]
        private Vector2 m_EndPosition;
        public Vector2 EndPosition => m_EndPosition;

        [SerializeField, Tooltip("敵の生成角度。EnemyGroupに対する相対角度")]
        private float m_GenerateAngle;
        public float GenerateAngle => m_GenerateAngle;

        [SerializeField, Tooltip("敵の生成数。1以上でなけれなならない。1の時は開始座標と終了座標の間に生成される。"), Min(1)]
        private int m_GenerateNum;
        public int GenerateNum => m_GenerateNum;

        [SerializeField, Tooltip("敵の生成オフセット時間。生成の開始タイミングを遅延させることができる。"), Min(0)]
        private float m_GenerateOffsetTime;
        public float GenerateOffsetTime => m_GenerateOffsetTime;

        [SerializeField, Tooltip("敵の生成間隔時間。"), Min(0)]
        private float m_GenerateInterval;
        public float GenerateInterval => m_GenerateInterval;

        #endregion

        #region Field

        private bool m_IsCountOffset;
        private float m_GenerateTimeCount;
        private int m_GenerateNumCount;
        private Vector2 m_DeltaPosition;

        #endregion

        #region Game Cycle

        public override void OnStart()
        {
            base.OnStart();

            m_IsCountOffset = true;
            m_GenerateTimeCount = 0;
            m_GenerateNumCount = 0;

            if (GenerateNum < 2)
            {
                m_DeltaPosition = (EndPosition - BeginPosition) / 2;
            }
            else
            {
                m_DeltaPosition = (EndPosition - BeginPosition) / (GenerateNum - 1);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_GenerateNumCount >= GenerateNum)
            {
                return;
            }

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
                    m_GenerateTimeCount += Time.deltaTime;
                }
            }
            else
            {
                if (m_GenerateTimeCount >= GenerateInterval)
                {
                    Generate();
                    m_GenerateNumCount++;
                    m_GenerateTimeCount -= GenerateOffsetTime;
                }
                m_GenerateTimeCount += Time.deltaTime;
            }
        }

        #endregion

        private void Generate()
        {
            var enemy = BattleRealEnemyManager.Instance.CreateEnemy(ParamSet);
            if (enemy == null)
            {
                return;
            }

            var enemyT = enemy.transform;
            enemyT.SetParent(EnemyGroup.transform);

            var pos = (GenerateNum < 2 ? m_DeltaPosition : m_DeltaPosition * m_GenerateNumCount) + BeginPosition;
            var angles = enemyT.localEulerAngles;
            angles.y = GenerateAngle;

            enemyT.localPosition = pos;
            enemyT.localEulerAngles = angles;
        }
    }
}

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
    [Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGenerator/CoorinateTween", fileName = "coordinate_tween.enemy_generator.asset", order = 10)]
    public class CoordinateTweenEnemyGenerator : BattleRealEnemyGenerator
    {
        #region Field Inspector

        [Header("Coordinate Tween Parameter")]

        [SerializeField, Tooltip("生成したい敵のパラメータ配列。この配列の順番で生成サイクルを回します。交互に敵を出したい場合は、要素数2にして異なるパラメータを入れます。")]
        private BattleRealEnemyParamSetBase[] m_ParamSets;
        protected BattleRealEnemyParamSetBase[] ParamSets => m_ParamSets;

        [SerializeField, Tooltip("EnemyGroupがある場所に対しての相対的な開始座標")]
        private Vector2 m_BeginPosition;
        protected Vector2 BeginPosition => m_BeginPosition;

        [SerializeField, Tooltip("EnemyGroupがある場所に対しての相対的な終了座標")]
        private Vector2 m_EndPosition;
        protected Vector2 EndPosition => m_EndPosition;

        [SerializeField, Tooltip("敵の生成角度。EnemyGroupに対する相対角度")]
        private float m_GenerateAngle;
        protected float GenerateAngle => m_GenerateAngle;

        [SerializeField, Tooltip("敵の生成数。1以上でなけれなならない。1の時は開始座標と終了座標の間に生成される。"), Min(1)]
        private int m_GenerateNum;
        protected int GenerateNum => m_GenerateNum;

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
        private Vector2 m_DeltaPosition;
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

            if (GenerateNum < 2)
            {
                m_DeltaPosition = (EndPosition - BeginPosition) / 2;
            }
            else
            {
                m_DeltaPosition = (EndPosition - BeginPosition) / (GenerateNum - 1);
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

        #endregion

        private void Generate()
        {
            var paramSet = ParamSets[m_GeneratedEnemyCount % ParamSets.Length];
            var enemy = BattleRealEnemyManager.Instance.CreateEnemy(paramSet);
            if (enemy == null)
            {
                return;
            }

            var enemyT = enemy.transform;
            enemyT.SetParent(EnemyGroup.transform, false);

            var pos = (GenerateNum < 2 ? m_DeltaPosition : m_DeltaPosition * m_GeneratedEnemyCount) + BeginPosition;
            var angles = new Vector3(0, GenerateAngle, 0);

            enemyT.localPosition = pos;
            enemyT.localEulerAngles = angles;

            m_GeneratedEnemies.Add(enemy);
        }
    }
}

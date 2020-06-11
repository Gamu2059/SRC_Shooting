#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using Rewired;
using System.Xml.Schema;

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

        /// <summary>
        /// プレイヤーがビューポート中心から見てどの位置にいるか
        /// </summary>
        private enum E_PLAYERS_VIEWPORT_POSITION 
        {
            UPPER_LEFT,

            UPPER_RIGHT,

            LOWER_RIGHT,

            LOWER_LEFT,
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

        private bool m_IsCountOffset;
        private float m_GenerateTimeCount;
        private int m_GeneratedEnemyCount;
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
        }

        protected override void OnLateUpdateGenerator()
        {
            base.OnLateUpdateGenerator();

            if(GenerateNum < 1)
            {
                EnemyGroup.Destory();
                return;
            }

            if(m_GeneratedEnemyCount >= GenerateNum && m_GeneratedEnemies.Count < 1)
            {
                EnemyGroup.Destory();
                return;
            }

            m_GeneratedEnemies.RemoveAll(e => e.GetCycle() == E_POOLED_OBJECT_CYCLE.POOLED);
        }

        protected override void OnFixedUpdateGenerator()
        {
            base.OnFixedUpdateGenerator();

            if(m_GeneratedEnemyCount >= GenerateNum)
            {
                return;
            }

            switch (GenerateTimingType) 
            {
                case E_GENERATE_TIMING_TYPE.INDIVIDUAL:
                    OnUpdateIndivisualType();
                    break;
                case E_GENERATE_TIMING_TYPE.SAME:
                    OnUpdateSameType();
                    break;
            }

        }

        #endregion

        private void OnUpdateIndivisualType()
        {
            if (m_IsCountOffset)
            {
                if(m_GenerateTimeCount >= GenerateOffsetTime)
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
                if(m_GenerateTimeCount >= GenerateInterval)
                {
                    Generate();
                    m_GeneratedEnemyCount--;
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
            
            if(enemy == null)
            {
                return;
            }

            var enemyT = enemy.transform;
            enemyT.SetParent(EnemyGroup.transform, false);

            enemyT.localPosition = GetEnemyPosition();

            m_GeneratedEnemies.Add(enemy);
        }

        private Vector3 GetEnemyPosition()
        {
            Vector2 min = BattleRealStageManager.Instance.MinLocalFieldPosition;
            Vector2 max = BattleRealStageManager.Instance.MaxLocalFieldPosition;
            Vector3 playerPos = BattleRealPlayerManager.Instance.Player.transform.position;

            var res = GetPlayersViewPortPosition(playerPos, min, max);

            if(res == E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT)
            {
                return new Vector3(UnityEngine.Random.Range(playerPos.x + m_OffsetRadius, max.x),
                                   0,
                                   UnityEngine.Random.Range(playerPos.z + m_OffsetRadius, min.y));
            }
            else if(res == E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT)
            {
                return new Vector3(UnityEngine.Random.Range(playerPos.x + m_OffsetRadius, min.x),
                                   0,
                                   UnityEngine.Random.Range(playerPos.z + m_OffsetRadius, max.y));
            }
            else if(res == E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT)
            {
                return new Vector3(UnityEngine.Random.Range(playerPos.x + m_OffsetRadius, max.x),
                                   0,
                                   UnityEngine.Random.Range(playerPos.z + m_OffsetRadius, min.y));
            }
            else
            {
                return new Vector3(UnityEngine.Random.Range(playerPos.x + m_OffsetRadius, min.x),
                                   0,
                                   UnityEngine.Random.Range(playerPos.z + m_OffsetRadius, max.y));
            }
        }

        private E_PLAYERS_VIEWPORT_POSITION GetPlayersViewPortPosition(Vector3 playerPos3D, Vector2 min, Vector2 max)
        {
            var pos2D = new Vector2(playerPos3D.x, playerPos3D.z);
            Vector2 center = (max - min) / 2.0f;

            int h = CalcPositionRelationship(center, center + Vector2.up, pos2D);
            int v = CalcPositionRelationship(Rotation(center, -90.0f * Mathf.Deg2Rad),
                                             Rotation(center + Vector2.up, -90.0f * Mathf.Deg2Rad),
                                             pos2D);

            if (h == 1 && v == 1)
            {
                return E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT;
            }
            else if (h == -1 && v == 1)
            {
                return E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT;
            }
            else if (h == -1 && v == -1)
            {
                return E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT;
            }
            else
            {
                return E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT;
            }
        }

        private Vector2 Rotation(Vector2 v, float rad)
        {
            return new Vector2(v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
                               v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad));
        }

        private int CalcPositionRelationship(Vector2 a, Vector2 b, Vector2 p)
        {
            Vector2 v = p - a;
            Vector2 w = b - a;
            float res = w.x * v.y - w.y * v.x;

            if (res >= 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}

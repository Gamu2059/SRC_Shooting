#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using Rewired;
using UnityEngine.UI;

namespace BattleReal.EnemyGenerator
{
    /// <summary>
    /// プレイヤーの現在位置から指定の距離離れたランダムな位置に敵を生成していくジェネレータ.
    /// </summary>
    [Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGroup/EnemyGenerator/AppearRandomPosition", fileName = "param.enemy_generator.asset", order = 20)]
    public class AppearRandomPositionEnemyGenerator : BattleRealEnemyGeneratorBase
    {
        #region Define

        private const float RAY_LENGTH = 1000.0f;

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

        [SerializeField, Tooltip("生成範囲の左上")]
        private Vector2 m_AppearAreaMin;

        [SerializeField, Tooltip("生成範囲の右下")]
        private Vector2 m_AppearAreaMax;

        [SerializeField, Tooltip("生成したい敵のパラメータ")]
        private BattleRealEnemyParamSetBase m_ParamSet;
        protected BattleRealEnemyParamSetBase ParamSet => m_ParamSet;

        [SerializeField, Range(0f, 1f), Tooltip("0に近いほどプレイヤー寄りに、1に近いほどフィールド端に出現する")]
        private float m_AppearPosFactor;
        protected float AppearPosFactor => m_AppearPosFactor;

        [SerializeField, Tooltip("敵の生成数")]
        private int m_GenerateNum;
        protected int GenerateNum => m_GenerateNum;

        [SerializeField, Tooltip("一度に複数の敵を生成するか?")]
        private bool m_EnableGeneratingMultipleEnemy;
        protected bool EnableGeneratingMultipleEnemy => m_EnableGeneratingMultipleEnemy;

        [SerializeField, Tooltip("一度に生成する数")]
        private int m_GenerateNumParOnce;
        protected int GenerateNumParOnce => m_GenerateNumParOnce;


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

        private Dictionary<E_PLAYERS_VIEWPORT_POSITION, Vector2> m_AppearAreaVertices;
        private List<Vector2> m_AppearAreaLines;
        private bool m_IsCountOffset;
        private float m_GenerateTimeCount;
        private int m_GeneratedEnemyCount;
        private List<BattleRealEnemyBase> m_GeneratedEnemies;

        #endregion

        #region Game Cycle
        
        protected override void OnStartGenerator()
        {
            base.OnStartGenerator();
          
            m_AppearAreaVertices = new Dictionary<E_PLAYERS_VIEWPORT_POSITION, Vector2> 
            {
                { E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT,  new Vector2(m_AppearAreaMin.x, m_AppearAreaMin.y)},
                { E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT,  new Vector2(m_AppearAreaMin.x, m_AppearAreaMax.y)},
                { E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT, new Vector2(m_AppearAreaMax.x, m_AppearAreaMax.y)},
                { E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT, new Vector2(m_AppearAreaMax.x, m_AppearAreaMin.y)},
            };
            m_AppearAreaLines = new List<Vector2> 
            {
                m_AppearAreaVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT],  m_AppearAreaVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT],  // up
                m_AppearAreaVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT], m_AppearAreaVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT],  // right
                m_AppearAreaVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT], m_AppearAreaVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT],   // bottom
                m_AppearAreaVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT],  m_AppearAreaVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT],   // left
            };
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
                    if (m_EnableGeneratingMultipleEnemy)
                    {
                        for(int i = 0; i < m_GenerateNumParOnce; i++)
                        {
                            Generate();                            
                        }
                        m_GeneratedEnemyCount += m_GenerateNumParOnce;
                        m_GenerateTimeCount -= GenerateInterval;
                    }
                    else
                    {
                        Generate();
                        m_GeneratedEnemyCount++;
                        m_GenerateTimeCount -= GenerateInterval;
                    }                                        
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

            Vector3 playerPos = BattleRealPlayerManager.Instance.Player.transform.position;
            Vector2 playerPos2D = new Vector2(playerPos.x, playerPos.z);
            
            var enemyPos = GetEnemyPosition(playerPos2D);
            enemyT.position = enemyPos;
            enemyT.LookAt(playerPos);

            m_GeneratedEnemies.Add(enemy);
        }

        private float CalcAppearMathAngle(E_PLAYERS_VIEWPORT_POSITION pvp)
        {
            float ang1, ang2;

            if(pvp == E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT)
            {
                ang1 = 0f;
                ang2 = 90.0f;
            }
            else if(pvp == E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT)
            {
                ang1 = 90.0f;
                ang2 = 180.0f;
            }
            else if(pvp == E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT)
            {
                ang1 = 270.0f;
                ang2 = 360.0f;
            }
            else
            {
                ang1 = 180.0f;
                ang2 = 270.0f;
            }

            return UnityEngine.Random.Range(ang1, ang2) * Mathf.Deg2Rad;
        }

        private Vector3 GetEnemyPosition(Vector2 playerPos2D)
        {                       
            var pvp = GetPlayersViewPortPosition(playerPos2D);

            float angle = CalcAppearMathAngle(pvp);

            float radius = CalcMaxRadius(angle, playerPos2D) * m_AppearPosFactor;

            angle.MathAngleToUnityObjectAngle();

            Vector2 pos2D = playerPos2D +  new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));

            return new Vector3(pos2D.x, 0, pos2D.y);
        }

        private Vector2? CalcIntersection(Vector2 from1, Vector2 to1, Vector2 from2, Vector2 to2) 
        {
            Vector2 v = from2 - from1;
            float crossV1V2 = Cross2D(to1 - from1, to2 - from2);

            if (crossV1V2 == 0)
            {
                return null;
            }

            float crossVV1 = Cross2D(v, to1 - from1);
            float crossVV2 = Cross2D(v, to2 - from2);

            float t1 = crossVV2 / crossV1V2;
            float t2 = crossVV1 / crossV1V2;

            if (t1 < 0 || t1 > 1 || t2 < 0 || t2 > 1)
            {
                return null;
            }
            else
            {
                return from1 + (to1 - from1) * t1;
            }
        }

        private float CalcMaxRadius(float angle, Vector2 playerPos2D) 
        {
            Vector2 ray = playerPos2D + new Vector2(RAY_LENGTH * Mathf.Cos(angle), RAY_LENGTH * Mathf.Sin(angle));
            Vector2 intersection = Vector2.zero;
            float dist;

            for(int i = 0; i < m_AppearAreaLines.Count - 1; i += 2)
            {
                Vector2? tmp = CalcIntersection(playerPos2D, ray, m_AppearAreaLines[i], m_AppearAreaLines[i + 1]);

                if (tmp != null)
                {
                    intersection = (Vector2)tmp;
                    break;
                }
            }

            dist = Vector2.Distance(playerPos2D, intersection);

            return dist;
        }

        private E_PLAYERS_VIEWPORT_POSITION GetPlayersViewPortPosition(Vector2 playerPos2D)
        {           
            float crossH = Cross2D(Vector2.down, playerPos2D);
            float crossV = Cross2D(Vector2.right, playerPos2D);

            if(crossH < 0)
            {
                if(crossV < 0)
                {
                    return E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT;
                }
                else
                {
                    return E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT;
                }
            }
            else
            {
                if(crossV < 0)
                {
                    return E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT;
                }
                else
                {
                    return E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT;
                }
            }
        }

        private float Cross2D(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }
    }
}

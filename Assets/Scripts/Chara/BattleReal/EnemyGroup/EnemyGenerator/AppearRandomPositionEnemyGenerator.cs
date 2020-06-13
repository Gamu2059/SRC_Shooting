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

        [SerializeField, Tooltip("生成したい敵のパラメータ")]
        private BattleRealEnemyParamSetBase m_ParamSet;
        protected BattleRealEnemyParamSetBase ParamSet => m_ParamSet;

        [SerializeField, Tooltip("敵が出現する最小の半径")]
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

        private Vector2 m_GameFieldMax;
        private Vector2 m_GameFieldMin;
        private Dictionary<E_PLAYERS_VIEWPORT_POSITION, Vector2> m_GameFieldVertices;
        private List<Vector2> m_GameFieldLines;
        private bool m_IsCountOffset;
        private float m_GenerateTimeCount;
        private int m_GeneratedEnemyCount;
        private List<BattleRealEnemyBase> m_GeneratedEnemies;

        #endregion

        #region Game Cycle
        
        protected override void OnStartGenerator()
        {
            base.OnStartGenerator();

            m_GameFieldMin = new Vector2(-1.0f, -1.0f);
            m_GameFieldMax = new Vector2(1.0f, 1.0f);            
            m_GameFieldVertices = new Dictionary<E_PLAYERS_VIEWPORT_POSITION, Vector2> 
            {
                { E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT,  new Vector2(m_GameFieldMin.x, m_GameFieldMin.y)},
                { E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT,  new Vector2(m_GameFieldMin.x, m_GameFieldMax.y)},
                { E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT, new Vector2(m_GameFieldMax.x, m_GameFieldMax.y)},
                { E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT, new Vector2(m_GameFieldMax.x, m_GameFieldMin.y)},
            };
            m_GameFieldLines = new List<Vector2> 
            {
                m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT],  m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT],  // up
                m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT], m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT],  // right
                m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT], m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT],   // bottom
                m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT],  m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT],   // left
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
            
            if(enemy == null)
            {
                return;
            }

            var enemyT = enemy.transform;
            enemyT.SetParent(EnemyGroup.transform, false);

            Vector3 playerPos = BattleRealPlayerManager.Instance.Player.transform.position;
            Vector2 playerPos2D = new Vector2(playerPos.x, playerPos.z);
            
            enemyT.localPosition = GetEnemyPosition(playerPos2D);
            enemyT.LookAt(playerPos);

            m_GeneratedEnemies.Add(enemy);
        }

        private float CalcAppearMathAngle(E_PLAYERS_VIEWPORT_POSITION pvp1, E_PLAYERS_VIEWPORT_POSITION pvp2, Vector2 direction, Vector2 playerPos2D) 
        {
            Vector2 v1 = m_GameFieldVertices[pvp1] - playerPos2D;
            Vector2 v2 = m_GameFieldVertices[pvp2] - playerPos2D;
            float ang1, ang2, finalAngle;            

            var delta = v1 - direction;
            ang1 = Mathf.Atan2(delta.y, delta.x);

            delta = v2 - direction;
            ang2 = Mathf.Atan2(delta.y, delta.x);

            //Debug.Log("ang1 = " + ang1 * Mathf.Rad2Deg + " | ang2 = " + ang2 * Mathf.Rad2Deg);

            if (ang1 < ang2)
            {
                finalAngle =  UnityEngine.Random.Range(ang1, ang2);
            }
            else
            {
                finalAngle =  UnityEngine.Random.Range(ang2, ang1);
            }

            //Debug.Log("finalAngle = " + finalAngle * Mathf.Rad2Deg);

            return finalAngle;
        }

        private Vector3 GetEnemyPosition(Vector2 playerPos2D)
        {                       
            var pvp = GetPlayersViewPortPosition(playerPos2D);
            Vector2 pos2D;
            float angle, radius;

            if (pvp == E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT)
            {                
                angle = CalcAppearMathAngle(E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT, E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT, Vector2.right, playerPos2D);
            }
            else if (pvp == E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT)
            {
                angle = CalcAppearMathAngle(E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT, E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT, Vector2.right, playerPos2D);
            }
            else if (pvp == E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT)
            {                
                angle = CalcAppearMathAngle(E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT, E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT, Vector2.right, playerPos2D);
            }
            else
            {
                angle = CalcAppearMathAngle(E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT, E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT, Vector2.right, playerPos2D);
            }

            var maxRadius = CalcMaxRadius(angle, playerPos2D);
            radius = UnityEngine.Random.Range(m_OffsetRadius, maxRadius);

            //Debug.Log("angle = " + angle * Mathf.Rad2Deg + " | radius = " + radius);

            angle.MathAngleToUnityObjectAngle();

            pos2D = playerPos2D +  new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));

            return new Vector3(pos2D.x, 0, pos2D.y);
        }

        private Vector2? CalcIntersection(Vector2 from1, Vector2 to1, Vector2 from2, Vector2 to2) 
        {
            Vector2 v1 = new Vector2(to1.x - from1.x, to1.y - from1.y);
            Vector2 v2 = new Vector2(to2.x - from2.x, to2.y - from2.y);
            Vector2 v3 = new Vector2(from2.x - from1.x, from2.y - from1.y);
            Vector2 v4 = new Vector2(to1.x - from2.x, to1.y - from2.y);
            
            float area1 = Cross2D(v2, v3);
            float area2 = Cross2D(v2, v4);
            float total = area1 + area2;

            if (Math.Abs(total) > 0)
            {
                var ratio = area1 / total;

                return new Vector2(from1.x + ratio * v1.x, from1.y + ratio * v1.y);
            }
            else
            {
                return null;
            }
        }

        private float CalcMaxRadius(float angle, Vector2 playerPos2D) 
        {
            Vector2 ray = playerPos2D + new Vector2(RAY_LENGTH * Mathf.Cos(angle), RAY_LENGTH * Mathf.Sin(angle));
            Vector2 intersection = Vector2.zero;
            float dist;

            for(int i = 0; i < m_GameFieldLines.Count - 1; i = i + 2)
            {
                Vector2? tmp = CalcIntersection(playerPos2D, ray, m_GameFieldLines[i], m_GameFieldLines[i + 1]);

                if(tmp != null)
                {
                    Vector2 v = (Vector2)tmp - m_GameFieldLines[i];
                    Vector2 w = m_GameFieldLines[i + 1] - m_GameFieldLines[i];

                    if(Vector2.Dot(v,w) == v.magnitude * w.magnitude && v.magnitude <= w.magnitude)
                    {

                    }
                    else
                    {
                        tmp = null;
                    }
                }

                if(tmp != null)
                {
                    intersection = (Vector2)tmp;
                    break;
                }
            }

            dist = Vector2.Distance(playerPos2D, intersection);

            //Debug.Log("intersection = " + intersection + " | distance = " + dist);

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

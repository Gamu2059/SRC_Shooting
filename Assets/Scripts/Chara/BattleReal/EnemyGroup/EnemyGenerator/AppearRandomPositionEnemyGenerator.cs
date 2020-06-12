#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using Rewired;


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

        private Vector2 m_GameFieldMax;
        private Vector2 m_GameFieldMin;
        private Dictionary<E_PLAYERS_VIEWPORT_POSITION, Vector2> m_GameFieldVertices;
        private Dictionary<E_PLAYERS_VIEWPORT_POSITION, Vector2> m_ViewPortVertices;
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
                { E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT, new Vector2(m_GameFieldMin.x, m_GameFieldMin.y)},
                { E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT, new Vector2(m_GameFieldMin.x, m_GameFieldMax.y)},
                { E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT, new Vector2(m_GameFieldMax.x, m_GameFieldMax.y)},
                { E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT, new Vector2(m_GameFieldMax.x, m_GameFieldMin.y)},
            };
            m_ViewPortVertices = new Dictionary<E_PLAYERS_VIEWPORT_POSITION, Vector2>
            {
                { E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT, new Vector2(-m_GameFieldMin.x, -m_GameFieldMin.y)},
                { E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT, new Vector2(-m_GameFieldMin.x, -m_GameFieldMax.y)},
                { E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT, new Vector2(-m_GameFieldMax.x, -m_GameFieldMax.y)},
                { E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT, new Vector2(-m_GameFieldMax.x, -m_GameFieldMin.y)},
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
            Debug.Log(GetPlayersViewPortPosition(playerPos2D));
            enemyT.localPosition = GetEnemyPosition(playerPos2D);
            enemyT.LookAt(playerPos);

            m_GeneratedEnemies.Add(enemy);
        }

        private Vector3 GetEnemyPosition(Vector2 playerPos2D)
        {
            //Debug.Log("Enter GetEnemyPosition");

            //Vector2 min = BattleRealStageManager.Instance.MinLocalFieldPosition;
            //Vector2 max = BattleRealStageManager.Instance.MaxLocalFieldPosition;

            //float radius;
            //float angle;




            //void CalcAngleAndRadius(Vector2 vert1, Vector2 vert2)
            //{

            //    float ang1 = CalcAngle(playerPos2D, vert1);
            //    float ang2 = CalcAngle(playerPos2D, vert2);

            //    Debug.Log("vert1 = " + vert1 + " | vert2 = " + vert2);
            //    Debug.Log("ang1 = " + ang1 + " | ang2 = " + ang2);

            //    angle = ang1;

            //    //float R = CalcMaxRadius(angle, playerPos2D, min, max);

            //    //Debug.Log("R = " + R);

            //    //if(m_OffsetRadius > R)
            //    //{
            //    //    radius = UnityEngine.Random.Range(R, m_OffsetRadius);
            //    //}
            //    //else
            //    //{
            //    //    radius = UnityEngine.Random.Range(m_OffsetRadius, R);
            //    //}


            //    radius = m_OffsetRadius;
            //}

            //var pvp = GetPlayersViewPortPosition(playerPos2D);
            //Vector2 v1, v2;
            //float ang1, ang2;

            //if(pvp == E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT)
            //{
            //    v1 = m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT] - playerPos2D;
            //    ang1 = Vector2.Angle(Vector2.up, v1);
            //    v2 = m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT] - playerPos2D;
            //    ang2 = Vector2.Angle(Vector2.up, v2);
            //}
            //else if(pvp == E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT)
            //{
            //    v1 = m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT] - playerPos2D;
            //    ang1 = Vector2.Angle(Vector2.right, v1);
            //    v2 = m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT] - playerPos2D;
            //    ang2 = Vector2.Angle(Vector2.right, v2);
            //}
            //else if(pvp == E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT)
            //{
            //    v1 = m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_LEFT] - playerPos2D;
            //    ang1 = Vector2.Angle(Vector2.down, v1);
            //    v2 = m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_RIGHT] - playerPos2D;
            //    ang2 = Vector2.Angle(Vector2.down, v2);
            //}
            //else
            //{
            //    v1 = m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.UPPER_RIGHT] - playerPos2D;
            //    ang1 = Vector2.Angle(Vector2.left, v1);
            //    v2 = m_GameFieldVertices[E_PLAYERS_VIEWPORT_POSITION.LOWER_LEFT] - playerPos2D;
            //    ang2 = Vector2.Angle(Vector2.left, v2);
            //}

            //Debug.Log("playerPos2D = " + playerPos2D);

            //Debug.Log("v1 = " + v1 + " | v2 = " + v2);
            //Debug.Log("ang1 = " + ang1 + " | ang2 = " + ang2);

            //ang1 = ang1.MathAngleToUnityObjectAngle();
            //ang2 = ang2.MathAngleToUnityObjectAngle();

            //Debug.Log("ang1 = " + ang1 + " | ang2 = " + ang2);

            //Vector2 pos2DAng1 = new Vector2(playerPos2D.x + m_OffsetRadius * Mathf.Cos(ang1 * Mathf.Deg2Rad), playerPos2D.y + OffsetRadius * Mathf.Sin(ang1 * Mathf.Deg2Rad));
            //Vector2 pos2DAng2 = new Vector2(playerPos2D.x + m_OffsetRadius * Mathf.Cos(ang2 * Mathf.Deg2Rad), playerPos2D.y + OffsetRadius * Mathf.Sin(ang2 * Mathf.Deg2Rad));

            //Debug.Log("pos2DAng1 = " + pos2DAng1 + " | pos2DAng2 = " + pos2DAng2);

            var ePos = m_ViewPortVertices[GetPlayersViewPortPosition(playerPos2D)];

            return new Vector3(ePos.x, 0, ePos.y);


            ////angle = angle.MathAngleToUnityObjectAngle();

            ////Debug.Log("angle = " + angle + " | radius = " + radius);

            //Vector3 retVal = new Vector3(playerPos2D.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad), 0, playerPos2D.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad));

            ////Debug.Log("Position = " + retVal);

            //return retVal;
        }

        //private float CalcAngleBetweenTwoVertices(Vector2 playerPos2D, Vector2 target)
        //{
        //    var diff = target - playerPos2D;
        //    return Vector2.Angle(playerPos2D, diff) * Cross2D(playerPos2D, diff) < 0 ? -1 : 1;
        //}

        private float CalcMaxRadius(float angle, Vector2 playerPos2D, Vector2 min, Vector3 max) 
        {
            //List<Vector2> viewport = new List<Vector2>
            //{
            //    new Vector2(max.x, max.y), new Vector2(min.x, max.y),  // up
            //    new Vector2(max.x, min.y), new Vector2(max.x, max.y),  // right
            //    new Vector2(min.x, min.y), new Vector2(max.x, min.y),  // bottom
            //    new Vector2(min.x, max.y), new Vector2(min.x, min.y),  // left
            //};

            //float length = 1000.0f;
            //Vector2 ray = new Vector2(playerPos2D.x + length * Mathf.Cos(angle * Mathf.Deg2Rad), playerPos2D.y + length * Mathf.Sin(angle * Mathf.Deg2Rad));            

            //Vector2 intercept = new Vector2();

            //Vector2? CalcIntercept(Vector2 from1, Vector2 to1, Vector2 from2, Vector2 to2)
            //{
            //    var v1 = new Vector2(to1.x - from1.x, to1.y - from1.y);
            //    var v2 = new Vector2(to2.x - from2.x, to2.y - from2.y);
            //    var v3 = new Vector2(from2.x - from1.x, from2.y - from1.y);
            //    var v4 = new Vector2(to1.x - from2.x, to1.y - from2.y);
            //    var area1 = Cross2D(v2, v3);
            //    var area2 = Cross2D(v2, v4);
            //    var total = area1 + area2;
            //    if (Math.Abs(total) > 0)
            //    {
            //        var ratio = area1 / total;

            //        return new Vector2(from1.x + ratio * v1.x, from1.y + ratio * v1.y);
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}

            //for(int i = 0; i < viewport.Count - 1; i = i + 2)
            //{
            //    Vector2? tmp = CalcIntercept(playerPos2D, ray, viewport[i], viewport[i + 1]);                                

            //    if(tmp != null)
            //    {
            //        Vector2 v = (Vector2)tmp - viewport[i];
            //        Vector2 w = viewport[i + 1] - viewport[i];

            //        if(Mathf.Pow(Vector2.Dot(v, w), 2) == v.sqrMagnitude * w.sqrMagnitude && v.sqrMagnitude <= w.sqrMagnitude)
            //        {

            //        }
            //        else
            //        {
            //            tmp = null;
            //        }
            //    }

            //    if(tmp != null)
            //    {
            //        Debug.Log("viewport[i] = " + viewport[i] + " | viewport[i+1] = " + viewport[i + 1]);
            //        intercept = (Vector2)tmp;
            //        break;
            //    }
            //}

            //Debug.Log("playerPos = " + playerPos2D + " | intercept = " + intercept);

            //return Vector2.Distance(playerPos2D, intercept);

            return 0;
        }

        private E_PLAYERS_VIEWPORT_POSITION GetPlayersViewPortPosition(Vector2 playerPos2D)
        {           
            float crossH = Cross2D(Vector2.down, playerPos2D);
            float crossV = Cross2D(Vector2.right, playerPos2D);

            Debug.Log("crossH = " + crossH + " | crossV = " + crossV);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBezierMove : AbstractEnemyMove
{

    // ベジェ曲線の点
    protected Vector3[][] m_LocalPositionBezier;


    [SerializeField, Tooltip("それぞれの形態でのベジェ曲線の第1制御点")]
    protected Vector3[] m_LocalPositionBezier0;

    [SerializeField, Tooltip("それぞれの形態でのベジェ曲線の第2制御点")]
    protected Vector3[] m_LocalPositionBezier1;

    [SerializeField, Tooltip("それぞれの形態でのベジェ曲線の最後の点の位置")]
    protected Vector3[] m_LocalPositionBezier2;


    protected override void OnAwake()
    {
        base.OnAwake();

        m_LocalPositionBezier = new Vector3[m_LocalPositionBezier0.Length][];

        for (int i = 0; i < m_LocalPositionBezier.Length; i++)
        {
            m_LocalPositionBezier[i] = new Vector3[3];

            m_LocalPositionBezier[i][0] = m_LocalPositionBezier0[i];
            m_LocalPositionBezier[i][1] = m_LocalPositionBezier1[i];
            m_LocalPositionBezier[i][2] = m_LocalPositionBezier2[i];
        }
    }


    // 
    protected override void BezierPositionMoving()
    {
        // 時刻の進行度(0～1の値)
        float normalizedTime = m_Time / m_PhaseTime[m_NowPhase];

        if (m_NowPhase == 0)
        {
            transform.localPosition = m_InitialPosition + BezierMoving(Vector3.zero, m_LocalPositionBezier[0][0], m_LocalPositionBezier[0][1], m_LocalPositionBezier[0][2], normalizedTime);
        }
        else
        {
            transform.localPosition = m_InitialPosition + BezierMoving(m_LocalPositionBezier[m_NowPhase - 1][2], m_LocalPositionBezier[m_NowPhase][0], m_LocalPositionBezier[m_NowPhase][1], m_LocalPositionBezier[m_NowPhase][2], normalizedTime);
        }
    }


    // ベジェ曲線を補間する(tは0～1の値)
    private Vector3 BezierMoving(Vector3 vec0, Vector3 vec1, Vector3 vec2, Vector3 vec3, float t)
    {
        return (1 - t) * (1 - t) * (1 - t) * vec0 + 3 * (1 - t) * (1 - t) * t * vec1 + 3 * (1 - t) * t * t * vec2 + t * t * t * vec3;
    }




    ///// <summary>
    ///// 動き方
    ///// </summary>
    //protected enum MOVE_TYPE
    //{
    //    /// <summary>
    //    /// 等速度
    //    /// </summary>
    //    LINEAR,

    //    /// <summary>
    //    /// 等加速度で加速
    //    /// </summary>
    //    ACCELE,

    //    /// <summary>
    //    /// 等加速度で減速
    //    /// </summary>
    //    BRAKE,

    //    /// <summary>
    //    /// 加速度の絶対値が等しく加減速
    //    /// </summary>
    //    ACCELE_AND_BRAKE,
    //}


    //// 動き方
    //[SerializeField]
    //protected MOVE_TYPE[] m_MoveType = new MOVE_TYPE[3];


    //// 弾幕を出し始める位置
    //[SerializeField]
    //protected Vector3 m_RelativeDanmakuStartPosition;

    //// 弾幕を出し終わる位置
    //[SerializeField]
    //protected Vector3 m_RelativeDanmakuFinishPosition;

    //// 敵本体が削除される位置
    //[SerializeField]
    //protected Vector3 m_RelativeDeletePosition;


    //// 始点と終点から補間する(tは0～1の値)
    //private Vector3 Moving(Vector3 startVector,Vector3 finishVector,float t,int index)
    //{

    //    // 始点と終点の相対ベクトル
    //    Vector3 deltaVector = finishVector - startVector;

    //    switch (m_MoveType[index])
    //    {
    //        case MOVE_TYPE.LINEAR:
    //            return startVector + deltaVector * t;

    //        case MOVE_TYPE.ACCELE:
    //            return startVector + deltaVector * t * t;

    //        case MOVE_TYPE.BRAKE:
    //            return startVector + deltaVector * (- t * t + 2 * t);

    //        case MOVE_TYPE.ACCELE_AND_BRAKE:
    //            if (t < 0.5f)
    //            {
    //                return startVector + deltaVector * 2 * t * t;
    //            }
    //            return startVector + deltaVector * (- 2 * t * t + 4 * t - 1);

    //        default:
    //            return startVector * (1 - t) + finishVector * t;
    //    }
    //}


    //private void Ft0()
    //{
    //    // 時刻の進行度(0～1の値)
    //    float normalizedTime = m_Time / m_PhaseTime[0];

    //    transform.localPosition = m_InitialPosition + Moving(Vector3.zero, m_RelativeDanmakuStartPosition, normalizedTime,0);
    //}


    //private void Ft1()
    //{
    //    // 時刻の進行度(0～1の値)
    //    float normalizedTime = m_Time / m_PhaseTime[1];

    //    transform.localPosition = m_InitialPosition + Moving(m_RelativeDanmakuStartPosition, m_RelativeDanmakuFinishPosition, normalizedTime, 1);
    //}


    //private void Ft2()
    //{
    //    // 時刻の進行度(0～1の値)
    //    float normalizedTime = m_Time / m_PhaseTime[2];

    //    transform.localPosition = m_InitialPosition + Moving(m_RelativeDanmakuFinishPosition, m_RelativeDeletePosition, normalizedTime, 2);
    //}

    //// 敵の状態を決めるデリゲートを返す関数
    //protected override MoveEnemyDelegate[] GetMoveEnemyDelegate()
    //{
    //    MoveEnemyDelegate[] moveEnemies = { Ft0, Ft1, Ft2 };

    //    return moveEnemies;
    //}

    //// それぞれの形態の時間の長さを返す関数
    //protected override float[] GetPhaseTime()
    //{
    //    float[] phaseTimes = { 2, 2, 2 };

    //    return phaseTimes;
    //}
}

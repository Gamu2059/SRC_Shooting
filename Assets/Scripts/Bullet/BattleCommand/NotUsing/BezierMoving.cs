//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 敵本体の動きを表すクラス。ベジェ曲線状に動く。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/MovingPhaseBase/BezierMoving", fileName = "BezierMoving", order = 0)]
//[System.Serializable]
//public class BezierMoving : MovingPhaseBase
//{

//    //[SerializeField, Tooltip("始点（自動的に決まる）")]
//    private Vector2 m_BeginPoint;

//    [SerializeField, Tooltip("第1制御点")]
//    private Vector2 m_ControlPoint1;

//    [SerializeField, Tooltip("第2制御点")]
//    private Vector2 m_ControlPoint2;

//    [SerializeField, Tooltip("終点")]
//    private Vector2 m_EndPoint;

//    //[SerializeField, Tooltip("角度（常に一定）")]
//    private float m_Angle;

//    //[SerializeField, Tooltip("大きさ（常に一定）")]
//    private float m_Scale;

//    [SerializeField, Tooltip("所要時間")]
//    private float m_Duration;


//    /// <summary>
//    /// 初期状態を代入する。
//    /// </summary>
//    public override void Init(TransformSimple transform)
//    {
//        m_BeginPoint = transform.m_Position;
//        m_Angle = transform.m_Angle;
//        m_Scale = transform.m_Scale;
//    }


//    /// <summary>
//    /// 与えられた時刻での状態を取得する。
//    /// </summary>
//    public override TransformSimple GetTransform(float time)
//    {
//        return new TransformSimple(
//            GetPosition(time),
//            m_Angle,
//            m_Scale
//            );
//    }


//    /// <summary>
//    /// 与えられた時刻での状態の時間微分を取得する。
//    /// </summary>
//    public override TransformSimple GetDdtTransform(float time)
//    {
//        return new TransformSimple(
//            GetVelocity(time),
//            0,
//            0
//            );
//    }


//    /// <summary>
//    /// 所要時間を所得する。
//    /// </summary>
//    public override float GetDuration()
//    {
//        return m_Duration;
//    }


//    /// <summary>
//    /// この形態の終了時の状態を取得する。
//    /// </summary>
//    public override TransformSimple GetEndTransform()
//    {
//        return new TransformSimple(
//            m_EndPoint,
//            m_Angle,
//            m_Scale
//            );
//    }


//    /// <summary>
//    /// 与えられた時刻での位置を取得する。
//    /// </summary>
//    private Vector2 GetPosition(float time)
//    {
//        // 時刻の進行度(0～1の値)
//        float normalizedTime = time / m_Duration;

//        return GetBezier(m_BeginPoint, m_ControlPoint1, m_ControlPoint2, m_EndPoint, normalizedTime);
//    }



//    /// <summary>
//    /// 4点と媒介変数から、ベジェ曲線上の1点を返す(tは0～1の値)
//    /// </summary>
//    private Vector2 GetBezier(Vector2 vec0, Vector2 vec1, Vector2 vec2, Vector2 vec3, float t)
//    {
//        return (1 - t) * (1 - t) * (1 - t) * vec0 + 3 * (1 - t) * (1 - t) * t * vec1 + 3 * (1 - t) * t * t * vec2 + t * t * t * vec3;
//    }


//    /// <summary>
//    /// 与えられた時刻での位置の時間微分（つまり速度）を取得する。
//    /// </summary>
//    private Vector2 GetVelocity(float time)
//    {
//        // 時刻の進行度(0～1の値)
//        float normalizedTime = time / m_Duration;

//        return GetDdtBezier(m_BeginPoint, m_ControlPoint1, m_ControlPoint2, m_EndPoint, normalizedTime);
//    }


//    /// <summary>
//    /// 4点と媒介変数から、ベジェ曲線上の1点を返す（時間微分）(tは0～1の値)
//    /// </summary>
//    private Vector2 GetDdtBezier(Vector2 vec0, Vector2 vec1, Vector2 vec2, Vector2 vec3, float t)
//    {
//        return vec0 * (-3 * t * t + 6 * t - 3) + vec1 * (3 * t * t - 4 * t + 1) + vec2 * (-3 * t * t + 2 * t) + vec3 * (3 * t * t);
//    }
//}

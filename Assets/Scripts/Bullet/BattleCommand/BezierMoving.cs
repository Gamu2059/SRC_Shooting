using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵本体の動きを表すクラス。ベジェ曲線状に動く。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/BezierMoving", fileName = "BezierMoving", order = 0)]
[System.Serializable]
public class BezierMoving : ScriptableObject
{

    //[SerializeField, Tooltip("始点（自動的に決まる）")]
    private Vector2 m_BeginPoint;

    [SerializeField, Tooltip("第1制御点")]
    private Vector2 m_ControlPoint1;

    [SerializeField, Tooltip("第2制御点")]
    private Vector2 m_ControlPoint2;

    [SerializeField, Tooltip("終点")]
    private Vector2 m_EndPoint;

    //[SerializeField, Tooltip("角度（常に一定）")]
    private float m_Angle;

    //[SerializeField, Tooltip("大きさ（常に一定）")]
    private float m_Scale;

    [SerializeField, Tooltip("所要時間")]
    private float m_Duration;


    /// <summary>
    /// 初期状態を代入する。
    /// </summary>
    /// <param name="transform">最初の状態</param>
    public void Init(TransformSimple transform)
    {
        m_BeginPoint = transform.m_Position;
        m_Angle = transform.m_Angle;
        m_Scale = transform.m_Scale;
    }


    /// <summary>
    /// 与えられた時刻での状態を取得する。
    /// </summary>
    /// <param name="time">時刻</param>
    /// <returns></returns>
    public TransformSimple GetTransform(float time)
    {
        return new TransformSimple(
            GetPosition(time),
            m_Angle,
            m_Scale
            );
    }


    /// <summary>
    /// 所要時間を所得する。
    /// </summary>
    /// <returns></returns>
    public float GetDuration()
    {
        return m_Duration;
    }


    /// <summary>
    /// この形態の終了時の状態を取得する。
    /// </summary>
    /// <returns></returns>
    public TransformSimple GetEndTransform()
    {
        return new TransformSimple(
            m_EndPoint,
            m_Angle,
            m_Scale
            );
    }


    /// <summary>
    /// 与えられた時刻での位置を取得する。
    /// </summary>
    /// <param name="time">時刻</param>
    /// <returns></returns>
    private Vector2 GetPosition(float time)
    {
        // 時刻の進行度(0～1の値)
        float normalizedTime = time / m_Duration;

        return GetBezier(m_BeginPoint, m_ControlPoint1, m_ControlPoint2, m_EndPoint, normalizedTime);
    }



    // 4点と媒介変数から、ベジェ曲線上の1点を返す(tは0～1の値)
    private Vector2 GetBezier(Vector2 vec0, Vector2 vec1, Vector2 vec2, Vector2 vec3, float t)
    {
        return (1 - t) * (1 - t) * (1 - t) * vec0 + 3 * (1 - t) * (1 - t) * t * vec1 + 3 * (1 - t) * t * t * vec2 + t * t * t * vec3;
    }
}

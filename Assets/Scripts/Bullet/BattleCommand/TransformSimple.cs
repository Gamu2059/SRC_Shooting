using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformSimple : object
{
    /// <summary>
    /// 位置
    /// </summary>
    public Vector2 m_Position;

    /// <summary>
    /// 回転角度
    /// </summary>
    public float m_Angle;

    /// <summary>
    /// 大きさ
    /// </summary>
    public float m_Scale;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public TransformSimple(Vector2 position, float angle, float scale)
    {
        m_Position = position;
        m_Angle = angle;
        m_Scale = scale;
    }
}

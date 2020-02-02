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


    /// <summary>
    /// コンストラクタ（クローン用）
    /// </summary>
    public TransformSimple(TransformSimple transform) : this(transform.m_Position, transform.m_Angle, transform.m_Scale)
    {

    }


    public override string ToString()
    {
        return m_Position.ToString() + ", " + m_Angle.ToString() + ", " + m_Scale.ToString();
    }
}

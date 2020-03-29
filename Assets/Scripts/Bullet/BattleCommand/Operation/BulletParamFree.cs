using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾が持つ、bool,int,float,Vector2型のパラメータを表すクラス。
/// </summary>
public class BulletParamFree : object
{
    /// <summary>
    /// bool型の配列
    /// </summary>
    public bool[] m_Bool { get; private set; }

    /// <summary>
    /// int型の配列
    /// </summary>
    public int[] m_Int { get; private set; }

    /// <summary>
    /// float型の配列
    /// </summary>
    public float[] m_Float { get; private set; }

    /// <summary>
    /// Vector2型の配列
    /// </summary>
    public Vector2[] m_Vector2 { get; private set; }


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public BulletParamFree(bool[] aBool, int[] aInt, float[] aFloat, Vector2[] aVector2)
    {
        m_Bool = aBool;
        m_Int = aInt;
        m_Float = aFloat;
        m_Vector2 = aVector2;
    }
}

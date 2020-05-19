using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 位置と速度で指定するベジェ曲線の一点を表すクラス。
/// </summary>
[System.Serializable]
public class BezierPointPV : object
{

    /// <summary>
    /// 位置ベクトル
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Position;
    public OperationVector2Base Position
    {
        set { m_Position = value; }
        get { return m_Position; }
    }

    /// <summary>
    /// 速度ベクトル
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Velocity;
    public OperationVector2Base Velocity
    {
        set { m_Velocity = value; }
        get { return m_Velocity; }
    }

    /// <summary>
    /// 所要時間
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_T;
    public OperationFloatBase T
    {
        set { m_T = value; }
        get { return m_T; }
    }
}

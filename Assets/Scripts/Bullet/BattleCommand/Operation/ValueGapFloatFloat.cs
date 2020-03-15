using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の値とfloat型の隙間を表すクラス。
/// </summary>
[System.Serializable]
public class ValueGapFloatFloat : object
{

    /// <summary>
    /// float型の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Value;
    public OperationFloatBase Value
    {
        set { m_Value = value; }
        get { return m_Value; }
    }

    /// <summary>
    /// float型の隙間
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Gap;
    public OperationFloatBase Gap
    {
        set { m_Gap = value; }
        get { return m_Gap; }
    }
}

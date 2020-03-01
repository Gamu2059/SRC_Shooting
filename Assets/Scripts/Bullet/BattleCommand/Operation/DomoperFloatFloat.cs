using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の領域とfloat型の演算をまとめたクラス。
/// </summary>
[System.Serializable]
public class DomoperFloatFloat : object
{

    /// <summary>
    /// float型の領域
    /// </summary>
    [SerializeField]
    private DomainFloatBase m_DomainFloat;
    public DomainFloatBase DomainFloat
    {
        set { m_DomainFloat = value; }
        get { return m_DomainFloat; }
    }

    /// <summary>
    /// float型の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Float;
    public OperationFloatBase Float
    {
        set { m_Float = value; }
        get { return m_Float; }
    }
}

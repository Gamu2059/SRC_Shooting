using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の領域とVector2型の演算をまとめたクラス。
/// </summary>
[System.Serializable]
public class DomoperFloatVector2 : object
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
    /// Vector2型の値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Vector2;
    public OperationVector2Base Vec2
    {
        set { m_Vector2 = value; }
        get { return m_Vector2; }
    }
}

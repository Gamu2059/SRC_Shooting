using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2型の演算の定数を表すクラス。
/// </summary>
public class OperationVector2Init : OperationVector2Base
{

    /// <summary>
    /// 値
    /// </summary>
    private Vector2 m_Value;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public OperationVector2Init(Vector2 value)
    {
        m_Value = value;
    }


    public override Vector2 GetResult()
    {
        return m_Value;
    }
}

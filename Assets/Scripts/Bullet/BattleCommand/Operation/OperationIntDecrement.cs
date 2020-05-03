#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int値のデクリメント演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/decrement", fileName = "OperationIntDecrement", order = 0)]
[System.Serializable]
public class OperationIntDecrement : OperationIntBase
{

    /// <summary>
    /// int値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Int;


    public override int GetResultInt()
    {
        return m_Int.GetResultInt() - 1;
    }
}
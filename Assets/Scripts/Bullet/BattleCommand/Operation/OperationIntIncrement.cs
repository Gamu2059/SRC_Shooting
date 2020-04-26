#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int値のインクリメント演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/increment", fileName = "OperationIntIncrement", order = 0)]
[System.Serializable]
public class OperationIntIncrement : OperationIntBase
{

    /// <summary>
    /// int値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Int;


    public override int GetResultInt()
    {
        return m_Int.GetResultInt() + 1;
    }
}
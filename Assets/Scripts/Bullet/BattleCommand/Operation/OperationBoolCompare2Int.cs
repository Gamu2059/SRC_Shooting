#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2つのint値を比較する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/bool/compare2Int", fileName = "OperationBoolCompare2Int", order = 0)]
[System.Serializable]
public class OperationBoolCompare2Int : OperationBoolBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_IntA;

    /// <summary>
    /// イコールが付くかどうか
    /// </summary>
    [SerializeField]
    private OperationBoolBase m_IsEqual;

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_IntB;


    public override bool GetResultBool()
    {
        if (m_IsEqual.GetResultBool())
        {
            return m_IntA.GetResultInt() <= m_IntB.GetResultInt();
        }
        else
        {
            return m_IntA.GetResultInt() < m_IntB.GetResultInt();
        }
    }
}

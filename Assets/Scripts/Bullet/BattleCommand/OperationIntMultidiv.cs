#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の乗除を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/multidiv", fileName = "OperationIntMultidiv", order = 0)]
[System.Serializable]
public class OperationIntMultidiv : OperationIntBase
{

    /// <summary>
    /// 分子の配列
    /// </summary>
    [SerializeField]
    private OperationIntBase[] m_Multiple;

    /// <summary>
    /// 分母の配列
    /// </summary>
    [SerializeField]
    private OperationIntBase[] m_Divide;


    public override int GetResultInt()
    {
        int numeratorResult = 1;

        if (m_Multiple != null)
        {
            foreach (OperationIntBase numerator in m_Multiple)
            {
                numeratorResult *= numerator.GetResultInt();
            }
        }

        int denominatorResult = 1;

        if (m_Divide != null)
        {
            foreach (OperationIntBase denominator in m_Divide)
            {
                denominatorResult *= denominator.GetResultInt();
            }
        }

        return numeratorResult / denominatorResult;
    }
}
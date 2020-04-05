#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の剰余を求める演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/remainder", fileName = "OperationIntRemainder", order = 0)]
[System.Serializable]
public class OperationIntRemainder : OperationIntBase
{

    /// <summary>
    /// 割られる数
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Dividend;

    /// <summary>
    /// 割る数
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Divisor;


    public override float GetResultFloat()
    {
        return m_Dividend.GetResultFloat() % m_Divisor.GetResultFloat();
    }


    public override int GetResultInt()
    {
        return m_Dividend.GetResultInt() % m_Divisor.GetResultInt();
    }
}
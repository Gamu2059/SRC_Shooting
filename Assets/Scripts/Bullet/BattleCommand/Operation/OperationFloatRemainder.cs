using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の剰余を求める演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/remainder", fileName = "OperationFloatRemainder", order = 0)]
[System.Serializable]
public class OperationFloatRemainder : OperationFloatBase
{

    /// <summary>
    /// 割られる数
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Dividend;

    /// <summary>
    /// 割る数
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Divisor;


    public override float GetResultFloat()
    {
        return m_Dividend.GetResultFloat() % m_Divisor.GetResultFloat();
    }
}
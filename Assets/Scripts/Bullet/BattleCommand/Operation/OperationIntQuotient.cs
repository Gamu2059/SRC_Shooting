using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 商を求める演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/quotient", fileName = "OperationIntQuotient", order = 0)]
[System.Serializable]
public class OperationIntQuotient : OperationIntBase
{

    /// <summary>
    /// 割られる数
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_DividendOfQuotient;

    /// <summary>
    /// 割る数
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_DivisorOfQuotient;


    public override float GetResultFloat()
    {
        return Mathf.Floor(m_DividendOfQuotient.GetResultFloat() / m_DivisorOfQuotient.GetResultFloat());
    }


    public override int GetResultInt()
    {
        return Mathf.FloorToInt(m_DividendOfQuotient.GetResultFloat() / m_DivisorOfQuotient.GetResultFloat());
    }
}
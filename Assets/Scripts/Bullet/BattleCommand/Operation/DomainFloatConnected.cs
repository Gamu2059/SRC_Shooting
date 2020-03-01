using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 連結したfloat型の領域を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/domain/float/connected", fileName = "DomainFloatConnected", order = 0)]
[System.Serializable]
public class DomainFloatConnected : DomainFloatBase
{

    /// <summary>
    /// 最小値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Minimum;

    /// <summary>
    /// 最小値を含むかどうか
    /// </summary>
    [SerializeField]
    private OperationBoolBase m_IsContainingMinimum;

    /// <summary>
    /// 最大値を含むかどうか
    /// </summary>
    [SerializeField]
    private OperationBoolBase m_IsContainingMaximum;

    /// <summary>
    /// 最大値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Maximum;


    public override bool IsInsideFloat(float value)
    {
        bool b1;

        if (m_IsContainingMinimum.GetResultBool())
        {
            b1 = m_Minimum.GetResultFloat() <= value;
        }
        else
        {
            b1 = m_Minimum.GetResultFloat() < value;
        }

        bool b2;

        if (m_IsContainingMaximum.GetResultBool())
        {
            b2 = value <= m_Maximum.GetResultFloat();
        }
        else
        {
            b2 = value < m_Maximum.GetResultFloat();
        }

        return b1 && b2;
    }
}




///// <summary>
///// 値
///// </summary>
//[SerializeField]
//private OperationFloatBase m_Value;

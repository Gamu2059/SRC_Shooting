using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の割り算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/divide", fileName = "OperationFloatDivide", order = 0)]
[System.Serializable]
public class OperationFloatDivide : OperationFloatBase
{

    /// <summary>
    /// 分子
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Numerator;

    /// <summary>
    /// 分母
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Denominator;


    public override float GetResultFloat()
    {
        return m_Numerator.GetResultFloat() / m_Denominator.GetResultFloat();
    }
}
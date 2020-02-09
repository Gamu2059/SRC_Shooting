using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の乗除を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/multidiv", fileName = "OperationFloatMultidiv", order = 0)]
[System.Serializable]
public class OperationFloatMultidiv : OperationFloatBase
{

    /// <summary>
    /// 分子の配列
    /// </summary>
    [SerializeField]
    private OperationFloatBase[] m_NumeratorArray;

    /// <summary>
    /// 分母の配列
    /// </summary>
    [SerializeField]
    private OperationFloatBase[] m_DenominatorArray;


    public override float GetResultFloat()
    {
        float numeratorResult = 1;

        foreach (OperationFloatBase numerator in m_NumeratorArray)
        {
            numeratorResult *= numerator.GetResultFloat();
        }

        float denominatorResult = 1;

        foreach (OperationFloatBase denominator in m_DenominatorArray)
        {
            denominatorResult *= denominator.GetResultFloat();
        }

        return numeratorResult / denominatorResult;
    }
}
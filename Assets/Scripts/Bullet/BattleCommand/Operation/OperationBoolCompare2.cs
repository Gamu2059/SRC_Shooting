#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2つのfloat値を比較する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/bool/compare2", fileName = "OperationBoolCompare2", order = 0)]
[System.Serializable]
public class OperationBoolCompare2 : OperationBoolBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_FloatA;

    /// <summary>
    /// 等号が入るかどうか
    /// </summary>
    [SerializeField]
    private bool m_IsEqual;

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_FloatB;


    public override bool GetResultBool()
    {
        if (m_IsEqual)
        {
            return m_FloatA.GetResultFloat() <= m_FloatB.GetResultFloat();
        }
        else
        {
            return m_FloatA.GetResultFloat() < m_FloatB.GetResultFloat();
        }
    }
}

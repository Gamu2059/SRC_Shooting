#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// way弾を発射するときなどのための演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/way", fileName = "OperationFloatWay", order = 0)]
[System.Serializable]
public class OperationFloatWay : OperationFloatBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Index;

    /// <summary>
    /// 中心の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Center;

    /// <summary>
    /// 全体の幅
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Range;

    /// <summary>
    /// 回数
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Num;


    public override float GetResultFloat()
    {
        float range = m_Range.GetResultFloat();
        int num = m_Num.GetResultInt();

        if (num <= 1)
        {
            return m_Center.GetResultFloat();
        }
        else
        {
            return m_Center.GetResultFloat() - range / 2 + m_Index.GetResultInt() * range / (num - 1);
        }
    }
}
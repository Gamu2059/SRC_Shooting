#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 最小値と最大値で指定してway弾を発射するときなどのための演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/wayminmax", fileName = "OperationFloatWayminmax", order = 0)]
[System.Serializable]
public class OperationFloatWayminmax : OperationFloatBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Index;

    /// <summary>
    /// 最小値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Min;

    /// <summary>
    /// 最大値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Max;

    /// <summary>
    /// 回数（個数）
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Num;


    public override float GetResultFloat()
    {
        float min = m_Min.GetResultFloat();
        int num = m_Num.GetResultInt();

        if (num <= 1)
        {
            return (min + m_Max.GetResultFloat()) / 2;
        }
        else
        {
            return min + m_Index.GetResultFloat() * (m_Max.GetResultFloat() - min) / (num - 1);
        }
    }
}
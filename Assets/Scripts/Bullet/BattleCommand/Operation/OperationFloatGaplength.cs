#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等間隔な隙間の長さの総和を求める演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/gaplength", fileName = "OperationFloatGaplength", order = 0)]
[System.Serializable]
public class OperationFloatGaplength : OperationFloatBase
{

    /// <summary>
    /// 1つ分の間隔の長さ
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Interval;

    /// <summary>
    /// 発射などのものの個数
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Num;


    public override float GetResultFloat()
    {
        return m_Interval.GetResultFloat() * (m_Num.GetResultInt() - 1);
    }
}
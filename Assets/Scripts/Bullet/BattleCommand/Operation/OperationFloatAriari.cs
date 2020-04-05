#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等差数列の等差数列の項の値を求める演算を表すクラス。ただし初項は第0項とする。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/ariari", fileName = "OperationFloatAriari", order = 0)]
[System.Serializable]
public class OperationFloatAriari : OperationFloatBase
{

    /// <summary>
    /// 初項
    /// </summary>
    [Header("初項")]
    [Header("")]
    [Header("等差数列の等差数列の項の値を求める演算")]
    [SerializeField]
    private OperationFloatBase m_A0;

    /// <summary>
    /// 小数列ごとの公差
    /// </summary>
    [Header("小数列ごとの公差")]
    [SerializeField]
    private OperationFloatBase m_D1;

    /// <summary>
    /// 小数列内での公差
    /// </summary>
    [Header("小数列内での公差")]
    [SerializeField]
    private OperationFloatBase m_D2;

    /// <summary>
    /// 各小数列の項数
    /// </summary>
    [Header("各小数列の項数")]
    [SerializeField]
    private OperationIntBase m_M;

    /// <summary>
    /// 何項目の値を求めるのか
    /// </summary>
    [Header("何項目の値を求めるのか")]
    [SerializeField]
    private OperationIntBase m_N;


    public override float GetResultFloat()
    {
        float a0 = m_A0.GetResultFloat();
        float d1 = m_D1.GetResultFloat();
        float d2 = m_D2.GetResultFloat();
        int m = m_M.GetResultInt();
        int n = m_N.GetResultInt();

        // 項数の商の部分
        int nQuotient = n / m;

        // 項数の余りの部分
        int nRemainder = n % m;

        // 計算結果の商の部分
        float resultQuotient = a0 + d1 * nQuotient;

        // 計算結果の余りの部分
        float resultRemainder = d2 * nRemainder;

        return resultQuotient + resultRemainder;
    }
}
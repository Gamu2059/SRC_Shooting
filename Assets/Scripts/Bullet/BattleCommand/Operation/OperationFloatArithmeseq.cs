using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等差数列の項の値を求める演算を表すクラス。ただし初項は第0項とする。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/arithmeseq", fileName = "OperationFloatArithmeseq", order = 0)]
[System.Serializable]
public class OperationFloatArithmeseq : OperationFloatBase
{

    /// <summary>
    /// 初項
    /// </summary>
    [Header("初項")]
    [Header("")]
    [Header("等差数列の項の値を求める演算")]
    [SerializeField]
    private OperationFloatBase m_A0;

    /// <summary>
    /// 公差
    /// </summary>
    [Header("公差")]
    [SerializeField]
    private OperationFloatBase m_D;

    /// <summary>
    /// 何項目の値を求めるのか
    /// </summary>
    [SerializeField]
    [Header("何項目の値を求めるのか")]
    private OperationFloatBase m_N;


    public override float GetResultFloat()
    {
        return m_A0.GetResultFloat() + m_N.GetResultFloat() * m_D.GetResultFloat();
    }
}
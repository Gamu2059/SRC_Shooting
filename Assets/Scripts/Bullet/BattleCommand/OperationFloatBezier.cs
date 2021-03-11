#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ベジェ曲線状に変化するfloat値を生成する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/bezier", fileName = "OperationFloatBezier", order = 0)]
[System.Serializable]
public class OperationFloatBezier : OperationFloatBase
{

    /// <summary>
    /// 始点
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_BeginPoint;

    /// <summary>
    /// 第一制御点
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_ControllPoint1;

    /// <summary>
    /// 第二制御点
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_ControllPoint2;

    /// <summary>
    /// 終点
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_EndPoint;

    /// <summary>
    /// 進行度（媒介変数）（0～1の値）
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_T;


    public override float GetResultFloat()
    {
        float t = m_T.GetResultFloat();

        return (1 - t) * (1 - t) * (1 - t) * m_BeginPoint.GetResultFloat() +
            3 * (1 - t) * (1 - t) * t * m_ControllPoint1.GetResultFloat() +
            3 * (1 - t) * t * t * m_ControllPoint2.GetResultFloat() +
            t * t * t * m_EndPoint.GetResultFloat();
    }
}
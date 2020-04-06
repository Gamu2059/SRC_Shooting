#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ベジェ曲線上の点であるVector2型の値を生成する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/bezier", fileName = "OperationVector2Bezier", order = 0)]
[System.Serializable]
public class OperationVector2Bezier : OperationVector2Base
{

    /// <summary>
    /// 始点
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_BeginPoint;

    /// <summary>
    /// 第一制御点
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_ControllPoint1;

    /// <summary>
    /// 第二制御点
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_ControllPoint2;

    /// <summary>
    /// 終点
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_EndPoint;

    /// <summary>
    /// 進行度（媒介変数）（0～1の値）
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_T;


    public override Vector2 GetResultVector2()
    {
        float t = m_T.GetResultFloat();

        return (1 - t) * (1 - t) * (1 - t) * m_BeginPoint.GetResultVector2() +
            3 * (1 - t) * (1 - t) * t * m_ControllPoint1.GetResultVector2() +
            3 * (1 - t) * t * t * m_ControllPoint2.GetResultVector2() +
            t * t * t * m_EndPoint.GetResultVector2();
    }
}
#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2次元ベクトルの外積を求める演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/cross", fileName = "OperationFloatCross", order = 0)]
[System.Serializable]
public class OperationFloatCross : OperationFloatBase
{

    /// <summary>
    /// 2次元ベクトル
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_VectorA;

    /// <summary>
    /// 2次元ベクトル
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_VectorB;


    public override float GetResultFloat()
    {
        Vector2 vecA = m_VectorA.GetResultVector2();
        Vector2 vecB = m_VectorB.GetResultVector2();

        return vecA.x * vecB.y - vecA.y * vecB.x;
    }
}
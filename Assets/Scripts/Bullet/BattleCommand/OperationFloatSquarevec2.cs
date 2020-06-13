#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2値の絶対値を二乗したfloat値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/squarevec2", fileName = "OperationFloatSquarevec2", order = 0)]
[System.Serializable]
public class OperationFloatSquarevec2 : OperationFloatBase
{

    /// <summary>
    /// 二乗する値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Value;


    public override float GetResultFloat()
    {
        Vector2 value = m_Value.GetResultVector2();

        return value.x * value.x + value.y * value.y;
    }
}
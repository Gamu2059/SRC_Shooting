#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の値を二乗する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/square", fileName = "OperationFloatSquare", order = 0)]
[System.Serializable]
public class OperationFloatSquare : OperationFloatBase
{

    /// <summary>
    /// 二乗する値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Value;


    public override float GetResultFloat()
    {
        float value = m_Value.GetResultFloat();

        return value * value;
    }
}
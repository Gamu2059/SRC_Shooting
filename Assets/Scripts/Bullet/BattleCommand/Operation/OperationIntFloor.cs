#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の値の小数部分を切り捨ててint型にする演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/floor", fileName = "OperationIntFloor", order = 0)]
[System.Serializable]
public class OperationIntFloor : OperationIntBase
{

    /// <summary>
    /// float型の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Float;


    public override float GetResultFloat()
    {
        return Mathf.FloorToInt(m_Float.GetResultFloat());
    }


    public override int GetResultInt()
    {
        return Mathf.FloorToInt(m_Float.GetResultFloat());
    }
}
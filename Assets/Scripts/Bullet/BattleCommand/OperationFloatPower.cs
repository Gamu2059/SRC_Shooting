#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型のべき乗の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/power", fileName = "OperationFloatPower", order = 0)]
[System.Serializable]
public class OperationFloatPower : OperationFloatBase
{

    /// <summary>
    /// 底
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Base;

    /// <summary>
    /// 冪指数
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Exponent;


    public override float GetResultFloat()
    {
        return Mathf.Pow(m_Base.GetResultFloat(), m_Exponent.GetResultFloat());
    }
}
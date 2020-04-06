#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// x座標とy座標からVector2型の値を生成する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/cartesian", fileName = "OperationVector2Cartesian", order = 0)]
[System.Serializable]
public class OperationVector2Cartesian : OperationVector2Base
{

    /// <summary>
    /// x座標
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_X;

    /// <summary>
    /// y座標
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Y;


    public override Vector2 GetResultVector2()
    {
        return new Vector2(m_X.GetResultFloat(), m_Y.GetResultFloat());
    }
}
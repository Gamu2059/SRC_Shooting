using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2次元ベクトルをスカラー倍する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/scalartimes", fileName = "OperationVector2Scalartimes", order = 0)]
[System.Serializable]
public class OperationVector2Scalartimes : OperationVector2Base
{

    /// <summary>
    /// ベクトル
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Vector2;

    /// <summary>
    /// スカラー
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Scalar;


    public override Vector2 GetResultVector2()
    {
        return m_Vector2.GetResultVector2() * m_Scalar.GetResultFloat();
    }
}
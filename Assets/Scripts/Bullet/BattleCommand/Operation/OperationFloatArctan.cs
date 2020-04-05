#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アークタンジェント値を求める演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/arctan", fileName = "OperationFloatArctan", order = 0)]
[System.Serializable]
public class OperationFloatArctan : OperationFloatBase
{

    /// <summary>
    /// x座標の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_X;

    /// <summary>
    /// y座標の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Y;


    public override float GetResultFloat()
    {
        return Mathf.Atan2(m_Y.GetResultFloat(), m_X.GetResultFloat());
    }
}
#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 動径と偏角からVector2型の値を生成する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/polar", fileName = "OperationVector2Polar", order = 0)]
[System.Serializable]
public class OperationVector2Polar : OperationVector2Base
{

    /// <summary>
    /// 動径
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_R;

    /// <summary>
    /// 偏角
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Theta;


    public override Vector2 GetResultVector2()
    {
        return m_R.GetResultFloat() * new Vector2(Mathf.Cos(m_Theta.GetResultFloat()), Mathf.Sin(m_Theta.GetResultFloat()));
    }
}
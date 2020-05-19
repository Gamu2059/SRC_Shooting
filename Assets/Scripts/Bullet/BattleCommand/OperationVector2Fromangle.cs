#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角度からVector2型の単位ベクトルを生成する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/fromangle", fileName = "OperationVector2Fromangle", order = 0)]
[System.Serializable]
public class OperationVector2Fromangle : OperationVector2Base
{
    /// <summary>
    /// 偏角
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Angle;


    public override Vector2 GetResultVector2()
    {
        float angle = m_Angle.GetResultFloat();

        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
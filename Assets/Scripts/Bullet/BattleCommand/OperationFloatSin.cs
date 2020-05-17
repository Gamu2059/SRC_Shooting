#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サイン値を求める演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/sin", fileName = "OperationFloatSin", order = 0)]
[System.Serializable]
public class OperationFloatSin : OperationFloatBase
{

    /// <summary>
    /// 角度の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Angle;


    public override float GetResultFloat()
    {
        return Mathf.Sin(m_Angle.GetResultFloat());
    }
}
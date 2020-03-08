using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コサイン値を求める演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/cos", fileName = "OperationFloatCos", order = 0)]
[System.Serializable]
public class OperationFloatCos : OperationFloatBase
{

    /// <summary>
    /// 角度の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Angle;


    public override float GetResultFloat()
    {
        return Mathf.Cos(m_Angle.GetResultFloat());
    }
}
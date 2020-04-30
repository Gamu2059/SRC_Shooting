#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の値を対称移動させる演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/symmetrical", fileName = "OperationFloatSymmetrical", order = 0)]
[System.Serializable]
public class OperationFloatSymmetrical : OperationFloatBase
{

    /// <summary>
    /// 対称移動させる対象となる値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Value;

    /// <summary>
    /// この値を基準として、対称移動をする
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Basis;


    public override float GetResultFloat()
    {
        return 2 * m_Basis.GetResultFloat() - m_Value.GetResultFloat();
    }
}
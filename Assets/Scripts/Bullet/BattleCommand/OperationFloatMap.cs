#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の値をある範囲から別の範囲へ再マッピングする演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/map", fileName = "OperationFloatMap", order = 0)]
[System.Serializable]
public class OperationFloatMap : OperationFloatBase
{

    /// <summary>
    /// 関数の引数にあたる値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_T;

    /// <summary>
    /// マッピング元の最小値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_AMin;

    /// <summary>
    /// マッピング元の最大値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_AMax;

    /// <summary>
    /// マッピング先の最小値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_BMin;

    /// <summary>
    /// マッピング先の最大値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_BMax;


    public override float GetResultFloat()
    {
        float t = m_T.GetResultFloat();
        float aMin = m_AMin.GetResultFloat();
        float aMax = m_AMax.GetResultFloat();
        float bMin = m_BMin.GetResultFloat();
        float bMax = m_BMax.GetResultFloat();

        float normT = (t - aMin) / (aMax - aMin);

        return (bMax - bMin) * normT + bMin;
    }
}
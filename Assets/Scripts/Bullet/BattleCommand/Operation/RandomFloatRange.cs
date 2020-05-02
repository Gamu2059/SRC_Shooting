#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定した範囲内のランダムなfloat値を取得するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/random/float/range", fileName = "RandomFloatRange", order = 0)]
[System.Serializable]
public class RandomFloatRange : OperationFloatBase
{

    /// <summary>
    /// 最小値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Min;

    /// <summary>
    /// 最大値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Max;


    public override float GetResultFloat()
    {
        return Random.Range(m_Min.GetResultFloat(), m_Max.GetResultFloat());
    }
}
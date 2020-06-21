#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 正規分布に基づくfloat型の乱数を取得するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/random/float/normdistri", fileName = "RandomFloatNormdistri", order = 0)]
[System.Serializable]
public class RandomFloatNormdistri : OperationFloatBase
{

    /// <summary>
    /// 平均
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Average;

    /// <summary>
    /// 標準偏差
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_StandardDeviation;


    public override float GetResultFloat()
    {
        // 一様に分布する乱数
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);

        // ボックス＝ミュラー法を用いる
        return m_StandardDeviation.GetResultFloat() * Mathf.Sqrt(-2 * Mathf.Log(x)) * Mathf.Sin(2 * Mathf.PI * y) + m_Average.GetResultFloat();
    }
}
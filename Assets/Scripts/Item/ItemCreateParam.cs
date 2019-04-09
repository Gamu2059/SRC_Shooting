using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムの生成情報
/// </summary>
public struct ItemCreateParam
{
    /// <summary>
    /// 小さなスコアアイテムの生成個数
    /// </summary>
    public int SmallScoreNum;

    /// <summary>
    /// 大きなスコアアイテムの生成個数
    /// </summary>
    public int BigScoreNum;

    /// <summary>
    /// 小さなスコア得点上昇アイテムの生成個数
    /// </summary>
    public int SmallScoreUpNum;

    /// <summary>
    /// 大きなスコア得点上昇アイテムの生成個数
    /// </summary>
    public int BigScoreUpNum;

    /// <summary>
    /// 小さな経験値アイテムの生成個数
    /// </summary>
    public int SmallExpNum;

    /// <summary>
    /// 大きな経験値アイテムの生成個数
    /// </summary>
    public int BigExpNum;

    /// <summary>
    /// 小さなボムポイントアイテムの生成個数
    /// </summary>
    public int SmallBombNum;

    /// <summary>
    /// 大きなボムポイントアイテムの生成個数
    /// </summary>
    public int BigBombNum;
}

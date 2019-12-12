using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムの種類を定義する列挙値
/// </summary>
public enum E_ITEM_TYPE
{
    /// <summary>
    /// ダミー用
    /// </summary>
    NONE,

    /// <summary>
    /// 残機アップ
    /// </summary>
    LIFE_RECOVERY,

    /// <summary>
    /// 小さなスコアアイテム
    /// </summary>
    SMALL_SCORE,

    /// <summary>
    /// 大きなスコアアイテム
    /// </summary>
    BIG_SCORE,

    /// <summary>
    /// 小さな経験値アイテム
    /// </summary>
    SMALL_EXP,

    /// <summary>
    /// 大きな経験値アイテム
    /// </summary>
    BIG_EXP,

    /// <summary>
    /// 小さなボムポイント増加アイテム
    /// </summary>
    SMALL_ENERGY,

    /// <summary>
    /// 大きなボムポイント増加アイテム
    /// </summary>
    BIG_ENERGY,
}

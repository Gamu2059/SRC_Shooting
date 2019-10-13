using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの経験値やボムチャージ量のパラメータを保持する。
/// </summary>
[System.Serializable]
public struct PlayerState
{
    /// <summary>
    /// 各レベルに必要な経験値量のカーブ
    /// </summary>
    public int[] NextNeedExpParams;

    /// <summary>
    /// ボムが溜まる量
    /// </summary>
    public float BombCharge;
}

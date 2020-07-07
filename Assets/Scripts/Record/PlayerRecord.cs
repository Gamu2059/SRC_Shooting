using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイデータの記録に必要な情報
/// </summary>
[Serializable]
public class PlayerRecord
{
    /// <summary>
    /// ゲーム終了時の最終スコア
    /// </summary>
    public ulong FinalScore;

    /// <summary>
    /// ゲーム終了時の到達ステージ
    /// (後でenumに変更予定)
    /// </summary>
    public E_CHAPTER FinalReachedStage;

    /// <summary>
    /// プレイした日付
    /// </summary>
    /// <value></value>
    public string PlayedDate;

    /// <summary>
    /// プレイヤーネーム
    /// </summary>
    public string PlayerName;

    public PlayerRecord(string name, ulong finalScore, E_CHAPTER reachedStage, DateTime date)
    {
        PlayerName = name;
        FinalScore = finalScore;
        FinalReachedStage = reachedStage;
        PlayedDate = string.Format("{0:yyyy/MM/dd}", date);
    }

    public string FinalScoreToString()
    {
        return string.Format("{0:000000000}", FinalScore);
    }
}

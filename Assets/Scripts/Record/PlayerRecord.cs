using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイデータの記録に必要な情報
/// </summary>
public class PlayerRecord
{
    /// <summary>
    /// ゲーム終了時の最終スコア
    /// </summary>
    public double m_FinalScore {get; private set;}

    /// <summary>
    /// ゲーム終了時の到達ステージ
    /// (後でenumに変更予定)
    /// </summary>
    public int m_FinalReachedStage {get; private set;}

    /// <summary>
    /// プレイした日付
    /// </summary>
    /// <value></value>
    public System.DateTime m_PlayedDate {get; private set;}

    /// <summary>
    /// プレイヤーネーム
    /// </summary>
    public string m_PlayerName { get; private set; }

    public PlayerRecord(){

    }

    public PlayerRecord(string name, double finalScore, int reachedStage, System.DateTime date){
        m_PlayerName = name;
        m_FinalScore = finalScore;
        m_FinalReachedStage = reachedStage;
        m_PlayedDate = date;
    }
}

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
    public float m_FinalScore {get; private set;}

    /// <summary>
    /// ゲーム終了時の到達ステージ
    /// (後でenumに変更予定)
    /// </summary>
    public int m_FinalReachedStage {get; private set;}

    public System.DateTime m_PlayedDate;

    // プレイ日時

    // プレイヤーネーム

    public PlayerRecord(){

    }

    public PlayerRecord(float finalScore, int reachedStage, System.DateTime date){
        m_FinalScore = finalScore;
        m_FinalReachedStage = reachedStage;
        m_PlayedDate = date;
    }
}

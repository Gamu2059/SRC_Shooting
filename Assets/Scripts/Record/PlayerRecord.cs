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
    public E_STAGE m_FinalReachedStage {get; private set;}

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

    public PlayerRecord(string name, double finalScore, E_STAGE reachedStage, System.DateTime date){
        m_PlayerName = name;
        m_FinalScore = finalScore;
        m_FinalReachedStage = reachedStage;
        m_PlayedDate = date;
    }

    public string FinalScoreToString()
    {
        return string.Format("{0:000000000}", m_FinalScore);
    }

    public E_DIFFICULTY StageDifficulty()
    {
        if(m_FinalReachedStage == E_STAGE.EASY_0 || m_FinalReachedStage == E_STAGE.EASY_1 || m_FinalReachedStage == E_STAGE.EASY_2 || m_FinalReachedStage == E_STAGE.EASY_3 || m_FinalReachedStage == E_STAGE.EASY_4
            || m_FinalReachedStage == E_STAGE.EASY_5 || m_FinalReachedStage == E_STAGE.EASY_6)
        {
            return E_DIFFICULTY.EASY;
        }
        else if(m_FinalReachedStage == E_STAGE.NORMAL_0 || m_FinalReachedStage == E_STAGE.NORMAL_1 || m_FinalReachedStage == E_STAGE.NORMAL_2 || m_FinalReachedStage == E_STAGE.NORMAL_3 || m_FinalReachedStage == E_STAGE.NORMAL_4
            || m_FinalReachedStage == E_STAGE.NORMAL_5 || m_FinalReachedStage == E_STAGE.NORMAL_6)
        {
            return E_DIFFICULTY.NORMAL;
        }
        else if(m_FinalReachedStage == E_STAGE.HARD_0 || m_FinalReachedStage == E_STAGE.HARD_1 || m_FinalReachedStage == E_STAGE.HARD_2 || m_FinalReachedStage == E_STAGE.HARD_3 || m_FinalReachedStage == E_STAGE.HARD_4
            || m_FinalReachedStage == E_STAGE.HARD_5 || m_FinalReachedStage == E_STAGE.HARD_6)
        {
            return E_DIFFICULTY.HARD;
        }
        else
        {
            return E_DIFFICULTY.HADES;
        }
    }

    public string FinalReachedStageToString()
    {
        if(m_FinalReachedStage == E_STAGE.EASY_0 || m_FinalReachedStage == E_STAGE.NORMAL_0 || m_FinalReachedStage == E_STAGE.HARD_0 || m_FinalReachedStage == E_STAGE.HADES_0)
        {
            return "0";
        }
        else if(m_FinalReachedStage == E_STAGE.EASY_1 || m_FinalReachedStage == E_STAGE.NORMAL_1 || m_FinalReachedStage == E_STAGE.HARD_1 || m_FinalReachedStage == E_STAGE.HADES_1)
        {
            return "1";
        }
        else if(m_FinalReachedStage == E_STAGE.EASY_2 || m_FinalReachedStage == E_STAGE.NORMAL_2 || m_FinalReachedStage == E_STAGE.HARD_2 || m_FinalReachedStage == E_STAGE.HADES_2)
        {
            return "2";
        }
        else if(m_FinalReachedStage == E_STAGE.EASY_3 || m_FinalReachedStage == E_STAGE.NORMAL_3 || m_FinalReachedStage == E_STAGE.HARD_3 || m_FinalReachedStage == E_STAGE.HADES_3)
        {
            return "3";
        }
        else if (m_FinalReachedStage == E_STAGE.EASY_4 || m_FinalReachedStage == E_STAGE.NORMAL_4 || m_FinalReachedStage == E_STAGE.HARD_4 || m_FinalReachedStage == E_STAGE.HADES_4)
        {
            return "4";
        }
        else if(m_FinalReachedStage == E_STAGE.EASY_5 || m_FinalReachedStage == E_STAGE.NORMAL_5 || m_FinalReachedStage == E_STAGE.HARD_5 || m_FinalReachedStage == E_STAGE.HADES_5)
        {
            return "5";
        }
        else if (m_FinalReachedStage == E_STAGE.EASY_6 || m_FinalReachedStage == E_STAGE.NORMAL_6 || m_FinalReachedStage == E_STAGE.HARD_6 || m_FinalReachedStage == E_STAGE.HADES_6)
        {
            return "6";
        }
        else
        {
            return "";
        }
    }

    public string PlayedDateToString()
    {
        return string.Format("{0:yyyy/MM/dd}", m_PlayedDate);
    }
}

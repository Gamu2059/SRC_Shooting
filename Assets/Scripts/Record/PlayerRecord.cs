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
    public ulong m_FinalScore { get; private set; }

    /// <summary>
    /// ゲーム終了時の到達ステージ
    /// (後でenumに変更予定)
    /// </summary>
    public E_CHAPTER m_FinalReachedStage { get; private set; }

    /// <summary>
    /// プレイした日付
    /// </summary>
    /// <value></value>
    public System.DateTime m_PlayedDate { get; private set; }

    /// <summary>
    /// プレイヤーネーム
    /// </summary>
    public string m_PlayerName { get; private set; }

    public PlayerRecord()
    {

    }

    public PlayerRecord(string name, ulong finalScore, E_CHAPTER reachedStage, System.DateTime date)
    {
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
        //if(m_FinalReachedStage == E_CHAPTER.CHAPTER_0 || m_FinalReachedStage == E_CHAPTER.CHAPTER_1 || m_FinalReachedStage == E_CHAPTER.CHAPTER_2 || m_FinalReachedStage == E_CHAPTER.CHAPTER_3 || m_FinalReachedStage == E_CHAPTER.CHAPTER_4
        //    || m_FinalReachedStage == E_CHAPTER.CHAPTER_5 || m_FinalReachedStage == E_CHAPTER.CHAPTER_6)
        //{
        //    return E_DIFFICULTY.EASY;
        //}
        //else if(m_FinalReachedStage == E_CHAPTER.NORMAL_0 || m_FinalReachedStage == E_CHAPTER.NORMAL_1 || m_FinalReachedStage == E_CHAPTER.NORMAL_2 || m_FinalReachedStage == E_CHAPTER.NORMAL_3 || m_FinalReachedStage == E_CHAPTER.NORMAL_4
        //    || m_FinalReachedStage == E_CHAPTER.NORMAL_5 || m_FinalReachedStage == E_CHAPTER.NORMAL_6)
        //{
        //    return E_DIFFICULTY.NORMAL;
        //}
        //else if(m_FinalReachedStage == E_CHAPTER.HARD_0 || m_FinalReachedStage == E_CHAPTER.HARD_1 || m_FinalReachedStage == E_CHAPTER.HARD_2 || m_FinalReachedStage == E_CHAPTER.HARD_3 || m_FinalReachedStage == E_CHAPTER.HARD_4
        //    || m_FinalReachedStage == E_CHAPTER.HARD_5 || m_FinalReachedStage == E_CHAPTER.HARD_6)
        //{
        //    return E_DIFFICULTY.HARD;
        //}
        //else
        //{
        //    return E_DIFFICULTY.HADES;
        //}
        return E_DIFFICULTY.NORMAL;
    }

    public string FinalReachedStageToString()
    {
        //if(m_FinalReachedStage == E_CHAPTER.CHAPTER_0 || m_FinalReachedStage == E_CHAPTER.NORMAL_0 || m_FinalReachedStage == E_CHAPTER.HARD_0 || m_FinalReachedStage == E_CHAPTER.HADES_0)
        //{
        //    return "0";
        //}
        //else if(m_FinalReachedStage == E_CHAPTER.CHAPTER_1 || m_FinalReachedStage == E_CHAPTER.NORMAL_1 || m_FinalReachedStage == E_CHAPTER.HARD_1 || m_FinalReachedStage == E_CHAPTER.HADES_1)
        //{
        //    return "1";
        //}
        //else if(m_FinalReachedStage == E_CHAPTER.CHAPTER_2 || m_FinalReachedStage == E_CHAPTER.NORMAL_2 || m_FinalReachedStage == E_CHAPTER.HARD_2 || m_FinalReachedStage == E_CHAPTER.HADES_2)
        //{
        //    return "2";
        //}
        //else if(m_FinalReachedStage == E_CHAPTER.CHAPTER_3 || m_FinalReachedStage == E_CHAPTER.NORMAL_3 || m_FinalReachedStage == E_CHAPTER.HARD_3 || m_FinalReachedStage == E_CHAPTER.HADES_3)
        //{
        //    return "3";
        //}
        //else if (m_FinalReachedStage == E_CHAPTER.CHAPTER_4 || m_FinalReachedStage == E_CHAPTER.NORMAL_4 || m_FinalReachedStage == E_CHAPTER.HARD_4 || m_FinalReachedStage == E_CHAPTER.HADES_4)
        //{
        //    return "4";
        //}
        //else if(m_FinalReachedStage == E_CHAPTER.CHAPTER_5 || m_FinalReachedStage == E_CHAPTER.NORMAL_5 || m_FinalReachedStage == E_CHAPTER.HARD_5 || m_FinalReachedStage == E_CHAPTER.HADES_5)
        //{
        //    return "5";
        //}
        //else if (m_FinalReachedStage == E_CHAPTER.CHAPTER_6 || m_FinalReachedStage == E_CHAPTER.NORMAL_6 || m_FinalReachedStage == E_CHAPTER.HARD_6 || m_FinalReachedStage == E_CHAPTER.HADES_6)
        //{
        //    return "6";
        //}
        //else
        //{
        //    return "";
        //}

        return "";
    }

    public string PlayedDateToString()
    {
        return string.Format("{0:yyyy/MM/dd}", m_PlayedDate);
    }
}

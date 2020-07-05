using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterRankingElement : MonoBehaviour
{
    [SerializeField]
    private Text m_Rank;

    [SerializeField]
    private Text m_PlayerName;

    [SerializeField]
    private Text m_Score;

    [SerializeField]
    private Text m_Date;

    public void SetRanking(int rank, PlayerRecord record)
    {
        if (record == null)
        {
            return;
        }

        m_Rank.text = rank.ToString();
        m_PlayerName.text = record.PlayerName;
        m_Score.text = record.FinalScoreToString();
        m_Date.text = record.PlayedDate;
    }
}

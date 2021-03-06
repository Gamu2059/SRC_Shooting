﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StoryModeRankingTextSet
{
    [SerializeField]
    private Text m_NameText;
    [SerializeField]
    private Text m_ScoreText;
    [SerializeField]
    private Text m_ChapterText;
    [SerializeField]
    private Text m_DateText;

    public void SetTextFromPlayerRecord(PlayerRecord rec)
    {
        m_NameText.text = rec.PlayerName;
        m_ScoreText.text = rec.FinalScoreToString();
        //m_ChapterText.text = rec.FinalReachedStageToString();
        m_DateText.text = rec.PlayedDate;
    }
}

public class StoryModeRankingTextSetManager : MonoBehaviour
{
    [SerializeField]
    private StoryModeRankingTextSet[] m_StoryModeRankingTextSets;

    public void SetStoryModeRaningText(List<PlayerRecord> recs)
    {
        for(int i=0; i<m_StoryModeRankingTextSets.Length; i++)
        {
            m_StoryModeRankingTextSets[i].SetTextFromPlayerRecord(recs[i]);
        }
    }
}

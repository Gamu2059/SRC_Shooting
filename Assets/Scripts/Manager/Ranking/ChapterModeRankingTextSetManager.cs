using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ChapterModeRankingTextSet
{
    [SerializeField]
    private Text m_NameText;
    [SerializeField]
    private Text m_ScoreText;
    [SerializeField]
    private Text m_DateText;

    public void SetTextFromPlayerRecord(PlayerRecord rec)
    {
        m_NameText.text = rec.m_PlayerName;
        m_ScoreText.text = rec.FinalScoreToString();
        m_DateText.text = rec.PlayedDateToString();
    }
}

public class ChapterModeRankingTextSetManager : MonoBehaviour
{
    [SerializeField]
    private ChapterModeRankingTextSet[] m_ChapterModeRankingTextSets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

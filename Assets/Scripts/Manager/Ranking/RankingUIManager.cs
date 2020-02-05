#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingUIManager : ControllableMonoBehavior
{
    private const string MENU_ENABLE = "menu_enable";
    private const string MENU_DISABLE = "menu_disable";
    private const string MENU_ENABLE_FORCE = "menu_enable_force";
    private const string MENU_DISABLE_FORCE = "menu_disable_force";

    private const string POPUP_ENABLE = "popup_enable";
    private const string POPUP_DISABLE = "popup_disable";
    private const string POPUP_DISABLE_FORCE = "popup_disable_force";
    private const string POPUP_BACK_ENABLE = "popup_back_enable";
    private const string POPUP_BACK_DISABLE = "popup_back_disable";
    private const string POPUP_BACK_DISABLE_FORCE = "popup_back_disable_force";

    [SerializeField]
    private Animator[] m_MenuAnimators;

    [SerializeField]
    private Animator m_PopupBackAnimator;

    [SerializeField]
    private Animator m_StoryRankingAnimator;

    [SerializeField]
    private Animator m_ChapterRankingAnimator;

    [SerializeField]
    private Text m_StoryRankingText;

    [SerializeField]
    private Text m_ChapterRankingText;

    [SerializeField]
    private int m_StoryRankingDisplayNum;

    private int m_EnableIdx;

    private string m_StoryRankingOutputText;

    private List<string> m_ChapterRankingOutputTexts;

    public override void OnInitialize()
    {
        base.OnInitialize();
        DisableAllMenuForce();
        ForcusMenu(0, true);
        m_StoryRankingOutputText = StoryRankingToString();

        m_ChapterRankingOutputTexts = new List<string>();
        for(int i = 0; i < 7; i++)
        {
            m_ChapterRankingOutputTexts.Add(ChapterRankingToString(i));
        }
    }

    private string StoryRankingToString()
    {
        var sb = new System.Text.StringBuilder();
        var rec = PlayerRecordManager.Instance.GetRecordsInRange(m_StoryRankingDisplayNum);
        sb.Append(string.Format("<size=48>{0,-45}</size>\n\n\n\n\n\n", "STORY MODE RANKING"));
        sb.Append(string.Format("{0,13}  {1,12}  {2,16}{3,13} {4,11}                \n\n\n\n", "RANK", "NAME", "SCORE", "STAGE", "DATE"));
        for (int i = 0; i < m_StoryRankingDisplayNum; i++)
        {
            if (i == m_StoryRankingDisplayNum - 1)
            {
                sb.Append(string.Format("{0,20}{1,20}{2,20}{3,10}{4,20}            ", i + 1, rec[i].m_PlayerName, rec[i].FinalScoreToString(), rec[i].FinalReachedStageToString(), rec[i].PlayedDateToString()));
            }
            else
            {
                sb.Append(string.Format("{0,20}{1,20}{2,20}{3,10}{4,20}            \n\n\n\n", i + 1, rec[i].m_PlayerName, rec[i].FinalScoreToString(), rec[i].FinalReachedStageToString(), rec[i].PlayedDateToString()));
            }
        }
        return sb.ToString();
    }

    private string ChapterRankingToString(int chap)
    {
        var sb = new System.Text.StringBuilder();
        var rec = PlayerRecordManager.Instance.GetRecordsInRange(m_StoryRankingDisplayNum);
        sb.Append(string.Format("<size=48>{0,-40}</size>\n\n\n", "CHAPTER MODE RANKING"));
        sb.Append(string.Format("{0} {1,-60}\n\n\n\n", "Stage", chap));
        sb.Append(string.Format("{0,-20}{1,-21}{2,-20}{3,-22}\n\n\n\n", "RANK", "NAME", "SCORE", "DATE"));
        for (int i = 0; i < m_StoryRankingDisplayNum; i++)
        {
            if (i == m_StoryRankingDisplayNum - 1)
            {
                sb.Append(string.Format("{0,-20}{1,-20}{2,-20}{3,-22}", i + 1, rec[i].m_PlayerName, rec[i].FinalScoreToString(), rec[i].PlayedDateToString()));
            }
            else
            {
                sb.Append(string.Format("{0,-20}{1,-20}{2,-20}{3,-22}\n\n\n\n", i + 1, rec[i].m_PlayerName, rec[i].FinalScoreToString(), rec[i].PlayedDateToString()));
            }
        }
        return sb.ToString();
    }

    public void SetChapterRankingOutputText(int chap)
    {
        m_ChapterRankingText.text = m_ChapterRankingOutputTexts[chap];
    }

    public void DisableAllMenuForce()
    {
        foreach(var m in m_MenuAnimators)
        {
            m.Play(MENU_DISABLE_FORCE, 0);
        }
    }

    private void EnableMenu(int idx)
    {
        var i = Mathf.Clamp(idx, 0, m_MenuAnimators.Length - 1);
        var m = m_MenuAnimators[i];
        m.Play(MENU_ENABLE_FORCE, 0);
    }

    private void DisableMenu(int idx)
    {
        var i = Mathf.Clamp(idx, 0, m_MenuAnimators.Length - 1);
        var m = m_MenuAnimators[i];
        m.Play(MENU_DISABLE, 0);
    }

    public void ForcusMenu(int idx, bool isForce = false)
    {
        if (m_EnableIdx != idx || isForce)
        {
            DisableMenu(m_EnableIdx);
            EnableMenu(idx);
            m_EnableIdx = idx;
        }
    }

    public void DisableAllPopupForce()
    {
        m_PopupBackAnimator.Play(POPUP_BACK_DISABLE_FORCE);
        m_StoryRankingAnimator.Play(POPUP_DISABLE_FORCE);
        m_ChapterRankingAnimator.Play(POPUP_DISABLE_FORCE);
    }

    public void EnableStoryRanking()
    {
        m_StoryRankingText.text = m_StoryRankingOutputText;
        m_PopupBackAnimator.Play(POPUP_BACK_ENABLE);
        m_StoryRankingAnimator.Play(POPUP_ENABLE);
    }

    public void DisableStoryRanking()
    {
        m_PopupBackAnimator.Play(POPUP_BACK_DISABLE);
        m_StoryRankingAnimator.Play(POPUP_DISABLE);
    }

    public void EnableChapterRanking()
    {
        m_ChapterRankingText.text = m_ChapterRankingOutputTexts[0];
        m_PopupBackAnimator.Play(POPUP_BACK_ENABLE);
        m_ChapterRankingAnimator.Play(POPUP_ENABLE);
    }

    public void DisableChapterRanking()
    {
        m_PopupBackAnimator.Play(POPUP_BACK_DISABLE);
        m_ChapterRankingAnimator.Play(POPUP_DISABLE);
    }
}

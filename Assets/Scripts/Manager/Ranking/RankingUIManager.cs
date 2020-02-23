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

    //[SerializeField]
    //private Animator[] m_MenuAnimators;

    //[SerializeField]
    //private Animator m_PopupBackAnimator;

    //[SerializeField]
    //private Animator m_StoryRankingAnimator;

    //[SerializeField]
    //private Animator m_ChapterRankingAnimator;

    //[SerializeField]
    //private Text m_StoryRankingText;

    //[SerializeField]
    //private Text m_ChapterRankingText;

    //[SerializeField]
    //private int m_RankingDisplayNum;

    //private int m_EnableIdx;

    //private List<string> m_StoryModeRankingOutputTexts;

    //private List<string> m_ChapterModeRankingOutputTexts;

    [SerializeField]
    private StoryModeRankingTextSetManager m_StoryModeRankingTextSetManager;

    [SerializeField]
    private bool m_IsStoryModeRankingAppear;

    [SerializeField]
    private ChapterModeRankingTextSetManager m_ChapterModeRankingTextSetManager;

    [SerializeField]
    private bool m_IsChapterModeRankingAppear;

    public override void OnInitialize()
    {
        base.OnInitialize();
        //DisableAllMenuForce();
        //ForcusMenu(0, true);

        //InitStoryModeRankingOutputTexts();
        //InitChapterModeRankingOutputTexts();

        m_StoryModeRankingTextSetManager.gameObject.SetActive(m_IsStoryModeRankingAppear);
        m_ChapterModeRankingTextSetManager.gameObject.SetActive(m_IsChapterModeRankingAppear);
    }

    private void InitStoryModeRankingOutputTexts()
    {
        //m_StoryModeRankingOutputTexts = new List<string>();
        //foreach(E_DIFFICULTY di in System.Enum.GetValues(typeof(E_DIFFICULTY)))
        //{
        //    m_StoryModeRankingOutputTexts.Add(GetStoryModeRankingToString(di));
        //}
    }

    private void GetStoryModeRankingToString(E_DIFFICULTY difficulty)
    {
        //var sb = new System.Text.StringBuilder();
        //var rec = PlayerRecordManager.Instance.GetStoryModeRecordsInRange(difficulty, m_RankingDisplayNum);

        //sb.Append(string.Format("<size=48>{0,-45}</size>\n\n\n", "STORY MODE RANKING"));
        //switch (difficulty)
        //{
        //    case E_DIFFICULTY.EASY:
        //        sb.Append(string.Format("{0,-60}\n\n\n\n", "EASY"));
        //        break;
        //    case E_DIFFICULTY.NORMAL:
        //        sb.Append(string.Format("{0,-60}\n\n\n\n", "NORMAL"));
        //        break;
        //    case E_DIFFICULTY.HARD:
        //        sb.Append(string.Format("{0,-60}\n\n\n\n", "HARD"));
        //        break;
        //    case E_DIFFICULTY.HADES:
        //        sb.Append(string.Format("{0,-60}\n\n\n\n", "HADES"));
        //        break;
        //}
        //sb.Append(string.Format("{0,13}  {1,12}  {2,16}{3,13} {4,11}                \n\n\n\n", "RANK", "NAME", "SCORE", "STAGE", "DATE"));
        //for (int i = 0; i < m_RankingDisplayNum; i++)
        //{
        //    if (i == m_RankingDisplayNum - 1)
        //    {
        //        sb.Append(string.Format("{0,20}{1,20}{2,20}{3,10}{4,20}            ", i + 1, rec[i].m_PlayerName, rec[i].FinalScoreToString(), rec[i].FinalReachedStageToString(), rec[i].PlayedDateToString()));
        //    }
        //    else
        //    {
        //        sb.Append(string.Format("{0,20}{1,20}{2,20}{3,10}{4,20}            \n\n\n\n", i + 1, rec[i].m_PlayerName, rec[i].FinalScoreToString(), rec[i].FinalReachedStageToString(), rec[i].PlayedDateToString()));
        //    }
        //}
        //return sb.ToString();
    }

    private void InitChapterModeRankingOutputTexts()
    {
        //m_ChapterModeRankingOutputTexts = new List<string>();
        //foreach (E_STATE st in System.Enum.GetValues(typeof(E_STATE)))
        //{
        //    m_ChapterModeRankingOutputTexts.Add(GetChapterModeRankingToString(st));
        //}
    }

    private void GetChapterModeRankingToString(E_STATE stage)
    {
        //var sb = new System.Text.StringBuilder();
        //var rec = PlayerRecordManager.Instance.GetChapterModeRecordsInRange(stage, m_RankingDisplayNum);
        //sb.Append(string.Format("<size=48>{0,-40}</size>\n\n\n", "CHAPTER MODE RANKING"));

        //var num = stage.GetHashCode();
        //if (0 <= num && num < 7)
        //{
        //    switch (num % 7)
        //    {
        //        case 0:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "EASY STAGE0"));
        //            break;
        //        case 1:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "EASY STAGE1"));
        //            break;
        //        case 2:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "EASY STAGE2"));
        //            break;
        //        case 3:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "EASY STAGE3"));
        //            break;
        //        case 4:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "EASY STAGE4"));
        //            break;
        //        case 5:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "EASY STAGE5"));
        //            break;
        //        case 6:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "EASY STAGE6"));
        //            break;
        //    }
        //}
        //else if (7 <= num && num < 14)
        //{
        //    switch (num % 7)
        //    {
        //        case 0:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "NORMAL STAGE0"));
        //            break;
        //        case 1:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "NORMAL STAGE1"));
        //            break;
        //        case 2:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "NORMAL STAGE2"));
        //            break;
        //        case 3:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "NORMAL STAGE3"));
        //            break;
        //        case 4:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "NORMAL STAGE4"));
        //            break;
        //        case 5:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "NORMAL STAGE5"));
        //            break;
        //        case 6:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "NORMAL STAGE6"));
        //            break;
        //    }
        //}
        //else if (14 <= num && num < 21)
        //{
        //    switch (num % 7)
        //    {
        //        case 0:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HARD STAGE0"));
        //            break;
        //        case 1:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HARD STAGE1"));
        //            break;
        //        case 2:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HARD STAGE2"));
        //            break;
        //        case 3:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HARD STAGE3"));
        //            break;
        //        case 4:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HARD STAGE4"));
        //            break;
        //        case 5:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HARD STAGE5"));
        //            break;
        //        case 6:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HARD STAGE6"));
        //            break;
        //    }
        //}
        //else
        //{
        //    switch (num % 7)
        //    {
        //        case 0:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HADES STAGE0"));
        //            break;
        //        case 1:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HADES STAGE1"));
        //            break;
        //        case 2:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HADES STAGE2"));
        //            break;
        //        case 3:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HADES STAGE3"));
        //            break;
        //        case 4:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HADES STAGE4"));
        //            break;
        //        case 5:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HADES STAGE5"));
        //            break;
        //        case 6:
        //            sb.Append(string.Format("{0,-60}\n\n\n\n", "HADES STAGE6"));
        //            break;
        //    }
        //}

        //sb.Append(string.Format("{0,-20}{1,-21}{2,-20}{3,-22}\n\n\n\n", "RANK", "NAME", "SCORE", "DATE"));
        //for (int i = 0; i < m_RankingDisplayNum; i++)
        //{
        //    if (i == m_RankingDisplayNum - 1)
        //    {
        //        sb.Append(string.Format("{0,-20}{1,-20}{2,-20}{3,-22}", i + 1, rec[i].m_PlayerName, rec[i].FinalScoreToString(), rec[i].PlayedDateToString()));
        //    }
        //    else
        //    {
        //        sb.Append(string.Format("{0,-20}{1,-20}{2,-20}{3,-22}\n\n\n\n", i + 1, rec[i].m_PlayerName, rec[i].FinalScoreToString(), rec[i].PlayedDateToString()));
        //    }
        //}
        //return sb.ToString();
    }

    public void SetStoryModeRankingOutputText(int idx)
    {
        //m_StoryRankingText.text = m_StoryModeRankingOutputTexts[idx];
    }

    public void SetChapterModeRankingOutputText(int idx)
    {
        //m_ChapterRankingText.text = m_ChapterModeRankingOutputTexts[idx];
    }

    public void DisableAllMenuForce()
    {
        //foreach (var m in m_MenuAnimators)
        //{
        //    m.Play(MENU_DISABLE_FORCE, 0);
        //}
    }

    private void EnableMenu(int idx)
    {
        //var i = Mathf.Clamp(idx, 0, m_MenuAnimators.Length - 1);
        //var m = m_MenuAnimators[i];
        //m.Play(MENU_ENABLE_FORCE, 0);
    }

    private void DisableMenu(int idx)
    {
        //var i = Mathf.Clamp(idx, 0, m_MenuAnimators.Length - 1);
        //var m = m_MenuAnimators[i];
        //m.Play(MENU_DISABLE, 0);
    }

    public void ForcusMenu(int idx, bool isForce = false)
    {
        //if (m_EnableIdx != idx || isForce)
        //{
        //    DisableMenu(m_EnableIdx);
        //    EnableMenu(idx);
        //    m_EnableIdx = idx;
        //}
    }

    public void DisableAllPopupForce()
    {
        //m_PopupBackAnimator.Play(POPUP_BACK_DISABLE_FORCE);
        //m_StoryRankingAnimator.Play(POPUP_DISABLE_FORCE);
        //m_ChapterRankingAnimator.Play(POPUP_DISABLE_FORCE);
    }

    public void EnableStoryRanking()
    {
        //m_StoryRankingText.text = m_StoryModeRankingOutputTexts[0];
        //m_PopupBackAnimator.Play(POPUP_BACK_ENABLE);
        //m_StoryRankingAnimator.Play(POPUP_ENABLE);
    }

    public void DisableStoryRanking()
    {
        //m_PopupBackAnimator.Play(POPUP_BACK_DISABLE);
        //m_StoryRankingAnimator.Play(POPUP_DISABLE);
    }

    public void EnableChapterRanking()
    {
        //m_ChapterRankingText.text = m_ChapterModeRankingOutputTexts[0];
        //m_PopupBackAnimator.Play(POPUP_BACK_ENABLE);
        //m_ChapterRankingAnimator.Play(POPUP_ENABLE);
    }

    public void DisableChapterRanking()
    {
        //m_PopupBackAnimator.Play(POPUP_BACK_DISABLE);
        //m_ChapterRankingAnimator.Play(POPUP_DISABLE);
    }
}

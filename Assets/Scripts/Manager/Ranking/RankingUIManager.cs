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

    [SerializeField]
    private Animator[] m_MenuAnimators;

    private int m_EnableIdx;

    [SerializeField]
    private StoryModeRankingTextSetManager m_StoryModeRankingTextSetManager;

    [SerializeField]
    private ChapterModeRankingTextSetManager m_ChapterModeRankingTextSetManager;

    [SerializeField]
    private Text m_ChapterText;

    [SerializeField]
    private Text m_DifficultyText;

    private Dictionary<int, List<PlayerRecord>> m_Records;

    private int m_RecordIndex;

    public override void OnInitialize()
    {
        base.OnInitialize();
        DisableAllMenuForce();

        m_Records = new Dictionary<int, List<PlayerRecord>>
        {
            { 0, PlayerRecordManager.Instance.GetStoryModeRecordsInRange(E_DIFFICULTY.EASY, 10) },
            { 1, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.CHAPTER_0, 10) },
            { 2, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.CHAPTER_1, 10) },
            { 3, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.CHAPTER_2, 10) },
            { 4, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.CHAPTER_3, 10) },
            { 5, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.CHAPTER_4, 10) },
            { 6, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.CHAPTER_5, 10) },
            { 7, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.CHAPTER_6, 10) },

            //{ 8, PlayerRecordManager.Instance.GetStoryModeRecordsInRange(E_DIFFICULTY.NORMAL, 10) },
            //{ 9, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.NORMAL_0, 10) },
            //{ 10, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.NORMAL_1, 10) },
            //{ 11, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.NORMAL_2, 10) },
            //{ 12, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.NORMAL_3, 10) },
            //{ 13, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.NORMAL_4, 10) },
            //{ 14, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.NORMAL_5, 10) },
            //{ 15, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.NORMAL_6, 10) },

            //{ 16, PlayerRecordManager.Instance.GetStoryModeRecordsInRange(E_DIFFICULTY.HARD, 10) },
            //{ 17, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HARD_0, 10) },
            //{ 18, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HARD_1, 10) },
            //{ 19, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HARD_2, 10) },
            //{ 20, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HARD_3, 10) },
            //{ 21, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HARD_4, 10) },
            //{ 22, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HARD_5, 10) },
            //{ 23, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HARD_6, 10) },

            //{ 24, PlayerRecordManager.Instance.GetStoryModeRecordsInRange(E_DIFFICULTY.HADES, 10) },
            //{ 25, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HADES_0, 10) },
            //{ 26, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HADES_1, 10) },
            //{ 27, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HADES_2, 10) },
            //{ 28, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HADES_3, 10) },
            //{ 29, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HADES_4, 10) },
            //{ 30, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HADES_5, 10) },
            //{ 31, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_CHAPTER.HADES_6, 10) }
        };
        m_RecordIndex = 0;
        SetRankingText(m_RecordIndex);
    }

    public void SetRankingText(int idx)
    {
        if (idx % 8 == 0)
        {
            m_StoryModeRankingTextSetManager.gameObject.SetActive(true);
            m_ChapterModeRankingTextSetManager.gameObject.SetActive(false);
            m_StoryModeRankingTextSetManager.SetStoryModeRaningText(m_Records[idx]);
        }
        else
        {
            m_StoryModeRankingTextSetManager.gameObject.SetActive(false);
            m_ChapterModeRankingTextSetManager.gameObject.SetActive(true);
            m_ChapterModeRankingTextSetManager.SetChapterModeRankingText(m_Records[idx]);
        }

        m_ChapterText.text = GetChapterText(idx);
        m_DifficultyText.text = GetDifficultyText(idx);
    }

    private string GetDifficultyText(int idx)
    {
        if (0 <= idx && idx < 8)
        {
            return "EASY";
        }
        else if(8 <= idx && idx < 16)
        {
            return "NORMAL";
        }
        else if(16 <= idx && idx < 24)
        {
            return "HARD";
        }
        else if(24 <= idx && idx < 32)
        {
            return "HADES";
        }
        else
        {
            return "DIFFICULTY";
        }
    }
            
    private string GetChapterText(int idx)
    {
        var c = idx % 8;
        switch (c)
        {
            case 0:
                return "STORY";
            case 1:
                return "CHAPTER 0";
            case 2:
                return "CHAPTER 1";
            case 3:
                return "CHAPTER 2";
            case 4:
                return "CHAPTER 3";
            case 5:
                return "CHAPTER 4";
            case 6:
                return "CHAPTER 5";
            case 7:
                return "CHAPTER 6";
            default:
                return "CHAPTER";
        }
    }

    public void DisableAllMenuForce()
    {
        foreach (var m in m_MenuAnimators)
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
}

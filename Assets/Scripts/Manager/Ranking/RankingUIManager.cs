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
    private bool m_IsStoryModeRankingAppear;

    [SerializeField]
    private ChapterModeRankingTextSetManager m_ChapterModeRankingTextSetManager;

    [SerializeField]
    private bool m_IsChapterModeRankingAppear;

    private Dictionary<int, List<PlayerRecord>> m_Records;

    private int m_DisplayIndex;

    public override void OnInitialize()
    {
        base.OnInitialize();
        DisableAllMenuForce();

        m_Records = new Dictionary<int, List<PlayerRecord>>
        {
            { 0, PlayerRecordManager.Instance.GetStoryModeRecordsInRange(E_DIFFICULTY.EASY, 10) },
            { 1, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.EASY_0, 10) },
            { 2, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.EASY_1, 10) },
            { 3, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.EASY_2, 10) },
            { 4, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.EASY_3, 10) },
            { 5, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.EASY_4, 10) },
            { 6, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.EASY_5, 10) },
            { 7, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.EASY_6, 10) },

            { 8, PlayerRecordManager.Instance.GetStoryModeRecordsInRange(E_DIFFICULTY.NORMAL, 10) },
            { 9, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.NORMAL_0, 10) },
            { 10, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.NORMAL_1, 10) },
            { 11, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.NORMAL_2, 10) },
            { 12, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.NORMAL_3, 10) },
            { 13, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.NORMAL_4, 10) },
            { 14, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.NORMAL_5, 10) },
            { 15, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.NORMAL_6, 10) },

            { 16, PlayerRecordManager.Instance.GetStoryModeRecordsInRange(E_DIFFICULTY.HARD, 10) },
            { 17, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HARD_0, 10) },
            { 18, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HARD_1, 10) },
            { 19, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HARD_2, 10) },
            { 20, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HARD_3, 10) },
            { 21, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HARD_4, 10) },
            { 22, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HARD_5, 10) },
            { 23, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HARD_6, 10) },

            { 24, PlayerRecordManager.Instance.GetStoryModeRecordsInRange(E_DIFFICULTY.HADES, 10) },
            { 25, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HADES_0, 10) },
            { 26, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HADES_1, 10) },
            { 27, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HADES_2, 10) },
            { 28, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HADES_3, 10) },
            { 29, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HADES_4, 10) },
            { 30, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HADES_5, 10) },
            { 31, PlayerRecordManager.Instance.GetChapterModeRecordsInRange(E_STATE.HADES_6, 10) }
        };

        m_DisplayIndex = 0;

        //m_StoryModeRankingTextSetManager.gameObject.SetActive(m_IsStoryModeRankingAppear);
        //m_ChapterModeRankingTextSetManager.gameObject.SetActive(m_IsChapterModeRankingAppear);

        SetRankingText(m_DisplayIndex);
    }

    public void SetRankingText(int idx)
    {
        if(idx % 8 == 0)
        {
            m_StoryModeRankingTextSetManager.SetStoryModeRaningText(m_Records[idx]);
        }
        else
        {
            m_ChapterModeRankingTextSetManager.SetChapterModeRankingText(m_Records[idx]);
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

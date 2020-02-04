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

    private int m_EnableIdx;

    public override void OnInitialize()
    {
        base.OnInitialize();
        DisableAllMenuForce();
        ForcusMenu(0, true);
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
        m_PopupBackAnimator.Play(POPUP_BACK_ENABLE);
        m_ChapterRankingAnimator.Play(POPUP_ENABLE);
    }

    public void DisableChapterRanking()
    {
        m_PopupBackAnimator.Play(POPUP_BACK_DISABLE);
        m_ChapterRankingAnimator.Play(POPUP_DISABLE);
    }
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RankingManager : ControllableMonoBehavior
{
    private enum E_RANKING_MENU_STATE
    {
        FORCUS_STORY,
        SELECT_STORY,
        FORCUS_CHAPTER,
        SELECT_CHAPTER,
        FORCUS_EXIT,
        SELECT_EXIT,
    }

    [SerializeField]
    private RankingUIManager m_UiManager;

    [SerializeField]
    private Transform m_Overall;

    [SerializeField]
    private PlaySoundParam m_CursorSe;

    [SerializeField]
    private PlaySoundParam m_OkSe;

    [SerializeField]
    private PlaySoundParam m_CancelSe;

    [SerializeField]
    private float m_WaitCursorTime;

    private TitleInputManager InputManager;
    private StateMachine<E_RANKING_MENU_STATE> m_StateMachine;

    private bool m_EnableMenu;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_RANKING_MENU_STATE>();

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.FORCUS_STORY)
        {
            m_OnStart = StartOnFocusStory,
            m_OnUpdate = UpdateOnFocusStory,
            m_OnEnd = EndOnFocusStory,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.FORCUS_CHAPTER)
        {
            m_OnStart = StartOnFocusChapter,
            m_OnUpdate = UpdateOnFocusChapter,
            m_OnEnd = EndOnFocusChapter,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.FORCUS_EXIT)
        {
            m_OnStart = StartOnFocusEnd,
            m_OnUpdate = UpdateOnFocusEnd,
            m_OnEnd = EndOnFocusEnd,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.SELECT_STORY) 
        {
            m_OnStart = StartOnSelectStory,
            m_OnUpdate = UpdateOnSelectStory,
            m_OnEnd = EndOnSelectStory,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.SELECT_CHAPTER) 
        { 
            m_OnStart = StartOnSelectChapter,
            m_OnUpdate = UpdateOnSelectChapter,
            m_OnEnd = EndOnSelectChapter,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.SELECT_EXIT) 
        { 
            m_OnStart = StartOnSelectExit,
            m_OnUpdate = UpdateOnSelectExit,
            m_OnEnd = EndOnSelectExit,
        });
    }

    #endregion
}

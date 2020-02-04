#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RankingManager : ControllableMonoBehavior
{
    private enum E_RANKING_MENU_STATE
    {
        FORCUS_STORY_RANKING,
        SELECT_STORY_RANKING,
        FORCUS_CHAPTER_RANKING,
        SELECT_CHAPTER_RANKING,
        FORCUS_EXIT,
        SELECT_EXIT,
    }

    [SerializeField]
    private RankingUIManager m_UiManager;

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

    private bool m_EnableMove;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_RANKING_MENU_STATE>();

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.FORCUS_STORY_RANKING)
        {
            m_OnStart = StartOnFocusStory,
            m_OnUpdate = UpdateOnFocusStory,
            m_OnEnd = EndOnFocusStory,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.FORCUS_CHAPTER_RANKING)
        {
            m_OnStart = StartOnFocusChapter,
            m_OnUpdate = UpdateOnFocusChapter,
            m_OnEnd = EndOnFocusChapter,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.FORCUS_EXIT)
        {
            m_OnStart = StartOnFocusExit,
            m_OnUpdate = UpdateOnFocusExit,
            m_OnEnd = EndOnFocusExit,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.SELECT_STORY_RANKING) 
        {
            m_OnStart = StartOnSelectStory,
            m_OnUpdate = UpdateOnSelectStory,
            m_OnEnd = EndOnSelectStory,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.SELECT_CHAPTER_RANKING) 
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

        InputManager = new TitleInputManager();
        InputManager.OnInitialize();
        m_UiManager.OnInitialize();
        m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_STORY_RANKING);
    }

    public override void OnFinalize()
    {
        InputManager.RemoveInput();

        m_UiManager.OnFinalize();
        InputManager.OnFinalize();
        m_StateMachine.OnFinalize();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
        InputManager.OnStart();
        InputManager.RegistInput();
        m_EnableMove = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        InputManager.OnUpdate();
        m_StateMachine?.OnUpdate();
    }

    #endregion

    private void CheckForcusAction(Action onMoveUp, Action onMoveDown)
    {
        if (!m_EnableMove)
        {
            return;
        }

        var v = InputManager.MoveDir.y;
        if (v > 0.8)
        {
            if (onMoveUp != null)
            {
                PlayCursor();
                WaitCursor();
                onMoveUp.Invoke();
            }
        }
        else if (v < -0.8)
        {
            if (onMoveDown != null)
            {
                PlayCursor();
                WaitCursor();
                onMoveDown.Invoke();
            }
        }
    }

    private void WaitCursor()
    {
        m_EnableMove = false;
        var t = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, m_WaitCursorTime, () => m_EnableMove = true);
        TimerManager.Instance.RegistTimer(t);
    }

    private void CheckSelectAction(Action onSelect)
    {
        if (InputManager.Submit == E_INPUT_STATE.DOWN)
        {
            if (onSelect != null)
            {
                PlayOk();
                onSelect.Invoke();
            }
        }
    }

    private void CheckCancelAction(Action onCancel)
    {
        if (InputManager.Cancel == E_INPUT_STATE.DOWN)
        {
            if (onCancel != null)
            {
                PlayCancel();
                onCancel.Invoke();
            }
        }
    }

    private void PlayCursor()
    {
        AudioManager.Instance.Play(m_CursorSe);
    }

    private void PlayOk()
    {
        AudioManager.Instance.Play(m_OkSe);
    }

    private void PlayCancel()
    {
        AudioManager.Instance.Play(m_CancelSe);
    }

    #region Focus Story

    private void StartOnFocusStory()
    {
        m_UiManager.ForcusMenu(0);
    }

    private void UpdateOnFocusStory()
    {
        CheckForcusAction(null, () => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_CHAPTER_RANKING));
        CheckSelectAction(() => m_StateMachine.Goto(E_RANKING_MENU_STATE.SELECT_STORY_RANKING));
    }

    private void EndOnFocusStory()
    {

    }

    #endregion

    #region Select Story

    private void StartOnSelectStory()
    {
        m_UiManager.EnableStoryRanking();
    }

    private void UpdateOnSelectStory()
    {
        CheckCancelAction(() => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_STORY_RANKING));
    }

    private void EndOnSelectStory()
    {
        m_UiManager.DisableStoryRanking();
    }

    #endregion

    #region Focus Chapter

    private void StartOnFocusChapter()
    {
        m_UiManager.ForcusMenu(1);
    }

    private void UpdateOnFocusChapter()
    {
        CheckForcusAction(
            () => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_STORY_RANKING),
            () => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_EXIT));
        CheckSelectAction(() => m_StateMachine.Goto(E_RANKING_MENU_STATE.SELECT_CHAPTER_RANKING));
    }

    private void EndOnFocusChapter()
    {

    }

    #endregion

    #region Select Chapter

    private void StartOnSelectChapter()
    {
        m_UiManager.EnableChapterRanking();
    }

    private void UpdateOnSelectChapter()
    {
        CheckCancelAction(() => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_CHAPTER_RANKING));
    }

    private void EndOnSelectChapter()
    {
        m_UiManager.DisableChapterRanking();
    }

    #endregion

    #region Focus Exit

    private void StartOnFocusExit()
    {
        m_UiManager.ForcusMenu(2);
    }

    private void UpdateOnFocusExit()
    {
        CheckForcusAction(() => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_CHAPTER_RANKING), null);
        CheckSelectAction(() => m_StateMachine.Goto(E_RANKING_MENU_STATE.SELECT_EXIT));
    }

    private void EndOnFocusExit()
    {

    }

    #endregion

    #region Select Exit

    private void StartOnSelectExit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
                Application.Quit();
        #endif
    }

    private void UpdateOnSelectExit()
    {

    }

    private void EndOnSelectExit()
    {

    }

#endregion
}

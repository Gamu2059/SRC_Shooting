#pragma warning disable 0649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterMenuManager : ControllableMonoBehavior
{
    #region Define

    private enum E_CHAPTER_MENU_STATE
    {
        FORCUS_CHAP_0,
        SELECT_CHAP_0,
        FORCUS_CHAP_1,
        SELECT_CHAP_1,
        FORCUS_CHAP_2,
        SELECT_CHAP_2,
        FORCUS_CHAP_3,
        SELECT_CHAP_3,
        FORCUS_CHAP_4,
        SELECT_CHAP_4,
        FORCUS_CHAP_5,
        SELECT_CHAP_5,
        FORCUS_CHAP_6,
        SELECT_CHAP_6,
    }

    private class StateCycle : StateCycleBase<ChapterMenuManager, E_CHAPTER_MENU_STATE> { }

    private class ChapterMenuManagerState : State<E_CHAPTER_MENU_STATE, ChapterMenuManager>
    {
        public ChapterMenuManagerState(E_CHAPTER_MENU_STATE state, ChapterMenuManager target) : base(state, target) { }
        public ChapterMenuManagerState(E_CHAPTER_MENU_STATE state, ChapterMenuManager target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    [SerializeField]
    private ChapterMenuUIManager m_UiManager;

    [SerializeField]
    private PlaySoundParam m_CursorSe;

    [SerializeField]
    private PlaySoundParam m_OkSe;

    [SerializeField]
    private PlaySoundParam m_CancelSe;

    [SerializeField]
    private float m_WaitCursorTime;

    private TwoAxisInputManager InputManager;

    private StateMachine<E_CHAPTER_MENU_STATE, ChapterMenuManager> m_StateMachine;

    private bool m_EnableMove;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_CHAPTER_MENU_STATE, ChapterMenuManager>();

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.FORCUS_CHAP_0, this)
        { 
            m_OnStart = StartOnForcusChap0,
            m_OnUpdate = UpdateOnForcusChap0,
            m_OnEnd = EndOnForcusChap0,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.SELECT_CHAP_0, this)
        {
            m_OnStart = StartOnSelectChap0,
            m_OnUpdate = UpdateOnSelectChap0,
            m_OnEnd = EndOnSelectChap0,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.FORCUS_CHAP_1, this)
        {
            m_OnStart = StartOnForcusChap1,
            m_OnUpdate = UpdateOnForcusChap1,
            m_OnEnd = EndOnForcusChap1,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.SELECT_CHAP_1, this)
        {
            m_OnStart = StartOnSelectChap1,
            m_OnUpdate = UpdateOnSelectChap1,
            m_OnEnd = EndOnSelectChap1,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.FORCUS_CHAP_2, this)
        {
            m_OnStart = StartOnForcusChap2,
            m_OnUpdate = UpdateOnForcusChap2,
            m_OnEnd = EndOnForcusChap2,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.SELECT_CHAP_2, this)
        {
            m_OnStart = StartOnSelectChap2,
            m_OnUpdate = UpdateOnSelectChap2,
            m_OnEnd = EndOnSelectChap2,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.FORCUS_CHAP_3, this)
        {
            m_OnStart = StartOnForcusChap3,
            m_OnUpdate = UpdateOnForcusChap3,
            m_OnEnd = EndOnForcusChap3,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.SELECT_CHAP_3, this)
        {
            m_OnStart = StartOnSelectChap3,
            m_OnUpdate = UpdateOnSelectChap3,
            m_OnEnd = EndOnSelectChap3,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.FORCUS_CHAP_4, this)
        {
            m_OnStart = StartOnForcusChap4,
            m_OnUpdate = UpdateOnForcusChap4,
            m_OnEnd = EndOnForcusChap4,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.SELECT_CHAP_4, this)
        {
            m_OnStart = StartOnSelectChap4,
            m_OnUpdate = UpdateOnSelectChap4,
            m_OnEnd = EndOnSelectChap4,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.FORCUS_CHAP_5, this)
        {
            m_OnStart = StartOnForcusChap5,
            m_OnUpdate = UpdateOnForcusChap5,
            m_OnEnd = EndOnForcusChap5,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.SELECT_CHAP_5, this)
        {
            m_OnStart = StartOnSelectChap5,
            m_OnUpdate = UpdateOnSelectChap5,
            m_OnEnd = EndOnSelectChap5,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.FORCUS_CHAP_6, this)
        {
            m_OnStart = StartOnForcusChap6,
            m_OnUpdate = UpdateOnForcusChap6,
            m_OnEnd = EndOnForcusChap6,
        });

        m_StateMachine.AddState(new ChapterMenuManagerState(E_CHAPTER_MENU_STATE.SELECT_CHAP_6, this)
        {
            m_OnStart = StartOnSelectChap6,
            m_OnUpdate = UpdateOnSelectChap6,
            m_OnEnd = EndOnSelectChap6,
        });

        InputManager = new TwoAxisInputManager();
        InputManager.OnInitialize();

        m_UiManager.OnInitialize();
        m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_0);
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

    private void CheckForcusActionVertical(Action onMoveUp, Action onMoveDown)
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

    private void ExitScene()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                Application.Quit();
#endif
    }

    #region Forcus Chap0

    private void StartOnForcusChap0()
    {
        m_UiManager.ForcusMenu(0);
    }

    private void UpdateOnForcusChap0()
    {
        CheckForcusActionVertical(null, ()=>m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_1));
        CheckSelectAction(()=>m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_0));
        CheckCancelAction(() => ExitScene());
    }

    private void EndOnForcusChap0()
    {

    }

    #endregion

    #region Select Chap0

    private void StartOnSelectChap0()
    {
        Debug.Log("Chapter0が選択されました");
        m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_0);
    }

    private void UpdateOnSelectChap0()
    {

    }

    private void EndOnSelectChap0()
    {

    }

    #endregion

    #region Forcus Chap1

    private void StartOnForcusChap1()
    {
        m_UiManager.ForcusMenu(1);
    }

    private void UpdateOnForcusChap1()
    {
        CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_0), () => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_2));
        CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_1));
        CheckCancelAction(() => ExitScene());
    }

    private void EndOnForcusChap1()
    {

    }

    #endregion

    #region Select Chap1

    private void StartOnSelectChap1()
    {
        Debug.Log("Chapter1が選択されました");
        m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_1);
    }

    private void UpdateOnSelectChap1()
    {

    }

    private void EndOnSelectChap1()
    {

    }

    #endregion

    #region Forcus Chap2

    private void StartOnForcusChap2()
    {
        m_UiManager.ForcusMenu(2);
    }

    private void UpdateOnForcusChap2()
    {
        CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_1), () => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_3));
        CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_2));
        CheckCancelAction(() => ExitScene());
    }

    private void EndOnForcusChap2()
    {

    }

    #endregion

    #region Select Chap2

    private void StartOnSelectChap2()
    {
        Debug.Log("Chapter2が選択されました");
        m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_2);
    }

    private void UpdateOnSelectChap2()
    {

    }

    private void EndOnSelectChap2()
    {

    }

    #endregion

    #region Forcus Chap3

    private void StartOnForcusChap3()
    {
        m_UiManager.ForcusMenu(3);
    }

    private void UpdateOnForcusChap3()
    {
        CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_2), () => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_4));
        CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_3));
        CheckCancelAction(() => ExitScene());
    }

    private void EndOnForcusChap3()
    {

    }

    #endregion

    #region Select Chap3

    private void StartOnSelectChap3()
    {
        Debug.Log("Chapter3が選択されました");
        m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_3);
    }

    private void UpdateOnSelectChap3()
    {

    }

    private void EndOnSelectChap3()
    {

    }

    #endregion

    #region Forcus Chap4

    private void StartOnForcusChap4()
    {
        m_UiManager.ForcusMenu(4);
    }

    private void UpdateOnForcusChap4()
    {
        CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_3), () => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_5));
        CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_4));
        CheckCancelAction(() => ExitScene());
    }

    private void EndOnForcusChap4()
    {

    }

    #endregion

    #region Select Chap4

    private void StartOnSelectChap4()
    {
        Debug.Log("Chapter4が選択されました");
        m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_4);
    }

    private void UpdateOnSelectChap4()
    {

    }

    private void EndOnSelectChap4()
    {

    }

    #endregion

    #region Forcus Chap5

    private void StartOnForcusChap5()
    {
        m_UiManager.ForcusMenu(5);
    }

    private void UpdateOnForcusChap5()
    {
        CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_4), () => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_6));
        CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_5));
        CheckCancelAction(() => ExitScene());
    }

    private void EndOnForcusChap5()
    {

    }

    #endregion

    #region Select Chap5

    private void StartOnSelectChap5()
    {
        Debug.Log("Chapter5が選択されました");
        m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_5);
    }

    private void UpdateOnSelectChap5()
    {

    }

    private void EndOnSelectChap5()
    {

    }

    #endregion

    #region Forcus Chap6

    private void StartOnForcusChap6()
    {
        m_UiManager.ForcusMenu(6);
    }

    private void UpdateOnForcusChap6()
    {
        CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_5), null);
        CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_6));
        CheckCancelAction(() => ExitScene());
    }

    private void EndOnForcusChap6()
    {

    }

    #endregion

    #region Select Chap6

    private void StartOnSelectChap6()
    {
        Debug.Log("Chapter6が選択されました");
        m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_6);
    }

    private void UpdateOnSelectChap6()
    {

    }

    private void EndOnSelectChap6()
    {

    }

    #endregion
}

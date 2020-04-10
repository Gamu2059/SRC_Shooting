#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TitleManager : ControllableMonoBehavior
{
    #region Define

    private enum E_MENU_STATE
    {
        FORCUS_PLAY,
        SELECT_PLAY,
        FORCUS_HOWTO,
        SELECT_HOWTO,
        FORCUS_CREDIT,
        SELECT_CREDIT,
        FORCUS_EXIT,
        SELECT_EXIT,
    }

    private class StateCycle : StateCycleBase<TitleManager, E_MENU_STATE> { }

    private class InnerState : State<E_MENU_STATE, TitleManager>
    {
        public InnerState(E_MENU_STATE state, TitleManager target) : base(state, target) { }
        public InnerState(E_MENU_STATE state, TitleManager target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    [SerializeField]
    private TitleUiManager m_UiManager;

    [SerializeField]
    private Transform m_Overall;

    [SerializeField]
    private Transform m_MotherColony;

    [SerializeField]
    private float m_OverallRotateSpeed;

    [SerializeField]
    private float m_MotherColonyRotateSpeed;

    [SerializeField]
    private PlaySoundParam m_CursorSe;

    [SerializeField]
    private PlaySoundParam m_OkSe;

    [SerializeField]
    private PlaySoundParam m_CancelSe;

    [SerializeField]
    private PlaySoundParam m_StartSe;

    [SerializeField]
    private PlaySoundParam m_TitleBgm;

    [SerializeField]
    private float m_WaitCursorTime;

    private TitleInputManager InputManager;
    private StateMachine<E_MENU_STATE, TitleManager> m_StateMachine;

    private bool m_EnableMove;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_MENU_STATE, TitleManager>();

        m_StateMachine.AddState(new InnerState(E_MENU_STATE.FORCUS_PLAY, this)
        {
            m_OnStart = StartOnForcusPlay,
            m_OnUpdate = UpdateOnForcusPlay,
            m_OnEnd = EndOnForcusPlay,
        });

        m_StateMachine.AddState(new InnerState(E_MENU_STATE.FORCUS_HOWTO, this)
        {
            m_OnStart = StartOnForcusHowto,
            m_OnUpdate = UpdateOnForcusHowto,
            m_OnEnd = EndOnForcusHowto,
        });

        m_StateMachine.AddState(new InnerState(E_MENU_STATE.FORCUS_CREDIT, this)
        {
            m_OnStart = StartOnForcusCredit,
            m_OnUpdate = UpdateOnForcusCredit,
            m_OnEnd = EndOnForcusCredit,
        });

        m_StateMachine.AddState(new InnerState(E_MENU_STATE.FORCUS_EXIT, this)
        {
            m_OnStart = StartOnForcusExit,
            m_OnUpdate = UpdateOnForcusExit,
            m_OnEnd = EndOnForcusExit
        });

        m_StateMachine.AddState(new InnerState(E_MENU_STATE.SELECT_PLAY, this)
        {
            m_OnStart = StartOnSelectPlay,
        });

        m_StateMachine.AddState(new InnerState(E_MENU_STATE.SELECT_HOWTO, this)
        {
            m_OnStart = StartOnSelectHowto,
            m_OnUpdate = UpdateOnSelectHowto,
            m_OnEnd = EndOnSelectHowto
        });

        m_StateMachine.AddState(new InnerState(E_MENU_STATE.SELECT_CREDIT, this)
        {
            m_OnStart = StartOnSelectCredit,
            m_OnUpdate = UpdateOnSelectCredit,
            m_OnEnd = EndOnSelectCredit
        });

        m_StateMachine.AddState(new InnerState(E_MENU_STATE.SELECT_EXIT, this)
        {
            m_OnStart = StartOnSelectExit,
            m_OnUpdate = UpdateOnSelectExit,
        });

        InputManager = new TitleInputManager();
        InputManager.OnInitialize();
        m_UiManager.OnInitialize();

        m_StateMachine.Goto(E_MENU_STATE.FORCUS_PLAY);
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
        AudioManager.Instance.Play(m_TitleBgm);
        m_EnableMove = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        InputManager.OnUpdate();
        m_StateMachine?.OnUpdate();

        m_Overall.RotateAround(m_Overall.position, m_Overall.up, m_OverallRotateSpeed * Time.deltaTime);
        m_MotherColony.RotateAround(m_MotherColony.position, m_MotherColony.up, m_MotherColonyRotateSpeed * Time.deltaTime);
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

    private void CheckStartAction()
    {
        if (InputManager.Submit == E_INPUT_STATE.DOWN)
        {
            PlayStart();
            BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE1);
            InputManager.RemoveInput();
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

    private void PlayStart()
    {
        AudioManager.Instance.Play(m_StartSe);
    }

    #region Forcus Play

    private void StartOnForcusPlay()
    {
        m_UiManager.ForcusMenu(0);
    }

    private void UpdateOnForcusPlay()
    {
        CheckForcusAction(null, () => m_StateMachine.Goto(E_MENU_STATE.FORCUS_HOWTO));
        CheckStartAction();
    }

    private void EndOnForcusPlay()
    {
    }

    #endregion

    #region Select Play

    private void StartOnSelectPlay()
    {

    }

    #endregion

    #region Forcus Howto

    private void StartOnForcusHowto()
    {
        m_UiManager.ForcusMenu(1);
    }

    private void UpdateOnForcusHowto()
    {
        CheckForcusAction(
            () => m_StateMachine.Goto(E_MENU_STATE.FORCUS_PLAY),
            () => m_StateMachine.Goto(E_MENU_STATE.FORCUS_CREDIT));
        CheckSelectAction(() => m_StateMachine.Goto(E_MENU_STATE.SELECT_HOWTO));
    }

    private void EndOnForcusHowto()
    {
    }

    #endregion

    #region Select Howto

    private void StartOnSelectHowto()
    {
        m_UiManager.EnableHowto();
    }

    private void UpdateOnSelectHowto()
    {
        CheckCancelAction(() => m_StateMachine.Goto(E_MENU_STATE.FORCUS_HOWTO));
    }

    private void EndOnSelectHowto()
    {
        m_UiManager.DisableHowto();
    }

    #endregion

    #region Forcus Credit

    private void StartOnForcusCredit()
    {
        m_UiManager.ForcusMenu(2);
    }

    private void UpdateOnForcusCredit()
    {
        CheckForcusAction(
            () => m_StateMachine.Goto(E_MENU_STATE.FORCUS_HOWTO),
            () => m_StateMachine.Goto(E_MENU_STATE.FORCUS_EXIT));
        CheckSelectAction(() => m_StateMachine.Goto(E_MENU_STATE.SELECT_CREDIT));
    }

    private void EndOnForcusCredit()
    {
    }

    #endregion

    #region Select Credit

    private void StartOnSelectCredit()
    {
        m_UiManager.EnableCredit();
    }

    private void UpdateOnSelectCredit()
    {
        CheckCancelAction(() => m_StateMachine.Goto(E_MENU_STATE.FORCUS_CREDIT));
    }

    private void EndOnSelectCredit()
    {
        m_UiManager.DisableCredit();
    }

    #endregion

    #region Forcus Exit

    private void StartOnForcusExit()
    {
        m_UiManager.ForcusMenu(3);
    }

    private void UpdateOnForcusExit()
    {
        CheckForcusAction(() => m_StateMachine.Goto(E_MENU_STATE.FORCUS_CREDIT), null);
        CheckSelectAction(() => m_StateMachine.Goto(E_MENU_STATE.SELECT_EXIT));
    }

    private void EndOnForcusExit()
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

    #endregion
}

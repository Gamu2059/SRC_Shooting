#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RankingManager : ControllableMonoBehavior
{
    private enum E_RANKING_MENU_STATE
    {
        FORCUS_BOARD,
        SELECT_BOARD,
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

    private TwoAxisInputManager InputManager;
    private StateMachine<E_RANKING_MENU_STATE> m_StateMachine;

    private bool m_EnableMove;

    private int m_OutputTextIndexHorizontal;

    private int m_OutputTextIndexVertical;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_RANKING_MENU_STATE>();

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.FORCUS_BOARD)
        {
            m_OnStart = StartOnFocusBoard,
            m_OnUpdate = UpdateOnFocusBoard,
            m_OnEnd = EndOnFocusBoard,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.FORCUS_EXIT)
        {
            m_OnStart = StartOnFocusExit,
            m_OnUpdate = UpdateOnFocusExit,
            m_OnEnd = EndOnFocusExit,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.SELECT_BOARD) 
        {
            m_OnStart = StartOnSelectBoard,
            m_OnUpdate = UpdateOnSelectBoard,
            m_OnEnd = EndOnSelectBoard,
        });

        m_StateMachine.AddState(new State<E_RANKING_MENU_STATE>(E_RANKING_MENU_STATE.SELECT_EXIT) 
        { 
            m_OnStart = StartOnSelectExit,
            m_OnUpdate = UpdateOnSelectExit,
            m_OnEnd = EndOnSelectExit,
        });

        InputManager = new TwoAxisInputManager();
        InputManager.OnInitialize();
        m_UiManager.OnInitialize();
        m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_BOARD);

        m_OutputTextIndexHorizontal = 0;
        m_OutputTextIndexVertical = 0;
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

    private void CheckForcusActionHorizontal(Action onMoveRight, Action onMoveLeft)
    {
        if (!m_EnableMove)
        {
            return;
        }

        var h = InputManager.MoveDir.x;

        if(h > 0.8)
        {
            if(onMoveRight != null)
            {
                PlayCursor();
                WaitCursor();
                onMoveRight.Invoke();
            }
        }else if(h < -0.8)
        {
            if (onMoveLeft != null)
            {
                PlayCursor();
                WaitCursor();
                onMoveLeft.Invoke();
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

    #region Focus Board

    private void StartOnFocusBoard()
    {
        m_UiManager.ForcusMenu(0);
    }

    private void UpdateOnFocusBoard()
    {
        CheckForcusActionVertical(
            () =>
            {
                if (m_OutputTextIndexVertical - 1 < 0)
                {
                    return;
                }
                else
                {
                    m_OutputTextIndexVertical--;
                }
            },
            () =>
            {
                if (m_OutputTextIndexVertical + 1 > 3)
                {
                    return;
                }
                else
                {
                    m_OutputTextIndexVertical++;
                }
            });
        CheckForcusActionHorizontal(
            () =>
            {
                if (m_OutputTextIndexHorizontal + 1 > 7)
                {
                    return;
                }
                else
                {
                    m_OutputTextIndexHorizontal++;
                }
            },
            () =>
            {
                if (m_OutputTextIndexHorizontal - 1 < 0)
                {
                    return;
                }
                else
                {
                    m_OutputTextIndexHorizontal--;
                }
            });
        var idx = m_OutputTextIndexHorizontal + m_OutputTextIndexVertical * 8;
        if (idx < 32)
        {
            m_UiManager.SetRankingText(idx);
        }
        CheckCancelAction(() => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_EXIT));
    }

    private void EndOnFocusBoard()
    {
        m_OutputTextIndexVertical = 0;
    }

    #endregion

    #region Select Board

    private void StartOnSelectBoard()
    {
        
    }

    private void UpdateOnSelectBoard()
    {
        
    }

    private void EndOnSelectBoard()
    {
        
    }

    #endregion

    #region Focus Exit

    private void StartOnFocusExit()
    {
        m_UiManager.ForcusMenu(1);
    }

    private void UpdateOnFocusExit()
    {
        CheckCancelAction(() => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_BOARD));
        CheckSelectAction(() => m_StateMachine.Goto(E_RANKING_MENU_STATE.SELECT_EXIT));
        CheckForcusActionHorizontal(() => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_BOARD),() => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_BOARD));
        CheckForcusActionVertical(() => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_BOARD), () => m_StateMachine.Goto(E_RANKING_MENU_STATE.FORCUS_BOARD));
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

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ステートを持つボタンのコンポーネント
/// </summary>
[Serializable]
public class CommonUiStatedButton : ControllableMonoBehavior
{
    #region Define

    private enum E_STATE
    {
        DISABLE,
        ENABLE_DEFOCUS,
        ENABLE_FOCUS,
    }

    private class StateCycle : StateCycleBase<CommonUiStatedButton, E_STATE> { }

    private class InnerState : State<E_STATE, CommonUiStatedButton>
    {
        public InnerState(E_STATE state, CommonUiStatedButton target) : base(state, target) { }
        public InnerState(E_STATE state, CommonUiStatedButton target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    #region Field Inspector

    [SerializeField]
    private GameObject m_DisableButton;

    [SerializeField]
    private GameObject m_EnableDefocusButton;

    [SerializeField]
    private GameObject m_EnableFocusButton;

    #endregion

    #region Field

    private StateMachine<E_STATE, CommonUiStatedButton> m_StateMachine;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        DisableButton();
    }

    #endregion

    #region Disable State


    #endregion

    #region Enable Defocus State

    #endregion

    #region Enable Focus State

    #endregion

    private void RequestChangeState(E_STATE state)
    {
        m_StateMachine?.Goto(state);
    }

    public void DisableButton()
    {
        //if (m_StateMachine == null || m_StateMachine.CurrentState == null)
        //{
        //    return;
        //}

        //var currentState = m_StateMachine.CurrentState.Key;
        //if (currentState == E_STATE.DISABLE)
        //{
        //    return;
        //}

        //RequestChangeState(E_STATE.DISABLE);

        m_DisableButton?.SetActive(true);
        m_EnableDefocusButton?.SetActive(false);
        m_EnableFocusButton?.SetActive(false);
    }

    public void EnableButton()
    {
        //if (m_StateMachine == null || m_StateMachine.CurrentState == null)
        //{
        //    return;
        //}

        //var currentState = m_StateMachine.CurrentState.Key;
        //if (currentState == E_STATE.ENABLE_FOCUS || currentState == E_STATE.ENABLE_DEFOCUS)
        //{
        //    return;
        //}

        //RequestChangeState(E_STATE.ENABLE_DEFOCUS);
        m_DisableButton?.SetActive(false);
        m_EnableDefocusButton?.SetActive(true);
        m_EnableFocusButton?.SetActive(false);
    }

    public void DefocusButton()
    {
        //if (m_StateMachine == null || m_StateMachine.CurrentState == null)
        //{
        //    return;
        //}

        //var currentState = m_StateMachine.CurrentState.Key;
        //if (currentState == E_STATE.DISABLE || currentState == E_STATE.ENABLE_DEFOCUS)
        //{
        //    return;
        //}

        //RequestChangeState(E_STATE.ENABLE_DEFOCUS);
        m_DisableButton?.SetActive(false);
        m_EnableDefocusButton?.SetActive(true);
        m_EnableFocusButton?.SetActive(false);
    }

    public void FocusButton()
    {
        //if (m_StateMachine == null || m_StateMachine.CurrentState == null)
        //{
        //    return;
        //}

        //var currentState = m_StateMachine.CurrentState.Key;
        //if (currentState == E_STATE.DISABLE || currentState == E_STATE.ENABLE_FOCUS)
        //{
        //    return;
        //}

        //RequestChangeState(E_STATE.ENABLE_FOCUS);
        m_DisableButton?.SetActive(false);
        m_EnableDefocusButton?.SetActive(false);
        m_EnableFocusButton?.SetActive(true);
    }
}

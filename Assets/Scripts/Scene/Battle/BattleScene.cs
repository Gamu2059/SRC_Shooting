using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// BattleSceneでは、直接扱うマネージャはBattleManagerだけということにする。
/// </summary>
public class BattleScene : BaseScene
{
    private enum E_STATE
    {
        NONE,
        BEFORE_SHOW,
        AFTER_SHOW,
        BEFORE_HIDE,
        AFTER_HIDE,
    }

    private E_STATE m_State;
    private BattleManager m_BattleManager;
    private Action m_OnBeforeShowComplete;
    private Action m_OnAfterShowComplete;
    private Action m_OnBeforeHideComplete;
    private Action m_OnAfterHideComplete;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_State = E_STATE.NONE;
        m_OnBeforeShowComplete = null;
        m_OnBeforeHideComplete = null;

        m_BattleManager = GetManager<BattleManager>();
    }

    public override void OnBeforeShow( Action onComplete )
	{
		OnInitializeManagers();
        m_State = E_STATE.BEFORE_SHOW;
        m_OnBeforeShowComplete = onComplete;
        m_BattleManager.OnBeforeShow();
	}

    public override void OnAfterShow(Action onComplete)
    {
        m_State = E_STATE.AFTER_SHOW;
        m_OnAfterShowComplete = onComplete;
        m_BattleManager.OnAfterShow();
    }

    public override void OnBeforeHide(Action onComplete)
    {
        m_State = E_STATE.BEFORE_HIDE;
        m_OnBeforeHideComplete = onComplete;
        m_BattleManager.OnBeforeHide();
    }

    public override void OnAfterHide( Action onComplete )
	{
        m_State = E_STATE.AFTER_HIDE;
        m_OnAfterHideComplete = onComplete;
        m_BattleManager.OnAfterHide();
		OnFinalizeManagers();
	}

    private void Update()
    {
        switch(m_State)
        {
            case E_STATE.BEFORE_SHOW:
                m_BattleManager.OnUpdate();
                if (m_BattleManager.IsReadyBeforeShow)
                {
                    m_OnBeforeShowComplete?.Invoke();
                }
                break;
            case E_STATE.AFTER_SHOW:
                //m_BattleManager.OnUpdate();
                m_OnAfterShowComplete?.Invoke();
                break;
            case E_STATE.BEFORE_HIDE:
                //m_BattleManager.OnUpdate();
                m_OnBeforeHideComplete?.Invoke();
                break;
            case E_STATE.AFTER_HIDE:
                m_OnAfterHideComplete?.Invoke();
                break;
            case E_STATE.NONE:
                break;
        }
    }
}

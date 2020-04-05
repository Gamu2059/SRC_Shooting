#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキング開始と終了の時に表示する演出を制御する。
/// </summary>
public class HackInOutController : ControllableMonoBehavior
{
    [SerializeField]
    private Animator m_Animator;

    #region Opend Callback

    public Action ChangeToHackingModeAction;
    public Action ChangeToRealModeAction;

    #endregion

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        gameObject.SetActive(false);
    }

    public override void OnFinalize()
    {
        ChangeToRealModeAction = null;
        ChangeToHackingModeAction = null;
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
        gameObject.SetActive(true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_Animator.Update(Time.deltaTime);
    }

    public void OnEnd()
    {
        gameObject.SetActive(false);
    }

    public void ChangeToHackingMode()
    {
        ChangeToHackingModeAction?.Invoke();
    }

    public void ChangeToRealMode()
    {
        ChangeToRealModeAction?.Invoke();
    }
}

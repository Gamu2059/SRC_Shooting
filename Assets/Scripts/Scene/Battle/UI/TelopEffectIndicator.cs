#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ゲーム中のテロップエフェクトを制御する。
/// </summary>
public class TelopEffectIndicator : ControllableMonoBehavior
{
    [System.Serializable]
    private enum E_END_TYPE
    {
        NONE,
        FLASH,
        EXIT,
        IMMEDIATE,
    }

    private enum E_PHASE
    {
        BEGIN,
        WAIT,
        EXIT,
    }

    private const string BEGIN = "begin";
    private const string END_EXIT = "end_exit";
    private const string END_FLASH = "end_flash";
    private const string END_NONE = "end_none";

    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private Text m_SubText;

    [Header("Const Parameter")]

    [SerializeField]
    private float m_WaitDuration;

    [SerializeField]
    private E_END_TYPE m_EndType;

    private E_PHASE m_Phase;

    private bool m_IsPlaying;
    private Timer m_WaitTimer;

    private Action m_OnEnd;

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_Animator.enabled = false;
        Stop();
    }

    public override void OnFinalize()
    {
        m_WaitTimer?.DestroyTimer();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!m_IsPlaying)
        {
            return;
        }

        m_Animator.Update(Time.deltaTime);
        switch (m_Phase)
        {
            case E_PHASE.BEGIN:
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    m_Phase = E_PHASE.WAIT;
                }
                break;
            case E_PHASE.WAIT:
                m_WaitTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, m_WaitDuration);
                m_WaitTimer.SetTimeoutCallBack(() =>
                {
                    switch (m_EndType)
                    {
                        case E_END_TYPE.NONE:
                            m_Animator.Play(END_NONE);
                            break;
                        case E_END_TYPE.EXIT:
                            m_Animator.Play(END_EXIT);
                            break;
                        case E_END_TYPE.FLASH:
                            m_Animator.Play(END_FLASH);
                            break;
                        case E_END_TYPE.IMMEDIATE:
                            Stop();
                            break;
                    }
                    m_Phase = E_PHASE.EXIT;
                    m_WaitTimer = null;
                });
                BattleRealTimerManager.Instance.RegistTimer(m_WaitTimer);
                break;
            case E_PHASE.EXIT:
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    Stop();
                    m_OnEnd?.Invoke();
                }
                break;
        }
    }

    public void Play(string subText = null, bool playForce = false, Action onEnd = null)
    {
        if (m_IsPlaying && !playForce)
        {
            return;
        }

        m_IsPlaying = true;
        m_OnEnd = onEnd;
        m_SubText.text = subText;
        gameObject.SetActive(true);
        m_Animator.Play(BEGIN);
        m_Phase = E_PHASE.BEGIN;
    }

    public void Stop()
    {
        m_IsPlaying = false;
        gameObject.SetActive(false);
    }
}

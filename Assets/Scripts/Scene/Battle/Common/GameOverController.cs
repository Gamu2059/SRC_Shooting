#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameOverController : ControllableMonoBehavior
{
    private const string GAME_OVER = "game_over";
    private const string END_GAME_OVER = "end_game_over";

    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private TextTypingAnimator m_TextTypingAnimator;

    [SerializeField]
    private CustomImageEffect m_CustomImageEffect;

    [SerializeField]
    private PlaySoundParam m_EndSe;

    private bool m_PlayTextAnimation;
    private bool m_PlayEndAnimation;

    public Action EndAction { get; set; }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_TextTypingAnimator.OnInitialize();
        m_CustomImageEffect.OnInitialize();

        m_PlayTextAnimation = false;
        m_PlayEndAnimation = false;
    }

    public override void OnFinalize()
    {
        m_CustomImageEffect.OnFinalize();
        m_TextTypingAnimator.OnFinalize();

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_TextTypingAnimator.OnUpdate();
        m_CustomImageEffect.OnUpdate();

        if (Input.anyKeyDown && !m_PlayEndAnimation)
        {
            PlayGameOverEnd();
        }
    }

    #endregion

    public void PlayGameOver()
    {
        gameObject.SetActive(true);
        m_Animator.Play(GAME_OVER, 0);
    }

    public void PlayTextAnimation()
    {
        m_PlayTextAnimation = true;
        m_TextTypingAnimator.StartAnimation();
    }

    public void PlayGameOverEnd()
    {
        if (!m_PlayTextAnimation || m_PlayEndAnimation)
        {
            return;
        }

        m_PlayEndAnimation = true;
        m_Animator.Play(END_GAME_OVER, 0);
        AudioManager.Instance.Play(m_EndSe);
    }

    public void EndGameOver()
    {
        if (!m_PlayEndAnimation)
        {
            return;
        }

        EndAction?.Invoke();
    }
}

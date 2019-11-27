using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : ControllableMonoBehavior
{
    private const string GAME_OVER = "game_over";

    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private TextTypingAnimator m_TextTypingAnimator;

    [SerializeField]
    private CustomImageEffect m_CustomImageEffect;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_TextTypingAnimator.OnInitialize();
        m_CustomImageEffect.OnInitialize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_TextTypingAnimator.OnUpdate();
        m_CustomImageEffect.OnUpdate();
    }

    #endregion

    public void PlayGameOver()
    {
        gameObject.SetActive(true);
        m_Animator.Play(GAME_OVER, 0);
    }

    public void PlayTextAnimation()
    {
        m_TextTypingAnimator.StartAnimation();
    }

    public void EndGameOver()
    {
        BattleManager.Instance.RequestChangeState(E_BATTLE_STATE.END);
    }
}

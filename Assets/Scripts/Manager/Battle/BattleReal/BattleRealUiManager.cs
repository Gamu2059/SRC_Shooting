#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRealUiManager : ControllableMonoBehavior
{

    [SerializeField]
    private CanvasGroup m_CanvasGroup;

    [Header("UI")]

    [SerializeField]
    private ScoreIndicator m_BestScoreIndicator;

    [SerializeField]
    private ScoreIndicator m_ScoreIndicator;


    [Header("ゲーム終了時のやつ")]

    [SerializeField]
    private GameObject m_GameOver;

    [SerializeField]
    private GameObject m_GameClear;

    protected override void OnAwake()
    {
        base.OnAwake();
        SetEnableGameClear(false);
        SetEnableGameClear(false);
    }

    public void SetAlpha(float normalizedAlpha)
    {
        m_CanvasGroup.alpha = normalizedAlpha;
    }

    public void SetEnableGameOver(bool isEnable)
    {
        m_GameOver.SetActive(isEnable);
    }

    public void SetEnableGameClear(bool isEnable)
    {
        m_GameClear.SetActive(isEnable);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リアルモードで表示されるUIの管理を行うマネージャ
/// </summary>
public class BattleRealUiManager : ControllableMonoBehavior
{
    public static BattleRealUiManager Instance => BattleRealManager.Instance.BattleRealUiManager;

    public BattleRealHpIndicatorManager BattleRealHpIndicatorManager{get; private set;}

    [SerializeField]
    private CanvasGroup m_CanvasGroup;

    [Header("Center Group")]
    [SerializeField]
    private GameObject m_MainView;

    [Header("RightGroup")]
    [SerializeField]
    private GameObject m_FpsIndicator;

    [SerializeField]
    private GameObject m_BackGround;

    [SerializeField]
    private GameObject m_Character;

    [SerializeField]
    private GameObject m_EnemyName;

    [SerializeField]
    private GameObject m_ScoreLabel;

    [SerializeField]
    private GameObject m_ScoreIndicator;

    [SerializeField]
    private GameObject m_BestScoreLabel;

    [SerializeField]
    private GameObject m_BestScoreIndicator;

    [Header("Entire Field")]
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

    public void SetEnableFpsIndicator(bool isEnable){
        m_FpsIndicator.SetActive(isEnable);
    }

    public void SetEnableBackGround(bool isEnable){
        m_BackGround.SetActive(isEnable);
    }

    public void SetEnableCharactor(bool isEnable){
        m_Character.SetActive(isEnable);
    }

    public void SetEnableEnemyName(bool isEnable){
        m_EnemyName.SetActive(isEnable);
    }

    public void SetEnableScoreLabel(bool isEnable){
        m_ScoreLabel.SetActive(isEnable);
    }

    public void SetEnableScoreIndicator(bool isEnable){
        m_ScoreIndicator.SetActive(isEnable);
    }

    public void SetEnableBestScoreLabel(bool isEnable){
        m_BestScoreLabel.SetActive(isEnable);
    }

    public void SetEnableBestScoreIndicator(bool isEnable){
        m_BestScoreIndicator.SetActive(isEnable);
    }

    public void SetEnableRealModeUI(bool isEnable){
        SetEnableFpsIndicator(isEnable);
        SetEnableBackGround(isEnable);
        SetEnableCharactor(isEnable);
        SetEnableEnemyName(isEnable);
        SetEnableScoreLabel(isEnable);
        SetEnableScoreIndicator(isEnable);
        SetEnableBestScoreLabel(isEnable);
        SetEnableBestScoreIndicator(isEnable);
    }
}
#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRealUiManager : ControllableMonoBehavior
{

    [SerializeField]
    private CanvasGroup m_CanvasGroup;

    [Header("Indicator")]

    [SerializeField]
    private Text m_ModeIndicator;

    [SerializeField]
    private Text m_StageIndicator;

    [SerializeField]
    private ScoreIndicator m_BestScoreIndicator;

    [SerializeField]
    private ScoreIndicator m_ScoreIndicator;

    [SerializeField]
    private IconCountIndicator m_LifeIndicator;

    [SerializeField]
    private IconCountIndicator m_LevelIcon;

    [SerializeField]
    private IconGageIndicator m_LevelGage;

    [SerializeField]
    private IconCountIndicator m_EnergyIcon;

    [SerializeField]
    private IconGageIndicator m_EnergyGage;

    [SerializeField]
    private WeaponIndicator m_WeaponIndicator;
    [SerializeField]
    private IconGageIndicator m_BossHpGage;
    [SerializeField]
    private IconGageIndicator m_BossDownGage;

    [SerializeField]
    private GameObject m_BossUI;


    [Header("ゲーム終了時のやつ")]

    [SerializeField]
    private GameObject m_GameOver;

    [SerializeField]
    private GameObject m_GameClear;


    #region Game Cycle

    protected override void OnAwake()
    {
        base.OnAwake();
        SetEnableGameClear(false);
        SetEnableGameClear(false);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        var battleData = DataManager.Instance.BattleData;

        m_ModeIndicator.text = battleData.GameMode.ToString();
        m_StageIndicator.text = battleData.Stage.ToString().Replace("_", " ");

        m_BestScoreIndicator.OnInitialize();
        m_ScoreIndicator.OnInitialize();
        m_LifeIndicator.OnInitialize();
        m_LevelIcon.OnInitialize();
        m_LevelGage.OnInitialize();
        m_EnergyIcon.OnInitialize();
        m_EnergyGage.OnInitialize();
        m_WeaponIndicator.OnInitialize();
        m_BossHpGage.OnInitialize();
        m_BossDownGage.OnInitialize();
        SetEnableBossUI(false);
    }

    public override void OnFinalize()
    {
        m_BossDownGage.OnFinalize();
        m_BossHpGage.OnFinalize();
        m_WeaponIndicator.OnFinalize();
        m_EnergyGage.OnFinalize();
        m_EnergyIcon.OnFinalize();
        m_LevelGage.OnFinalize();
        m_LevelIcon.OnFinalize();
        m_LifeIndicator.OnFinalize();
        m_ScoreIndicator.OnFinalize();
        m_BestScoreIndicator.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_BestScoreIndicator.OnUpdate();
        m_ScoreIndicator.OnUpdate();
        m_LifeIndicator.OnUpdate();
        m_LevelIcon.OnUpdate();
        m_LevelGage.OnUpdate();
        m_EnergyIcon.OnUpdate();
        m_EnergyGage.OnUpdate();
        m_WeaponIndicator.OnUpdate();
        m_BossHpGage.OnUpdate();
        m_BossDownGage.OnUpdate();
        
    }

    #endregion

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

    public void SetEnableBossUI(bool isEnable){
        m_BossUI.SetActive(isEnable);
    }
}

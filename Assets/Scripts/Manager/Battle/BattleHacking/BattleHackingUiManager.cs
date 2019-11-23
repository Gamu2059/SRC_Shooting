#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHackingUiManager : ControllableMonoBehavior
{
    #region Inspector Field

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
    private GridGageIndicator m_TimeIndicator;

    [SerializeField]
    private GridGageIndicator m_BonusTimeIndicator;

    [SerializeField]
    private GridGageIndicator m_BossHpIndicator;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        var battleData = DataManager.Instance.BattleData;

        m_ModeIndicator.text = battleData.GameMode.ToString();
        m_StageIndicator.text = battleData.Stage.ToString().Replace("_", " ");

        m_BestScoreIndicator.OnInitialize();
        m_ScoreIndicator.OnInitialize();
        m_TimeIndicator.OnInitialize();
        m_BonusTimeIndicator.OnInitialize();
        m_BossHpIndicator.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_BossHpIndicator.OnFinalize();
        m_BonusTimeIndicator.OnFinalize();
        m_TimeIndicator.OnFinalize();
        m_ScoreIndicator.OnFinalize();
        m_BestScoreIndicator.OnFinalize();

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_BestScoreIndicator.OnUpdate();
        m_ScoreIndicator.OnUpdate();
        m_TimeIndicator.OnUpdate();
        m_BonusTimeIndicator.OnUpdate();
        m_BossHpIndicator.OnUpdate();
    }

    #endregion

    public void SetAlpha(float normalizedAlpha)
    {
        m_CanvasGroup.alpha = normalizedAlpha;
    }
}

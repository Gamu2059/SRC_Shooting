#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHackingUiManager : SingletonMonoBehavior<BattleHackingUiManager>
{
    private const string TO_HACKING = "battle_hacking_ui_to_hacking";
    private const string TO_REAL = "battle_hacking_ui_to_real";

    #region Inspector Field

    [SerializeField]
    private CanvasGroup m_CanvasGroup;

    [Header("Center View")]

    [SerializeField]
    private HackingGridHoleEffect m_GridHoleEffect;
    public HackingGridHoleEffect GridHoleEffect => m_GridHoleEffect;

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

    [Header("Animator")]

    [SerializeField]
    private Animator m_MainViewAnimator;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        var battleData = DataManager.Instance.BattleData;

        m_ModeIndicator.text = battleData.GameMode.ToString();
        m_StageIndicator.text = battleData.Chapter.ToString().Replace("_", " ");

        m_GridHoleEffect.OnInitialize();
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
        m_GridHoleEffect.OnFinalize();

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_GridHoleEffect.OnUpdate();
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

    public void PlayToHacking()
    {
        m_MainViewAnimator.Play(TO_HACKING, 0);
    }

    public void PlayToReal()
    {
        m_MainViewAnimator.Play(TO_REAL, 0);
    }
}

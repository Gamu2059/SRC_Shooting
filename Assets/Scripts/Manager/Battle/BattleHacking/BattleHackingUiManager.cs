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
    private Text m_ChapterIndicator;

    [SerializeField]
    private Text m_DifficultyIndicator;

    [SerializeField]
    private TextValueIndicator m_BestScoreIndicator;

    [SerializeField]
    private TextValueIndicator m_ScoreIndicator;

    [SerializeField]
    private BattleHackingBossUI m_BossUi;

    [Header("Animator")]

    [SerializeField]
    private Animator m_MainViewAnimator;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_ChapterIndicator.text = DataManager.Instance.GetChapterString();
        m_DifficultyIndicator.text = DataManager.Instance.Difficulty.ToString();

        m_GridHoleEffect.OnInitialize();
        m_BestScoreIndicator.OnInitialize();
        m_ScoreIndicator.OnInitialize();
        m_BossUi.OnInitialize();

        DisableAllBossUI();
    }

    public override void OnFinalize()
    {
        m_BossUi.OnFinalize();
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
        m_BossUi.OnUpdate();
    }

    #endregion

    public void SetAlpha(float normalizedAlpha)
    {
        m_CanvasGroup.alpha = normalizedAlpha;
    }

    /// <summary>
    /// ボスUIを有効にする。
    /// </summary>
    public void EnableBossUI(BattleHackingBoss boss)
    {
        if (boss == null)
        {
            return;
        }

        if (m_BossUi != null && m_BossUi.ReferencedBoss == null )
        {
            m_BossUi.EnableBossUI(boss);
        }
    }

    public void DisableAllBossUI()
    {
        m_BossUi?.DisableBossUI();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using System;

/// <summary>
/// チャプターリザルト画面を管理するクラス
/// </summary>
public class ResultIndicator : ControllableMonoBehavior
{
    #region Field Inspector

    [SerializeField]
    private CanvasGroup m_ResultCanvasGroup;

    [SerializeField]
    private ResultItemIndicator[] m_BonusItems;

    [SerializeField]
    private ResultItemIndicator m_TotalScore;

    [SerializeField]
    private Text m_RankLabel;

    [SerializeField]
    private Text m_Rank;

    [SerializeField]
    private float m_ShowItemDuration;


    [SerializeField]
    private ResultItemIndicator m_ScoreIndicator;

    [SerializeField]
    private ResultItemIndicator m_LifeBonusIndicator;

    [SerializeField]
    private ResultItemIndicator m_PerfectHackingIndicator;

    [SerializeField]
    private ResultItemIndicator m_TotalScoreIndicator;


    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_ScoreIndicator.OnInitialize();
        m_LifeBonusIndicator.OnInitialize();
        m_PerfectHackingIndicator.OnInitialize();
        m_TotalScoreIndicator.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_TotalScoreIndicator.OnFinalize();
        m_PerfectHackingIndicator.OnFinalize();
        m_LifeBonusIndicator.OnFinalize();
        m_ScoreIndicator.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_ScoreIndicator.OnUpdate();
        m_LifeBonusIndicator.OnUpdate();
        m_PerfectHackingIndicator.OnUpdate();
        m_TotalScoreIndicator.OnUpdate();
    }

    #endregion

    public void PlayResult()
    {
    }

    /// <summary>
    /// リザルトのシーケンスを開始する
    /// </summary>
    public void StartResultSequence()
    {
    }

    private IEnumerator ResultSequence()
    {
        foreach (var i in m_BonusItems)
        {
            i.InitValue();
        }

        var seq = DOTween.Sequence();
        seq.Append(m_ResultCanvasGroup.DOFade(1, 0.5f));
        foreach (var i in m_BonusItems)
        {
            seq.Append(i.ShowItemSequence(m_ShowItemDuration));
        }
        seq.Append(m_TotalScore.ShowItemSequence(m_ShowItemDuration));

        // スコア表示
        var scoreType = E_RESULT_SCORE_TYPE.SCORE;
        foreach (var i in m_BonusItems)
        {
            scoreType |= i.ItemType;
            i.DramUpItem(0.5f);
            m_TotalScore.DramUpTotalScore(scoreType, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }

        // ランクラベル表示
        m_RankLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        // ランク表示
        m_Rank.text = "SS";
        m_Rank.gameObject.SetActive(true);
    }

    public void HideResult()
    {

    }
}

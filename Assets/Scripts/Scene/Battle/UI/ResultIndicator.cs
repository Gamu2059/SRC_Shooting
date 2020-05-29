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

    [Header("Component")]

    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private List<ResultItemIndicator> m_BonusItems;

    [SerializeField]
    private ResultItemIndicator m_TotalScore;

    [SerializeField]
    private Text m_RankLabel;

    [SerializeField]
    private Text m_Rank;

    [SerializeField]
    private GameObject m_BackObj;

    [SerializeField]
    private GameObject m_GroupObj;

    [Header("Parameter")]

    [SerializeField]
    private string m_ShowAnimationName;

    [SerializeField]
    private string m_HideAnimationName;

    [SerializeField]
    private float m_ShowItemDuration;

    [SerializeField]
    private float m_ShowItemWaitTime;

    [SerializeField]
    private float m_DramUpItemDuration;

    [SerializeField]
    private float m_DramUpItemWaitTime;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_BonusItems.ForEach(i => i.OnInitialize());
        m_TotalScore.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_TotalScore.OnFinalize();
        m_BonusItems.ForEach(i => i.OnFinalize());

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_BonusItems.ForEach(i => i.OnUpdate());
        m_TotalScore.OnUpdate();
    }

    #endregion

    /// <summary>
    /// リザルトを表示する
    /// </summary>
    public bool ShowResult(Action onComplete = null)
    {
        ResultSequence(onComplete);
        return false;
    }

    private void ResultSequence(Action onComplete = null)
    {
        IDisposable disposable = null;
        disposable = Observable.Concat(
            StartSequenceProcessObservable(),
            AnimationObservable(m_ShowAnimationName),
            ShowScoresObservable(m_ShowItemDuration, m_ShowItemWaitTime),
            DramUpScoresObservable(),
            Observable.Timer(TimeSpan.FromSeconds(1f)).AsUnitObservable(),
            ShowRankLabelObservable(1f),
            ShowRankObservable(0),
            WaitInputButtonObservable(),
            AnimationObservable(m_HideAnimationName),
            EndSequenceProcessObservable()
            ).Last().Subscribe(_ => { disposable.Dispose(); onComplete?.Invoke(); });
    }

    private IObservable<Unit> StartSequenceProcessObservable()
    {
        return Observable.Empty<Unit>().DoOnCompleted(() =>
        {
            foreach (var i in m_BonusItems)
            {
                i.PrepareShowItemSequence();
            }
            m_TotalScore.PrepareShowItemSequence();

            m_RankLabel.gameObject.SetActive(false);
            m_Rank.gameObject.SetActive(false);
            m_BackObj.SetActive(true);
            m_GroupObj.SetActive(true);
        });
    }

    private IObservable<Unit> EndSequenceProcessObservable()
    {
        return Observable.Empty<Unit>().DoOnCompleted(() =>
        {
            m_GroupObj.SetActive(false);
            m_BackObj.SetActive(false);
        });
    }

    /// <summary>
    /// アニメーションを行う
    /// </summary>
    private IObservable<Unit> AnimationObservable(string animationName)
    {
        return Observable.Defer(() =>
        {
            m_Animator.Play(animationName);
            return Observable.Concat(
                m_Animator
                    .ObserveEveryValueChanged(x => x.GetCurrentAnimatorStateInfo(0).normalizedTime)
                    .Where(x => x <= 0.01f)
                    .First()
                    .AsUnitObservable(),
                m_Animator
                    .ObserveEveryValueChanged(x => x.GetCurrentAnimatorStateInfo(0).normalizedTime)
                    .Where(x => x >= 1)
                    .First()
                    .AsUnitObservable()
                );
        });
    }

    /// <summary>
    /// ボーナススコアやトータルスコアを順次表示する
    /// </summary>
    private IObservable<Unit> ShowScoresObservable(float showItemDuration, float waitTime)
    {
        return Observable.Defer(() =>
        {
            var obs = new List<IObservable<Unit>>();
            foreach (var i in m_BonusItems)
            {
                var bonusScoreObservable = Observable.Defer(() =>
                {
                    i.ShowItemSequence(showItemDuration).Play();
                    return Observable.Timer(TimeSpan.FromSeconds(waitTime)).AsUnitObservable();
                });
                obs.Add(bonusScoreObservable);
            }

            var totalScoreObservable = Observable.Defer(() =>
            {
                m_TotalScore.ShowItemSequence(showItemDuration).Play();
                return Observable.Timer(TimeSpan.FromSeconds(waitTime)).AsUnitObservable();
            });
            obs.Add(totalScoreObservable);

            return Observable.Concat(obs);
        });
    }

    /// <summary>
    /// スコアを順次開示する
    /// </summary>
    private IObservable<Unit> DramUpScoresObservable()
    {
        return Observable.Defer(() =>
        {
            var obs = new List<IObservable<Unit>>();
            E_ACHIEVEMENT_TYPE scoreType = default;
            foreach (var i in m_BonusItems)
            {
                if (!i.IsValidValue())
                {
                    continue;
                }

                var bonusScoreObservable = Observable.Defer(() =>
                {
                    scoreType |= i.BonusType;
                    i.DramUpItem(m_DramUpItemDuration);
                    m_TotalScore.DramUpTotalScore(scoreType, m_DramUpItemDuration);
                    return Observable.Timer(TimeSpan.FromSeconds(m_DramUpItemWaitTime)).AsUnitObservable();
                });
                obs.Add(bonusScoreObservable);
            }

            return Observable.Concat(obs);
        });
    }

    /// <summary>
    /// ランクラベルを表示する
    /// </summary>
    private IObservable<Unit> ShowRankLabelObservable(float waitTime)
    {
        return Observable.Defer(() =>
        {
            m_RankLabel.gameObject.SetActive(true);
            return Observable.Timer(TimeSpan.FromSeconds(waitTime)).AsUnitObservable();
        });
    }

    /// <summary>
    /// ランクを表示する
    /// </summary>
    private IObservable<Unit> ShowRankObservable(float waitTime)
    {
        return Observable.Defer(() =>
        {
            var chapter = DataManager.Instance.Chapter;
            var resultData = DataManager.Instance.BattleResultData.GetChapterResult(chapter);
            m_Rank.text = resultData.Rank.ToString();
            m_Rank.gameObject.SetActive(true);
            return Observable.Timer(TimeSpan.FromSeconds(waitTime)).AsUnitObservable();
        }).CatchIgnore();
    }

    /// <summary>
    /// 入力を待ち受ける
    /// </summary>
    private IObservable<Unit> WaitInputButtonObservable()
    {
        return Observable.Defer(() =>
        {
            return Observable.Amb(
                RewiredInputManager.Instance.ObserveEveryValueChanged(x => x.UiSubmit).Where(x => x == E_REWIRED_INPUT_STATE.DOWN).AsUnitObservable(),
                RewiredInputManager.Instance.ObserveEveryValueChanged(x => x.Shot).Where(x => x == E_REWIRED_INPUT_STATE.DOWN).AsUnitObservable())
            .First();
        });
    }
}

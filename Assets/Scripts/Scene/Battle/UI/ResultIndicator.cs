using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultIndicator : ControllableMonoBehavior
{
    private const string RESULT_GROUP_ON = "result_group_on";

    [SerializeField]
    private Animator m_ResultGroupAnimator;

    [SerializeField]
    private ResultItemIndicator m_ScoreIndicator;

    [SerializeField]
    private ResultItemIndicator m_LifeBonusIndicator;

    [SerializeField]
    private ResultItemIndicator m_PerfectHackingIndicator;

    [SerializeField]
    private ResultItemIndicator m_TotalScoreIndicator;

    [SerializeField]
    private Text m_Text;

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

    public void PlayResult()
    {
        m_ResultGroupAnimator.Play(RESULT_GROUP_ON);
    }

    public void PlayScore()
    {
        m_ScoreIndicator.PlayResultItem();
    }

    public void PlayLifeBonus()
    {
        m_LifeBonusIndicator.PlayResultItem();
    }

    public void PlayPerfectHacking()
    {
        m_PerfectHackingIndicator.PlayResultItem();
    }

    public void PlayTotalScore()
    {
        m_TotalScoreIndicator.PlayResultItem();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResultItemIndicator : ControllableMonoBehavior
{
    [Serializable]
    private enum E_ITEM_TYPE
    {
        SCORE,
        LIFE_BONUS,
        PERFECT_HACKING,
        TOTAL_SCORE,
    }

    private const string RESULT_ITEM_ON = "result_item_on";

    [SerializeField]
    private E_ITEM_TYPE m_ItemType;

    [SerializeField]
    private Animator m_ResultItemAnimator;

    [SerializeField]
    private Text m_ValueText;

    [SerializeField]
    private float m_DramUpDuration;

    [SerializeField]
    private PlaySoundParam m_ResultSe;

    private bool m_IsAnimation;
    private bool m_IsDramUp;
    private float m_NowTime;
    private double m_TargetValue;

    private Timer m_Timer;

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_IsAnimation = false;
        m_IsDramUp = false;
    }

    public override void OnFinalize()
    {
        m_Timer?.DestroyTimer();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!m_IsDramUp)
        {
            return;
        }

        if (m_DramUpDuration <= 0)
        {
            m_ValueText.text = m_TargetValue.ToString("f0");
            m_IsDramUp = false;
            m_IsAnimation = false;
            return;
        }

        var normalizedTime = Mathf.Clamp01(m_NowTime / m_DramUpDuration);
        var value = m_TargetValue * normalizedTime;

        m_ValueText.text = value.ToString("f0");

        if (normalizedTime >= 1)
        {
            m_IsDramUp = false;
            m_IsAnimation = false;
            return;
        }

        m_NowTime += Time.unscaledDeltaTime;
    }

    public void PlayResultItem()
    {
        if (m_IsAnimation)
        {
            return;
        }

        m_IsAnimation = true;
        m_ResultItemAnimator.Play(RESULT_ITEM_ON);

        m_Timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 1f, PlayDramUp);
        TimerManager.Instance.RegistTimer(m_Timer);
        AudioManager.Instance.Play(m_ResultSe);
    }

    private void PlayDramUp()
    {
        if (m_IsDramUp)
        {
            return;
        }

        m_IsDramUp = true;
        m_NowTime = 0;
        m_TargetValue = GetValue();
    }

    private double GetValue()
    {
        //var data = DataManager.Instance.BattleResultData;
        //switch (m_ItemType)
        //{
        //    case E_ITEM_TYPE.SCORE:
        //        return data.Score;
        //    case E_ITEM_TYPE.LIFE_BONUS:
        //        return data.LifeBonusScore;
        //    case E_ITEM_TYPE.PERFECT_HACKING:
        //        return data.PerfectHackingBonusScore;
        //    case E_ITEM_TYPE.TOTAL_SCORE:
        //        return data.TotalScore;
        //}
        return 0;
    }
}

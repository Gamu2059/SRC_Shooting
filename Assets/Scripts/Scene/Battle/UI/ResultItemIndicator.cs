using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class ResultItemIndicator : ControllableMonoBehavior
{
    private readonly E_ACHIEVEMENT_TYPE[] m_BonusTypes = new E_ACHIEVEMENT_TYPE[]
    {
        E_ACHIEVEMENT_TYPE.LEVEL,
        E_ACHIEVEMENT_TYPE.MAX_CHAIN,
        E_ACHIEVEMENT_TYPE.BULLET_REMOVE,
        E_ACHIEVEMENT_TYPE.SECRET_ITEM,
        E_ACHIEVEMENT_TYPE.RESCUE,
    };

    #region Field Inspector

    [Header("Component")]

    [SerializeField]
    private CanvasGroup m_CanvasGroup;

    [SerializeField]
    private Text m_Label;

    [SerializeField]
    private Text m_CurrentValueText;

    [SerializeField]
    private Text m_TargetValueText;

    [SerializeField]
    private Text m_PercentageValueText;

    [SerializeField]
    private Text m_BonusValueText;

    [SerializeField]
    private PlaySoundParam m_ResultSe;

    [SerializeField]
    private Color m_LabelColor;

    [SerializeField]
    private Color m_NotAchievedPercentageColor;

    [SerializeField]
    private Color m_AchievedPercentageColor;

    [Header("Parameter")]

    [SerializeField]
    private bool m_IsTotalScore;

    [SerializeField]
    private E_ACHIEVEMENT_TYPE m_BonusType;
    public E_ACHIEVEMENT_TYPE BonusType => m_BonusType;

    #endregion

    #region Field

    private bool m_IsDramUp;
    private float m_NowTime;
    private ulong m_CurrentBonusValue;
    private ulong m_TargetBonusValue;
    private float m_DramUpDuration;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_IsDramUp = false;
        m_CurrentBonusValue = 0;
        m_TargetBonusValue = 0;
    }

    public override void OnFinalize()
    {
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
            m_BonusValueText.text = m_TargetBonusValue.ToString("f0");
            m_IsDramUp = false;
            return;
        }

        var normalizedTime = Mathf.Clamp01(m_NowTime / m_DramUpDuration);
        var value = (m_TargetBonusValue - m_CurrentBonusValue) * (double)normalizedTime + m_CurrentBonusValue;

        m_BonusValueText.text = value.ToString("f0");

        if (normalizedTime >= 1)
        {
            m_IsDramUp = false;
            return;
        }

        m_NowTime += Time.deltaTime;
    }

    #endregion

    public bool IsValidValue()
    {
        return IsValidValue(m_BonusType);
    }

    private bool IsValidValue(E_ACHIEVEMENT_TYPE type)
    {
        var battleData = DataManager.Instance.BattleData;
        if (battleData != null)
        {
            return battleData.IsAchieve(type);
        }

        return false;
    }

    private int GetCurrentValue(E_ACHIEVEMENT_TYPE type)
    {
        var battleData = DataManager.Instance.BattleData;
        if (battleData != null)
        {
            return battleData.GetAchievementCurrentValue(type);
        }

        return 0;
    }

    private int GetTargetValue(E_ACHIEVEMENT_TYPE type)
    {
        var battleData = DataManager.Instance.BattleData;
        if (battleData != null)
        {
            return battleData.GetAchievementTargetValue(type);
        }

        return 0;
    }

    private ulong GetBonusValue(E_ACHIEVEMENT_TYPE type)
    {
        var chapter = DataManager.Instance.Chapter;
        var data = DataManager.Instance.BattleResultData.GetChapterResult(chapter);
        if (data != null)
        {
            return data.GetBonusScore(type);
        }

        return 0;
    }

    private ulong GetScore()
    {
        var chapter = DataManager.Instance.Chapter;
        var data = DataManager.Instance.BattleResultData.GetChapterResult(chapter);
        if (data != null)
        {
            return data.Score;
        }

        return 0;
    }

    /// <summary>
    /// 項目の初期化
    /// </summary>
    public void PrepareShowItemSequence()
    {
        m_Label.color = m_LabelColor;
        if (m_IsTotalScore)
        {
            m_BonusValueText.color = m_AchievedPercentageColor;
            m_BonusValueText.text = GetScore().ToString();
        }
        else
        {
            var color = IsValidValue() ? m_AchievedPercentageColor : m_NotAchievedPercentageColor;
            m_PercentageValueText.color = color;
            m_BonusValueText.color = color;

            var currentValue = GetCurrentValue(m_BonusType);
            var targetValue = GetTargetValue(m_BonusType);
            m_CurrentValueText.text = currentValue.ToString();
            m_TargetValueText.text = targetValue.ToString();
            m_PercentageValueText.text = string.Format("{0}%", targetValue < 1 ? 0 : (currentValue * 100 / targetValue));
            m_BonusValueText.text = 0.ToString();
        }

        m_CanvasGroup.alpha = 0;
    }

    /// <summary>
    /// 項目の表示シーケンス
    /// </summary>
    public Sequence ShowItemSequence(float duration)
    {
        return DOTween.Sequence().Append(m_CanvasGroup.DOFade(1, duration));
    }

    public void DramUpItem(float duration)
    {
        AudioManager.Instance.Play(m_ResultSe);
        PlayDramUp(0, GetBonusValue(m_BonusType), duration);
    }

    public void DramUpTotalScore(E_ACHIEVEMENT_TYPE type, float duration)
    {
        ulong value = GetScore();
        foreach (var t in m_BonusTypes)
        {
            if ((type & t) == t)
            {
                value += GetBonusValue(t);
            }
        }

        m_CurrentValueText = m_TargetValueText;
        PlayDramUp(m_CurrentBonusValue, value, duration);
    }

    private void PlayDramUp(ulong currentValue, ulong targetValue, float duration)
    {
        if (m_IsDramUp)
        {
            return;
        }

        m_IsDramUp = true;
        m_NowTime = 0;
        m_DramUpDuration = duration;
        m_CurrentBonusValue = currentValue;
        m_TargetBonusValue = targetValue;
    }
}

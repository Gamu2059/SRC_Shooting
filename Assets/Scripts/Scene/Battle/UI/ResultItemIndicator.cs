using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class ResultItemIndicator : ControllableMonoBehavior
{
    private readonly E_RESULT_SCORE_TYPE[] m_BonusTypes = new E_RESULT_SCORE_TYPE[]
    {
        E_RESULT_SCORE_TYPE.SCORE,
        E_RESULT_SCORE_TYPE.LEVEL_BONUS,
        E_RESULT_SCORE_TYPE.MAX_CHAIN_BONUS,
        E_RESULT_SCORE_TYPE.BULLET_REMOVE_BONUS,
        E_RESULT_SCORE_TYPE.SECRET_ITEM_BONUS,
        E_RESULT_SCORE_TYPE.MAX_HACKING_CHAIN_BONUS,
    };

    #region Field Inspector

    [SerializeField]
    private E_RESULT_SCORE_TYPE m_ItemType;
    public E_RESULT_SCORE_TYPE ItemType => m_ItemType;

    [SerializeField]
    private Text m_Label;

    [SerializeField]
    private Text m_Value;

    [SerializeField]
    private PlaySoundParam m_ResultSe;

    [SerializeField]
    private Color m_LabelColor;

    [SerializeField]
    private Color m_ValueColor;

    #endregion

    #region Field

    private bool m_IsDramUp;
    private float m_NowTime;
    private ulong m_CurrentValue;
    private ulong m_TargetValue;
    private float m_DramUpDuration;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_IsDramUp = false;
        m_TargetValue = 0;
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
            m_Value.text = m_TargetValue.ToString("f0");
            m_IsDramUp = false;
            return;
        }

        var normalizedTime = Mathf.Clamp01(m_NowTime / m_DramUpDuration);
        var value = (m_TargetValue - m_CurrentValue) * (double)normalizedTime + m_CurrentValue;

        m_Value.text = value.ToString("f0");

        if (normalizedTime >= 1)
        {
            m_IsDramUp = false;
            return;
        }

        m_NowTime += Time.unscaledDeltaTime;
    }

    #endregion

    private ulong GetValue(E_RESULT_SCORE_TYPE type)
    {
        var chapter = DataManager.Instance.Chapter;
        var data = DataManager.Instance.BattleResultData.GetChapterResult(chapter);
        if (data == null)
        {
            return 0;
        }

        return /*data.GetScore(type);*/0;
    }

    /// <summary>
    /// 項目の初期化
    /// </summary>
    public void InitValue()
    {
        switch (m_ItemType)
        {
            default:
                m_Value.text = 0.ToString();
                break;
            case E_RESULT_SCORE_TYPE.SCORE:
                m_Value.text = GetValue(E_RESULT_SCORE_TYPE.SCORE).ToString();
                break;
        }
    }

    /// <summary>
    /// 項目の表示シーケンス
    /// </summary>
    public Sequence ShowItemSequence(float duration)
    {
        var lc = m_LabelColor;
        var vc = m_ValueColor;
        lc.a = 0;
        vc.a = 0;

        m_Label.color = lc;
        m_Value.color = vc;
        return DOTween.Sequence()
            .Append(m_Label.DOColor(m_LabelColor, duration))
            .Join(m_Value.DOColor(m_ValueColor, duration));
    }

    public void DramUpItem(float duration)
    {
        AudioManager.Instance.Play(m_ResultSe);
        PlayDramUp(0, GetValue(m_ItemType), duration);
    }

    public void DramUpTotalScore(E_RESULT_SCORE_TYPE type, float duration)
    {
        ulong value = 0;
        foreach (var t in m_BonusTypes)
        {
            if ((type & t) == t)
            {
                value += GetValue(t);
            }
        }

        m_CurrentValue = m_TargetValue;
        PlayDramUp(m_CurrentValue, value, duration);
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
        m_CurrentValue = currentValue;
        m_TargetValue = targetValue;
    }
}

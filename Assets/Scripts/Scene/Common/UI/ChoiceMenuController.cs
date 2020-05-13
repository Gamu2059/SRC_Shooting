using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

/// <summary>
/// ChoiceMenuを制御する
/// </summary>
[RequireComponent(typeof(ChoiceMenuIndicator))]
public class ChoiceMenuController : MonoBehaviour, ISelectHandler
{
    #region Field Inspector

    [SerializeField]
    private Text[] m_Indicators;

    [SerializeField]
    private Image m_LeftAllow;

    [SerializeField]
    private Image m_RightAllow;

    [SerializeField]
    private float m_MoveDuration;

    [SerializeField]
    private float m_SlideXAmount;

    [SerializeField]
    private float m_AllowSlideXAmount;

    [SerializeField]
    private float m_InputRecoverDuration;

    [SerializeField]
    private float m_InputDisableSelecteDuration;

    [SerializeField]
    private Ease m_OutEase;

    [SerializeField]
    private Ease m_InEase;

    [SerializeField]
    private E_COMMON_SOUND m_GoSound;

    #endregion

    #region Field

    private ChoiceMenuIndicator m_Indicator;
    private int m_PrevIndicatorIndex;
    private int m_CurrentIndicatorIndex;
    private bool m_IsEnableInput;

    #endregion

    private void Awake()
    {
        m_IsEnableInput = true;
        m_Indicator = GetComponent<ChoiceMenuIndicator>();
        m_PrevIndicatorIndex = -1;
        m_CurrentIndicatorIndex = 0;
    }

    private void Start()
    {
        m_Indicator.OnChangeValue += ChangeValueForce;
        ChangeValueForce();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            return;
        }

        var im = RewiredInputManager.Instance;
        if (im == null)
        {
            return;
        }

        if (!m_IsEnableInput)
        {
            return;
        }

        m_IsEnableInput = false;
        Observable
            .Timer(TimeSpan.FromSeconds(m_InputRecoverDuration))
            .Subscribe(_ => m_IsEnableInput = true)
            .AddTo(this);

        var x = im.UiAxisDir.x;
        if (x < 0)
        {
            GoPrev();
        }
        else if (x > 0)
        {
            GoNext();
        }
    }

    private void GoPrev()
    {
        if (m_Indicator == null || !m_Indicator.CanGoPrev())
        {
            return;
        }

        m_Indicator.GoPrev();
        Move(-m_SlideXAmount, m_LeftAllow.transform as RectTransform);
    }

    private void GoNext()
    {
        if (m_Indicator == null || !m_Indicator.CanGoNext())
        {
            return;
        }

        m_Indicator.GoNext();
        Move(m_SlideXAmount, m_RightAllow.transform as RectTransform);
    }

    private void Move(float slideXAmount, RectTransform allowRect)
    {
        AudioManager.Instance.Play(m_GoSound);

        m_PrevIndicatorIndex = m_CurrentIndicatorIndex;
        var prevInd = m_Indicators[m_PrevIndicatorIndex];
        m_CurrentIndicatorIndex = (m_CurrentIndicatorIndex + 1) % m_Indicators.Length;
        var nextInd = m_Indicators[m_CurrentIndicatorIndex];
        nextInd.text = GetStringValue();

        var prevRect = prevInd.transform as RectTransform;
        var nextRect = nextInd.transform as RectTransform;

        var pos = prevRect.anchoredPosition;
        pos.x = 0;
        prevRect.anchoredPosition = pos;
        pos = nextRect.anchoredPosition;
        pos.x = slideXAmount;
        nextRect.anchoredPosition = pos;

        DOTween.Sequence()
            .Append(prevRect.DOAnchorPosX(-slideXAmount, m_MoveDuration).SetEase(m_OutEase))
            .Join(prevInd.DOColor(Color.clear, m_MoveDuration).SetEase(m_OutEase))
            .Join(nextRect.DOAnchorPosX(0, m_MoveDuration).SetEase(m_InEase))
            .Join(nextInd.DOColor(Color.white, m_MoveDuration).SetEase(m_InEase));

        DOTween.Sequence()
            .Append(allowRect.DOAnchorPosX(m_AllowSlideXAmount, m_MoveDuration / 2f).SetEase(Ease.OutQuad))
            .Append(allowRect.DOAnchorPosX(0, m_MoveDuration / 2f).SetEase(Ease.InQuad))
            .OnComplete(ChangeEnableAllows);
    }

    private void ChangeValueForce()
    {
        var ind = m_Indicators[m_CurrentIndicatorIndex];
        ind.text = GetStringValue();
        ChangeEnableAllows();
    }

    private void ChangeEnableAllows()
    {
        if (m_Indicator == null)
        {
            return;
        }

        m_LeftAllow.gameObject.SetActive(m_Indicator.CanGoPrev());
        m_RightAllow.gameObject.SetActive(m_Indicator.CanGoNext());
    }

    private string GetStringValue()
    {
        if (m_Indicator == null)
        {
            return null;
        }

        switch (m_Indicator.Type)
        {
            case ChoiceMenuIndicator.E_TYPE.NUM:
                return m_Indicator.NumValue.ToString();
            case ChoiceMenuIndicator.E_TYPE.DIFFICULTY:
                return m_Indicator.DifficultyValue.ToString();
            case ChoiceMenuIndicator.E_TYPE.CHAPTER:
                return m_Indicator.ChapterValue.ToString().Replace("_", " ");
        }

        return null;
    }

    public void OnSelect(BaseEventData e)
    {
        m_IsEnableInput = false;
        var di = Observable
            .Timer(TimeSpan.FromSeconds(m_InputDisableSelecteDuration))
            .Subscribe(_ => m_IsEnableInput = true)
            .AddTo(this);
    }
}

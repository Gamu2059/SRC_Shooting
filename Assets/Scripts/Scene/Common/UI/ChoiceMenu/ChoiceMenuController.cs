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
public class ChoiceMenuController : MonoBehaviour, IMoveHandler
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
    private Ease m_OutEase;

    [SerializeField]
    private Ease m_InEase;

    [SerializeField]
    private E_COMMON_SOUND m_GoSound;

    #endregion

    #region Field

    private ChoiceMenuIndicator m_Target;
    private int m_PrevIndicatorIndex;
    private int m_CurrentIndicatorIndex;
    private bool m_CheckAllows;

    #endregion

    private void Awake()
    {
        m_Target = GetComponent<ChoiceMenuIndicator>();
        m_CheckAllows = true;
        m_PrevIndicatorIndex = -1;
        m_CurrentIndicatorIndex = 0;
    }

    private void Start()
    {
        m_Target.OnChangeValue += ChangeValueForce;
        ChangeValueForce();
    }

    private void GoPrev()
    {
        if (m_Target == null || !m_Target.CanGoPrev())
        {
            return;
        }

        m_CheckAllows = false;
        m_Target.GoPrev();
        Move(-m_SlideXAmount, m_LeftAllow.transform as RectTransform);
    }

    private void GoNext()
    {
        if (m_Target == null || !m_Target.CanGoNext())
        {
            return;
        }

        m_CheckAllows = false;
        m_Target.GoNext();
        Move(m_SlideXAmount, m_RightAllow.transform as RectTransform);
    }

    private void Move(float slideXAmount, RectTransform allowRect)
    {
        AudioManager.Instance.Play(m_GoSound);

        m_PrevIndicatorIndex = m_CurrentIndicatorIndex;
        var prevInd = m_Indicators[m_PrevIndicatorIndex];
        m_CurrentIndicatorIndex = (m_CurrentIndicatorIndex + 1) % m_Indicators.Length;
        var nextInd = m_Indicators[m_CurrentIndicatorIndex];
        nextInd.text = m_Target.GetStringValue();

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
            .OnComplete(() =>
            {
                ChangeEnableAllows();
                m_CheckAllows = true;
            });
    }

    private void ChangeValueForce()
    {
        var ind = m_Indicators[m_CurrentIndicatorIndex];
        ind.text = m_Target.GetStringValue();

        if (m_CheckAllows)
        {
            ChangeEnableAllows();
        }
    }

    private void ChangeEnableAllows()
    {
        if (m_Target == null)
        {
            return;
        }

        m_LeftAllow.gameObject.SetActive(m_Target.CanGoPrev());
        m_RightAllow.gameObject.SetActive(m_Target.CanGoNext());
    }

    public void OnMove(AxisEventData e)
    {
        switch (e.moveDir)
        {
            case MoveDirection.Left:
                GoPrev();
                break;
            case MoveDirection.Right:
                GoNext();
                break;
        }
    }
}

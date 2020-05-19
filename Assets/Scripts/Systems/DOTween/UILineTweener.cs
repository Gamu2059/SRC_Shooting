using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UILineTweener : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    #region Field Inspector

    [SerializeField]
    private Transform m_Target;

    [Header("Select")]

    [SerializeField]
    private float m_SelectedDuration;

    [SerializeField]
    private Vector3 m_SelectedLocalScale;

    [SerializeField]
    private Ease m_SelectedEase;

    [Header("Deselect")]

    [SerializeField]
    private float m_DeselectedDuration;

    [SerializeField]
    private Vector3 m_DeselectedLocalScale;

    [SerializeField]
    private Ease m_DeselectedEase;

    #endregion

    #region Field

    private Tween m_SelectedTween;
    private Tween m_DeselectedTween;

    #endregion

    public void OnSelect(BaseEventData e)
    {
        KillTween();
        m_SelectedTween = m_Target.DOScale(m_SelectedLocalScale, m_SelectedDuration).SetEase(m_SelectedEase);
    }

    public void OnDeselect(BaseEventData e)
    {
        KillTween();
        m_DeselectedTween = m_Target.DOScale(m_DeselectedLocalScale, m_DeselectedDuration).SetEase(m_DeselectedEase);
    }

    private void KillTween()
    {
        if (m_SelectedTween != null && !m_SelectedTween.IsComplete())
        {
            m_SelectedTween.Kill();
            m_SelectedTween = null;
        }

        if (m_DeselectedTween != null && !m_DeselectedTween.IsComplete())
        {
            m_DeselectedTween.Kill();
            m_DeselectedTween = null;
        }
    }
}

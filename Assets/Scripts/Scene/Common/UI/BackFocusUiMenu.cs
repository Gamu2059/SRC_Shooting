#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// メニューアイテムの背景をフォーカスするメニュー
/// </summary>
[Serializable]
public class BackFocusUiMenu : CommonUiMenu
{
    #region Field Inspector

    [SerializeField]
    private RectTransform m_BackFocus;

    [SerializeField]
    private AnimationCurve m_LerpPositionCurve;

    #endregion

    #region Field

    private bool m_IsAnimationBackFocus;
    private Vector2 m_CurrentBackFocusPosition;
    private Vector2 m_NextBackFocusPosition;
    private float m_BackFocusDuration;
    private float m_BackFocusTimeCount;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        DefocusAllItemAction += OnDefocusAllItem;
        FocusItemAction += OnFocusItem;

        m_IsAnimationBackFocus = false;
        m_BackFocusDuration = m_LerpPositionCurve.Duration();
        m_BackFocusTimeCount = 0;
    }

    public override void OnFinalize()
    {
        FocusItemAction -= OnFocusItem;
        DefocusAllItemAction -= OnDefocusAllItem;
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_IsAnimationBackFocus && m_BackFocus != null)
        {
            var lerp = m_LerpPositionCurve.Evaluate(m_BackFocusTimeCount);
            var pos = Vector2.Lerp(m_CurrentBackFocusPosition, m_NextBackFocusPosition, lerp);
            m_BackFocus.anchoredPosition = pos;

            m_BackFocusTimeCount += Time.deltaTime;
            if (m_BackFocusTimeCount >= m_BackFocusDuration)
            {
                m_IsAnimationBackFocus = false;
                m_BackFocus.anchoredPosition = m_NextBackFocusPosition;
            }
        }
    }

    #endregion

    public override void FocusMenu()
    {
        base.FocusMenu();

    }

    public override void DefocusMenu()
    {
        base.DefocusMenu();
        m_BackFocus?.gameObject.SetActive(false);
    }

    private void OnDefocusAllItem()
    {
        DefocusAllItemAction -= OnDefocusAllItem;
        m_BackFocus?.gameObject.SetActive(false);
    }

    private void OnFocusItem()
    {
        if (m_BackFocus == null || CurrentFocusMenuItem == null)
        {
            return;
        }

        if (m_IsAnimationBackFocus)
        {
            return;
        }

        m_IsAnimationBackFocus = true;
        if (!m_BackFocus.gameObject.activeSelf)
        {
            m_BackFocus.gameObject.SetActive(true);
        }

        m_BackFocusTimeCount = 0;
        m_CurrentBackFocusPosition = m_BackFocus.anchoredPosition;

        var menuItem = CurrentFocusMenuItem.transform as RectTransform;
        m_NextBackFocusPosition = menuItem.anchoredPosition;
    }
}

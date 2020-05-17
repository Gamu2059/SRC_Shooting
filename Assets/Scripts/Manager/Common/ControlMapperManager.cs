using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired.UI.ControlMapper;
using DG.Tweening;
using Doozy.Engine;

public class ControlMapperManager : SingletonMonoBehavior<ControlMapperManager>
{
    #region Field Inspector

    [SerializeField]
    private ControlMapper m_ControlMapper;

    [SerializeField]
    private RectTransform m_ControlMapperRoot;

    [SerializeField]
    private CanvasGroup m_ControlMapperCanvasGroup;

    [Header("Open Screen")]

    [SerializeField]
    private string m_OpenGameEvent;

    [SerializeField]
    private float m_OpenDuration;

    [SerializeField]
    private float m_OpenBeginXPosition;

    [SerializeField]
    private float m_OpenEndXPosition;

    [Header("Close Screen")]

    [SerializeField]
    private string m_CloseGameEvent;

    [SerializeField]
    private float m_CloseDuration;

    [SerializeField]
    private float m_CloseEndXPosition;

    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();

        m_ControlMapper.onScreenOpened += OnScreenOpened;
        m_ControlMapper.onScreenClosed += OnScreenClosed;
    }

    protected override void OnDestroyed()
    {
        m_ControlMapper.onScreenClosed -= OnScreenClosed;
        m_ControlMapper.onScreenOpened -= OnScreenOpened;

        base.OnDestroyed();
    }

    private void OnScreenOpened()
    {
        RewiredInputManager.Instance.ChangeToKeyConInputModule();
        GameEventMessage.SendEvent(m_OpenGameEvent);

        DOTween.Sequence()
            .Append(m_ControlMapperCanvasGroup.DOFade(1, m_OpenDuration))
            .Join(m_ControlMapperRoot.DOAnchorPosX(m_OpenEndXPosition, m_OpenDuration));
    }

    private void OnScreenClosed()
    {
        GameEventMessage.SendEvent(m_CloseGameEvent);

        // どの場所から始まるのか不明なので(開いた直後に閉じると、想定してる値と異なる場合がある)、初期値は設定しない
        DOTween.Sequence()
            .Append(m_ControlMapperCanvasGroup.DOFade(0, m_CloseDuration))
            .Join(m_ControlMapperRoot.DOAnchorPosX(m_CloseEndXPosition, m_CloseDuration))
            .OnComplete(() =>
            {
                m_ControlMapperCanvasGroup.gameObject.SetActive(false);
                RewiredInputManager.Instance.ChangeToUIInputModule();
            });
    }

    public void Open()
    {
        m_ControlMapperCanvasGroup.alpha = 0;

        var pos = m_ControlMapperRoot.anchoredPosition;
        pos.x = m_OpenBeginXPosition;
        m_ControlMapperRoot.anchoredPosition = pos;

        m_ControlMapper.Open();
    }
}

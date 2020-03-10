#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// 汎用的なフェード処理を管理する。
/// </summary>
public class FadeManager : SingletonMonoBehavior<FadeManager>
{
    #region Field Inspector

    [SerializeField]
    private bool m_IsUpdateManualy;

    [Header("Target Object")]

    [SerializeField]
    private Canvas m_Canvas;

    [SerializeField]
    private Image m_FadeImage;

    [Header("Parameter")]

    [SerializeField]
    private int m_CanvasOrder;

    [SerializeField]
    private Color m_InitColor;

    #endregion

    #region Field

    // 今後複数のフェードを同時に処理する場合とかにはリストにする
    private FadeData m_FadeData;
    public bool IsFading { get; private set; }

    #endregion

    #region Game Cycle

    protected override void OnAwake()
    {
        base.OnAwake();
        if (!m_IsUpdateManualy)
        {
            OnInitialize();
        }
    }

    protected override void OnDestroyed()
    {
        if (!m_IsUpdateManualy)
        {
            OnFinalize();
        }
        base.OnDestroyed();
    }

    private void Start()
    {
        if (!m_IsUpdateManualy)
        {
            OnStart();
        }
    }

    private void Update()
    {
        if (!m_IsUpdateManualy)
        {
            OnUpdate();
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_Canvas.sortingOrder = m_CanvasOrder;
        m_FadeImage.color = m_InitColor;
        IsFading = false;
        m_FadeData = null;
    }

    public override void OnFinalize()
    {
        m_FadeData = null;
        IsFading = false;
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!IsFading)
        {
            return;
        }

        m_FadeData.Update(Time.deltaTime);
        m_FadeImage.color = m_FadeData.GetColor();
        if (m_FadeData.IsDone())
        {
            m_FadeData.CallComplete();
        }
    }

    #endregion

    /// <summary>
    /// フェードパラメータを使ってフェードする。
    /// フェードが開始できなかった場合、falseを返す。
    /// </summary>
    public bool Fade(FadeParam fadeParam, Action onComplete = null)
    {
        bool result = false;
        if (fadeParam.IsFadeOut)
        {
            if (fadeParam.UseAnimation)
            {
                result = FadeOut(fadeParam.RateCurve, fadeParam.FadeOutColor, onComplete);
            }
            else
            {
                result =FadeOut(fadeParam.Duration, fadeParam.FadeOutColor, onComplete);
            }
        }
        else
        {
            if (fadeParam.UseAnimation)
            {
                result = FadeIn(fadeParam.RateCurve, onComplete);
            }
            else
            {
                result = FadeIn(fadeParam.Duration, onComplete);
            }
        }

        return result;
    }

    /// <summary>
    /// 画面を特定の色で塗りつぶす。
    /// フェードが開始できなかった場合、falseを返す。
    /// </summary>
    public bool FadeOut(float duration, Color color, Action onComplete = null)
    {
        if (IsFading)
        {
            return false;
        }

        Action completeAction = () => {
            IsFading = false;
            m_FadeData = null;
            m_FadeImage.color = color;
            onComplete?.Invoke();
        };
        m_FadeData = new FadeData(duration, m_FadeImage.color, color, completeAction);
        m_FadeData.Start();
        IsFading = true;

        return true;
    }

    /// <summary>
    /// 画面を特定の色で塗りつぶす。
    /// フェードが開始できなかった場合、falseを返す。
    /// </summary>
    public bool FadeOut(AnimationCurve rateCurve, Color color, Action onComplete = null)
    {
        if (IsFading)
        {
            return false;
        }

        Action completeAction = () => {
            IsFading = false;
            m_FadeData = null;
            m_FadeImage.color = color;
            onComplete?.Invoke();
        };
        m_FadeData = new FadeData(rateCurve, m_FadeImage.color, color, completeAction);
        m_FadeData.Start();
        IsFading = true;

        return true;
    }

    /// <summary>
    /// 画面の塗りつぶしを消す。
    /// フェードが開始できなかった場合、falseを返す。
    /// </summary>
    public bool FadeIn(float duration, Action onComplete = null)
    {
        if (IsFading)
        {
            return false;
        }

        var color = m_FadeImage.color;
        color.a = 0;

        Action completeAction = () => {
            IsFading = false;
            m_FadeData = null;
            m_FadeImage.color = color;
            onComplete?.Invoke();
        };
        m_FadeData = new FadeData(duration, m_FadeImage.color, color, completeAction);
        m_FadeData.Start();
        IsFading = true;

        return true;
    }

    /// <summary>
    /// 画面の塗りつぶしを消す。
    /// フェードが開始できなかった場合、falseを返す。
    /// </summary>
    public bool FadeIn(AnimationCurve rateCurve, Action onComplete = null)
    {
        if (IsFading)
        {
            return false;
        }

        var color = m_FadeImage.color;
        color.a = 0;

        Action completeAction = () => {
            IsFading = false;
            m_FadeData = null;
            m_FadeImage.color = color;
            onComplete?.Invoke();
        };
        m_FadeData = new FadeData(rateCurve, m_FadeImage.color, color, completeAction);
        m_FadeData.Start();
        IsFading = true;

        return true;
    }

    private class FadeData
    {
        private enum E_PHASE
        {
            STANDBY_START,
            UPDATE,
            DONE,
        }

        private float m_CurrentTime;
        private float m_Duration;
        private bool m_UseRateCurve;
        private AnimationCurve m_RateCurve;
        private Color m_StartColor;
        private Color m_EndColor;
        private Action m_OnComplete;
        private E_PHASE m_Phase;

        public FadeData(float duration, Color startColor, Color endColor, Action onComplete)
        {
            m_CurrentTime = 0;
            m_UseRateCurve = false;
            m_Duration = duration;
            m_StartColor = startColor;
            m_EndColor = endColor;
            m_OnComplete = onComplete;
            m_Phase = E_PHASE.STANDBY_START;
        }

        public FadeData(AnimationCurve curve, Color startColor, Color endColor, Action onComplete)
        {
            m_CurrentTime = 0;
            m_UseRateCurve = true;
            m_RateCurve = curve;
            m_Duration = m_RateCurve.Duration();
            m_StartColor = startColor;
            m_EndColor = endColor;
            m_OnComplete = onComplete;
            m_Phase = E_PHASE.STANDBY_START;
        }

        public void Start()
        {
            m_Phase = E_PHASE.UPDATE;
        }

        public void Update(float deltaTime)
        {
            if (m_Phase != E_PHASE.UPDATE)
            {
                return;
            }

            m_CurrentTime += deltaTime;
            if (m_CurrentTime >= m_Duration)
            {
                m_Phase = E_PHASE.DONE;
            }
        }

        public bool IsDone()
        {
            return m_Phase == E_PHASE.DONE;
        }

        public void CallComplete()
        {
            m_OnComplete?.Invoke();
        }

        public Color GetColor()
        {
            if (m_Duration <= 0)
            {
                return m_EndColor;
            }

            var rate = m_CurrentTime / m_Duration;
            if (m_UseRateCurve)
            {
                rate = m_RateCurve.Evaluate(m_CurrentTime);
            }

            var r = (m_EndColor.r - m_StartColor.r) * rate + m_StartColor.r;
            var g = (m_EndColor.g - m_StartColor.g) * rate + m_StartColor.g;
            var b = (m_EndColor.b - m_StartColor.b) * rate + m_StartColor.b;
            var a = (m_EndColor.a - m_StartColor.a) * rate + m_StartColor.a;
            return new Color(r, g, b, a);
        }
    }
}

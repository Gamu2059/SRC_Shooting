using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : ControllableMonoBehavior
{
    [Serializable]
    public enum E_SE_GROUP
    {
        GLOBAL,
        PLAYER,
    }

    [Serializable]
    public enum E_AISAC_TYPE
    {
        BGM_FADE_CONTROL,
    }

    [Serializable]
    private struct SeGroup
    {
        public E_SE_GROUP Group;
        public CriAtomSource Source;
    }

    public static AudioManager Instance => GameManager.Instance.AudioManager;

    [SerializeField]
    private CriWareInitializer m_CriWareInitializer;

    [SerializeField]
    private CriAtomSource[] m_BgmSources;

    [SerializeField]
    private SeGroup[] m_SeGroups;

    private Dictionary<E_AISAC_TYPE, string> m_AisacDict;

    private int m_BgmSourceIndex;

    private float m_Duration;
    private AnimationCurve m_FadeOutCurve;
    private AnimationCurve m_FadeInCurve;
    private float m_FadeOutDurationRate;
    private float m_FadeInDurationRate;

    private bool m_IsFading;
    private float m_CurrentTime;
    private CriAtomSource m_FadeOutSource;
    private CriAtomSource m_FadeInSource;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CriWareInitializer.Initialize();

        m_AisacDict = new Dictionary<E_AISAC_TYPE, string>()
        {
            { E_AISAC_TYPE.BGM_FADE_CONTROL, "BGM_FadeControl"},
        };

        m_BgmSourceIndex = 0;
        InitFadeField();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (!m_IsFading)
        {
            return;
        }

        m_CurrentTime += Time.fixedDeltaTime;

        if (m_FadeOutSource != null)
        {
            var fadeOutValue = m_FadeOutCurve.Evaluate(m_FadeOutDurationRate * m_CurrentTime);
            m_FadeOutSource.volume = fadeOutValue;
        }

        if (m_FadeInSource != null)
        {
            var fadeInValue = m_FadeInCurve.Evaluate(m_FadeInDurationRate * m_CurrentTime);
            m_FadeInSource.volume = fadeInValue;
        }

        if (m_CurrentTime >= m_Duration)
        {
            if (m_FadeOutSource != null)
            {
                m_FadeOutSource.volume = 0;
                m_FadeOutSource.Stop();
            }

            if (m_FadeInSource != null)
            {
                m_FadeInSource.volume = 1;
            }

            InitFadeField();
        }
    }

    #endregion

    private void InitFadeField()
    {
        m_Duration = 0;
        m_CurrentTime = 0;
        m_FadeOutCurve = null;
        m_FadeInCurve = null;
        m_FadeOutDurationRate = 0;
        m_FadeInDurationRate = 0;
        m_FadeOutSource = null;
        m_FadeInSource = null;
        m_IsFading = false;
    }

    private CriAtomSource GetCurrentBgmSource()
    {
        if (m_BgmSources == null || m_BgmSources.Length < 1)
        {
            return null;
        }

        var idx = m_BgmSourceIndex % m_BgmSources.Length;
        return m_BgmSources[m_BgmSourceIndex];
    }

    private CriAtomSource GetNextBgmSource()
    {
        if (m_BgmSources == null || m_BgmSources.Length < 1)
        {
            return null;
        }

        var idx = (m_BgmSourceIndex + 1) % m_BgmSources.Length;
        return m_BgmSources[idx];
    }

    private CriAtomSource GetSeSource(E_SE_GROUP group)
    {
        for (int i = 0; i < m_SeGroups.Length; i++)
        {
            var g = m_SeGroups[i];
            if (g.Group == group)
            {
                return g.Source;
            }
        }

        return null;
    }

    public void PlaySe(E_SE_GROUP group, string name)
    {
        var source = GetSeSource(group);
        if (source == null)
        {
            return;
        }

        source.cueName = name;
        source.Play();
    }

    public void StopSe(E_SE_GROUP group)
    {
        var source = GetSeSource(group);
        if (source != null)
        {
            source.Stop();
        }
    }

    /// <summary>
    /// クロスフェードを使ってBGMを再生する
    /// </summary>
    public void PlayBgm(string name, float duration, AnimationCurve fadeOutCurve, AnimationCurve fadeInCurve)
    {
        if (m_IsFading)
        {
            return;
        }

        var current = GetCurrentBgmSource();
        var next = GetNextBgmSource();
        if (current == null || next == null)
        {
            return;
        }

        if (duration <= 0)
        {
            return;
        }

        if (fadeOutCurve == null || fadeInCurve == null)
        {
            return;
        }

        var fadeOutLength = fadeOutCurve.keys.Length;
        var fadeInLength = fadeInCurve.keys.Length;
        if (fadeOutLength < 1 || fadeInLength < 1)
        {
            return;
        }

        var fadeOutDuration = fadeOutCurve.keys[fadeOutLength - 1].time;
        var fadeInDuration = fadeInCurve.keys[fadeInLength - 1].time;
        if (fadeOutDuration <= 0 || fadeInDuration <= 0)
        {
            return;
        }

        m_Duration = duration;
        m_FadeOutCurve = fadeOutCurve;
        m_FadeInCurve = fadeInCurve;
        m_FadeOutDurationRate = fadeOutDuration / duration;
        m_FadeInDurationRate = fadeInDuration / duration;
        m_FadeOutSource = current;
        m_FadeInSource = next;

        m_FadeOutSource.volume = 1;
        m_FadeInSource.volume = 0;
        m_FadeInSource.cueName = name;
        m_FadeInSource.Play();

        m_BgmSourceIndex = (m_BgmSourceIndex + 1) % m_BgmSources.Length;
        m_CurrentTime = 0;
        m_IsFading = true;
    }

    /// <summary>
    /// フェードアウトを使ってBGMを止める
    /// </summary>
    public void StopBgm(float duration, AnimationCurve fadeOutCurve)
    {
        if (m_IsFading)
        {
            return;
        }

        var current = GetCurrentBgmSource();
        if (current == null)
        {
            return;
        }

        if (duration <= 0 || fadeOutCurve == null)
        {
            return;
        }

        var fadeOutLength = fadeOutCurve.keys.Length;
        if (fadeOutLength < 1)
        {
            return;
        }

        var fadeOutDuration = fadeOutCurve.keys[fadeOutLength - 1].time;
        if (fadeOutDuration <= 0)
        {
            return;
        }

        m_Duration = duration;
        m_FadeOutCurve = fadeOutCurve;
        m_FadeInCurve = null;
        m_FadeOutDurationRate = fadeOutDuration / duration;
        m_FadeOutSource = current;
        m_FadeInSource = null;

        m_FadeOutSource.volume = 1;
        m_CurrentTime = 0;
        m_IsFading = true;
    }

    /// <summary>
    /// 即時にBGMを再生する
    /// </summary>
    public void PlayBgmImmediate(string name)
    {
        var current = GetCurrentBgmSource();
        if (current == null)
        {
            return;
        }

        InitFadeField();

        current.cueName = name;
        current.volume = 1;
        current.Play();
    }

    /// <summary>
    /// 即時にBGMを止める
    /// </summary>
    public void StopBgmImmediate()
    {
        var current = GetCurrentBgmSource();
        if (current == null)
        {
            return;
        }

        InitFadeField();

        current.volume = 0;
        current.Stop();
    }

    public void StopAllBgmImmediate()
    {
        foreach (var source in m_BgmSources)
        {
            source.volume = 0;
            source.Stop();
        }
    }

    public void SetBgmAisac(E_AISAC_TYPE controlType, float value)
    {
        var current = GetCurrentBgmSource();
        if (current == null)
        {
            return;
        }

        var type = m_AisacDict[controlType];
        current.SetAisacControl(type, value);
    }
}

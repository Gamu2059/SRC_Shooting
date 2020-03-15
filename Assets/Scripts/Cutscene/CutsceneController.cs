﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;

/// <summary>
/// カットシーンの呼び出しやコールバックを制御する。
/// </summary>
public class CutsceneController : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector m_PlayableDirector;

    /// <summary>
    /// カットシーン終了時に呼び出されるコールバック
    /// </summary>
    public Action OnCompleteCutscene;
    public float TimeRate;
    private bool m_IsDone;

    #region Cycle

    private void Awake()
    {
        TimeRate = 1;
        m_IsDone = false;
    }

    private void OnDestroy()
    {
        OnCompleteCutscene = null;
    }

    private void Update()
    {
        if (m_IsDone)
        {
            return;
        }

        m_PlayableDirector.time += Time.deltaTime * Mathf.Max(TimeRate, 0f);
        m_PlayableDirector.Evaluate();

        var time = m_PlayableDirector.time;
        var duration = m_PlayableDirector.duration;
        var isDone = duration <= 0f ? true : time / duration >= 1f;

        if (isDone)
        {
            m_IsDone = true;
            OnCompleteCutscene?.Invoke();
        }
    }

    #endregion

    public void Play()
    {
        m_PlayableDirector.Play();
    }

    public void Stop()
    {
        m_PlayableDirector.Stop();
    }

    public void Pause()
    {
        m_PlayableDirector.Pause();
    }

    public void Resume()
    {
        m_PlayableDirector.Resume();
    }
}
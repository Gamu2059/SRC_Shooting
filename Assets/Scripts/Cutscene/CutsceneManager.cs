#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/// <summary>
/// カットシーンを制御する。
/// </summary>
public class CutsceneManager : SingletonMonoBehavior<CutsceneManager>
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

    protected override void OnAwake()
    {
        base.OnAwake();
        OnInitialize();
    }

    protected override void OnDestroyed()
    {
        OnFinalize();
        base.OnDestroyed();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        TimeRate = 1;
        m_IsDone = false;
    }

    public override void OnFinalize()
    {
        OnCompleteCutscene?.Invoke();
        OnCompleteCutscene = null;
        base.OnFinalize();
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
            SceneManager.UnloadSceneAsync(gameObject.scene);
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

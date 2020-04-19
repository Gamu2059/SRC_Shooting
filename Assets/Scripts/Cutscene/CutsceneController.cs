using System.Collections;
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
    private bool m_IsPaused;

    #region Cycle

    private void Awake()
    {
        TimeRate = 1;
        m_IsDone = false;
        m_IsPaused = false;
    }

    private void OnDestroy()
    {
        OnCompleteCutscene = null;
    }

    private void Update()
    {
        if (m_IsDone || m_IsPaused)
        {
            return;
        }

        m_PlayableDirector.time += GetDeltaTime();
        m_PlayableDirector.Evaluate();

        var time = m_PlayableDirector.time;
        var duration = m_PlayableDirector.duration;
        var isDone = duration <= 0f ? true : time / duration >= 1f;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isDone = true;
        }
#endif

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
        //m_PlayableDirector.Pause();
        m_IsPaused = true;
    }

    public void Resume()
    {
        //m_PlayableDirector.Resume();
        m_IsPaused = false;
    }

    public float GetDeltaTime()
    {
        return Time.deltaTime * Mathf.Max(TimeRate, 0f);
    }
}

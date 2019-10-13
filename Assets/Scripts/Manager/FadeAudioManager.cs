using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// BGMにフェード機能を付けたもの。
/// </summary>
public class FadeAudioManager : SingletonMonoBehavior<FadeAudioManager>
{
    private AudioSource[] m_AuidoSources;

    private int m_CurrentIndex;

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CurrentIndex = 0;

        m_AuidoSources = GetComponentsInChildren<AudioSource>();
    }

    public void PlayBGM(AudioClip clip, float fadeOutDuration, float playOffset, float fadeInDuration)
    {
        if (clip == null)
        {
            return;
        }

        var currentSource = m_AuidoSources[m_CurrentIndex];
        if (currentSource.isPlaying)
        {
            StopBGM(fadeOutDuration);
            m_CurrentIndex = (m_CurrentIndex + 1) % m_AuidoSources.Length;
            var nextSource = m_AuidoSources[m_CurrentIndex];

            PlayBGM(nextSource, clip, playOffset, fadeInDuration);
        } else
        {
            PlayBGM(currentSource, clip, playOffset, fadeInDuration);
        }
    }

    private void PlayBGM(AudioSource source, AudioClip clip, float playOffset, float fadeInDuration)
    {
        if (playOffset <= 0)
        {
            PlayBGM(source, clip, fadeInDuration);
        }
        else
        {
            var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, playOffset, () => {
                PlayBGM(source, clip, fadeInDuration);
            });
            //TimerManager.Instance.RegistTimer(timer);
        }
    }

    private void PlayBGM(AudioSource source, AudioClip clip, float fadeInDuration)
    {
        source.volume = 0;
        source.clip = clip;
        source.Play();
        source.DOFade(1, fadeInDuration);
    }

    public void StopBGM(float fadeOutDuration)
    {
        var currentSource = m_AuidoSources[m_CurrentIndex];
        currentSource.DOFade(0, fadeOutDuration).OnComplete(()=>currentSource.Stop());
    }
}

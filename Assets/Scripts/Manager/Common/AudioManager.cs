using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : ControllableMonoBehavior
{
    public static AudioManager Instance => GameManager.Instance.AudioManager;

    [SerializeField]
    private CriWareInitializer m_CriWareInitializer;

    [SerializeField]
    private CriAtomSource m_CriAtomBgmSource;

    [SerializeField]
    private CriAtomSource m_CriAtomSeSource;

    [SerializeField]
    private AudioSource m_PrimaryBgmSource;

    [SerializeField]
    private AudioSource m_SecondaryBgmSource;

    [SerializeField]
    private AudioSource m_SeSource;

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CriWareInitializer.Initialize();
    }

    public void PlaySeAdx2(string name)
    {
        m_CriAtomSeSource.cueName = name;
        m_CriAtomSeSource.Play();
    }

    //public void PlaySe(AudioClip seClip)
    //{
    //    m_SeSource.PlayOneShot(seClip);
    //}

    //public void PlayPrimaryBgm(AudioClip bgmClip)
    //{
    //    m_PrimaryBgmSource.Stop();
    //    m_PrimaryBgmSource.clip = bgmClip;
    //    m_PrimaryBgmSource.Play();
    //}

    //public void PlaySecondaryBgm(AudioClip bgmClip)
    //{
    //    m_SecondaryBgmSource.Stop();
    //    m_SecondaryBgmSource.clip = bgmClip;
    //    m_SecondaryBgmSource.Play();
    //}

    //public void SetPrimaryBgmVolume(float normalizedVolume)
    //{
    //    m_PrimaryBgmSource.volume = normalizedVolume;
    //}

    //public void SetSecondaryBgmVolume(float normalizedVolume)
    //{
    //    m_SecondaryBgmSource.volume = normalizedVolume;
    //}

    //public void PlayBossBgm(BattleBossBgmParamSet bossBgmParamSet)
    //{
    //    m_PrimaryBgmSource.Stop();
    //    m_SecondaryBgmSource.Stop();

    //    m_PrimaryBgmSource.PlayOneShot(bossBgmParamSet.RealModeIntro);
    //    m_PrimaryBgmSource.clip = bossBgmParamSet.RealModeLoop;
    //    m_PrimaryBgmSource.PlayDelayed(bossBgmParamSet.RealModeIntro.length);

    //    m_SecondaryBgmSource.PlayOneShot(bossBgmParamSet.HackingModeIntro);
    //    m_SecondaryBgmSource.clip = bossBgmParamSet.HackingModeLoop;
    //    m_SecondaryBgmSource.PlayDelayed(bossBgmParamSet.HackingModeIntro.length);
    //}

    public void PlayBossBgmAdx2()
    {
        m_CriAtomBgmSource.Play();
    }

    public void SetAisac(float value)
    {
        m_CriAtomBgmSource.SetAisacControl("BGM_FadeControll", value);
    }
}

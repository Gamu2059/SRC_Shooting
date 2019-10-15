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
    private struct SeGroup
    {
        public E_SE_GROUP Group;
        public CriAtomSource Source;
    }

    public static AudioManager Instance => GameManager.Instance.AudioManager;

    [SerializeField]
    private CriWareInitializer m_CriWareInitializer;

    [SerializeField]
    private CriAtomSource m_CriAtomBgmSource;

    [SerializeField]
    private SeGroup[] m_SeGroups;

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CriWareInitializer.Initialize();
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

    public void PlaySeAdx2(E_SE_GROUP group, string name)
    {
        var source = GetSeSource(group);
        if (source == null)
        {
            return;
        }

        source.cueName = name;
        source.Play();
    }

    public void StopSeAdx2(E_SE_GROUP group)
    {
        var source = GetSeSource(group);
        if (source != null)
        {
            source.Stop();
        }
    }

    public void PlayBgmAdx2(string name)
    {
        m_CriAtomBgmSource.cueName = name;
        m_CriAtomBgmSource.Play();
    }

    public void StopBgmAdx2()
    {
        m_CriAtomBgmSource.Stop();
    }

    public void SetAisac(float value)
    {
        m_CriAtomBgmSource.SetAisacControl("BGM_FadeControl", value);
    }
}

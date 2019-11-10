#pragma warning disable 0649

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
        ENEMY,
        SYSTEM,
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
    private CriAtomSource m_BgmSource;

    [SerializeField]
    private SeGroup[] m_SeGroups;

    private Dictionary<E_AISAC_TYPE, string> m_AisacDict;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CriWareInitializer.Initialize();
        m_AisacDict = new Dictionary<E_AISAC_TYPE, string>()
        {
            { E_AISAC_TYPE.BGM_FADE_CONTROL, "BGM_FadeControl"},
        };
    }

    #endregion

    private CriAtomSource GetSeSource(E_SE_GROUP group)
    {
        for (int i=0;i<m_SeGroups.Length;i++)
        {
            if (group == m_SeGroups[i].Group)
            {
                return m_SeGroups[i].Source;
            }
        }

        return null;
    }

    public void PlaySe(E_SE_GROUP group, string name)
    {
        var source = GetSeSource(group);
        source.cueName = name;
        source.Play();
    }

    public void StopSe(E_SE_GROUP group)
    {
        var source = GetSeSource(group);
        source.Stop();
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    public void PlayBgm(string name)
    {
        m_BgmSource.cueName = name;
        m_BgmSource.Play();
    }

    /// <summary>
    /// BGMを止める
    /// </summary>
    public void StopBgm()
    {
        m_BgmSource.Stop();
    }

    public void SetBgmAisac(E_AISAC_TYPE controlType, float value)
    {
        var type = m_AisacDict[controlType];
        m_BgmSource.SetAisacControl(type, value);
    }
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : ControllableMonoBehavior
{

    [Serializable]
    private struct SeGroup
    {
        public E_CUE_SHEET Group;
        public CriAtomSource Source;
    }

    public static AudioManager Instance => GameManager.Instance.AudioManager;

    [SerializeField]
    private CriWareInitializer m_CriWareInitializer;

    [SerializeField]
    private CriAtomSource m_BgmSource;

    [SerializeField]
    private SeGroup[] m_SeGroups;

    private Dictionary<E_CUE_SHEET, CriAtomSource> m_SeSourceDict;
    private Dictionary<E_AISAC_TYPE, string> m_AisacDict;
    private Dictionary<E_CUE_NAME, AdxAssetParam.CueSet> m_CueDict;

    #region Game Cycle

    /// <summary>
    /// Adxパラメータをセットする。
    /// OnInitializeより先に呼び出す。
    /// </summary>
    /// <param name="adxParam"></param>
    public void SetAdxParam(AdxAssetParam adxParam)
    {
        m_AisacDict = new Dictionary<E_AISAC_TYPE, string>();
        m_CueDict = new Dictionary<E_CUE_NAME, AdxAssetParam.CueSet>();

        foreach (var aisacSet in adxParam.AisacSets)
        {
            m_AisacDict.Add(aisacSet.AisacType, aisacSet.Name);
        }

        foreach (var cueSet in adxParam.CueSets)
        {
            m_CueDict.Add(cueSet.CueName, cueSet);
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_CriWareInitializer.Initialize();

        m_SeSourceDict = new Dictionary<E_CUE_SHEET, CriAtomSource>();
        foreach (var seGroup in m_SeGroups)
        {
            m_SeSourceDict.Add(seGroup.Group, seGroup.Source);
        }
    }

    #endregion

    private CriAtomSource GetSeSource(E_CUE_SHEET sheet)
    {
        return m_SeSourceDict[sheet];
    }

    public void PlaySe(E_CUE_NAME cue)
    {
        var cueSet = m_CueDict[cue];
        var source = GetSeSource(cueSet.BelongCueSheet);
        source.cueName = cueSet.Name;
        source.Play();
    }

    public void PlaySe(E_CUE_SHEET sheet, E_CUE_NAME cue)
    {
        var cueSet = m_CueDict[cue];
        var source = GetSeSource(sheet);
        source.cueName = cueSet.Name;
        source.Play();
    }

    public void StopSe(E_CUE_SHEET sheet)
    {
        var source = GetSeSource(sheet);
        source.Stop();
    }

    public void PlayBgm(E_CUE_NAME cue)
    {
        var cueSet = m_CueDict[cue];
        m_BgmSource.cueName = cueSet.Name;
        m_BgmSource.Play();
    }

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

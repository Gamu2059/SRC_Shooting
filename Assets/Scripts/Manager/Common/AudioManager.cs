#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : ControllableMonoBehavior
{
    #region Definition

    [Serializable]
    private struct SourceSet
    {
        public E_CUE_SHEET CueSheet;
        public CriAtomSource Source;
    }

    private class OperateAisacData
    {
        public OperateAisacParam OperateAisacParam;
        public float NowTime;

        public OperateAisacData(OperateAisacParam param)
        {
            OperateAisacParam = param;
            NowTime = 0;
        }
    }

    #endregion

    public static AudioManager Instance => GameManager.Instance.AudioManager;

    #region Field Inspector

    [SerializeField]
    private CriWareInitializer m_CriWareInitializer;

    [SerializeField]
    private SourceSet[] m_SourceSets;

    #endregion

    #region Field

    private AdxAssetParam m_AdxAssetParam;

    private Dictionary<E_CUE_SHEET, CriAtomSource> m_SourceDict;
    private Dictionary<E_AISAC_TYPE, string> m_AisacDict;

    private List<OperateAisacData> m_ProcessingOperateAisacList;
    private List<OperateAisacData> m_DestroyOperateAisacList;

    #endregion

    #region Game Cycle

    /// <summary>
    /// Adxパラメータをセットする。
    /// OnInitializeより先に呼び出す。
    /// </summary>
    /// <param name="adxParam"></param>
    public void SetAdxParam(AdxAssetParam adxParam)
    {
        m_AdxAssetParam = adxParam;

        m_AisacDict = new Dictionary<E_AISAC_TYPE, string>();

        foreach (var aisacSet in adxParam.AisacSets)
        {
            m_AisacDict.Add(aisacSet.AisacType, aisacSet.Name);
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_CriWareInitializer.Initialize();

        m_SourceDict = new Dictionary<E_CUE_SHEET, CriAtomSource>();
        foreach (var sourceSet in m_SourceSets)
        {
            m_SourceDict.Add(sourceSet.CueSheet, sourceSet.Source);
        }

        m_ProcessingOperateAisacList = new List<OperateAisacData>();
        m_DestroyOperateAisacList = new List<OperateAisacData>();
    }

    public override void OnFinalize()
    {
        m_DestroyOperateAisacList.Clear();
        m_ProcessingOperateAisacList.Clear();
        m_SourceDict.Clear();

        m_DestroyOperateAisacList = null;
        m_ProcessingOperateAisacList = null;
        m_SourceDict = null;

        m_AdxAssetParam = null;

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        foreach (var data in m_ProcessingOperateAisacList)
        {
            var aisac = data.OperateAisacParam.AisacType;
            var sheet = data.OperateAisacParam.TargetCueSheet;
            var anim = data.OperateAisacParam.AnimationValue;
            var value = anim.Evaluate(data.NowTime);
            OperateAisac(aisac, sheet, value);

            if (data.NowTime > anim.Duration())
            {
                m_DestroyOperateAisacList.Add(data);
                continue;
            }

            data.NowTime += Time.deltaTime;
        }
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        foreach (var data in m_DestroyOperateAisacList)
        {
            m_ProcessingOperateAisacList.Remove(data);
        }

        m_DestroyOperateAisacList.Clear();
    }

    #endregion

    private CriAtomSource GetSource(E_CUE_SHEET sheet)
    {
        if (m_SourceDict == null || !m_SourceDict.ContainsKey(sheet))
        {
            return null;
        }

        return m_SourceDict[sheet];
    }

    /// <summary>
    /// サウンドを再生する。
    /// BGMかSEかは問わない。
    /// </summary>
    public void Play(PlaySoundParam playSoundParam)
    {
        if (playSoundParam == null)
        {
            return;
        }

        var source = GetSource(playSoundParam.CueSheet);
        if (source == null)
        {
            return;
        }

        source.cueName = playSoundParam.CueName;
        source.Play();
    }

    /// <summary>
    /// サウンドを一時停止する。
    /// BGMかSEかは問わない。
    /// </summary>
    public void Pause(E_CUE_SHEET target)
    {
        var source = GetSource(target);
        if (source == null)
        {
            return;
        }

        if (!source.IsPaused())
        {
            source.Pause(true);
        }
    }

    /// <summary>
    /// サウンドを再開する。
    /// BGMかSEかは問わない。
    /// </summary>
    public void Resume(E_CUE_SHEET target)
    {
        var source = GetSource(target);
        if (source == null)
        {
            return;
        }

        if (source.IsPaused())
        {
            source.Pause(false);
        }
    }

    /// <summary>
    /// サウンドを停止する。
    /// BGMかSEかは問わない。
    /// </summary>
    public void Stop(E_CUE_SHEET target)
    {
        var source = GetSource(target);
        if (source != null)
        {
            source.Stop();
        }
    }

    /// <summary>
    /// 全てのSEを停止する。
    /// </summary>
    public void StopAllSe()
    {
        foreach (var t in m_AdxAssetParam.SeCueSheets)
        {
            Stop(t);
        }
    }

    /// <summary>
    /// 全てのBGMを停止する。
    /// </summary>
    public void StopAllBgm()
    {
        foreach (var t in m_AdxAssetParam.BgmCueSheets)
        {
            Stop(t);
        }
    }

    /// <summary>
    /// AISACを制御する。
    /// </summary>
    public void OperateAisac(OperateAisacParam operateAisacParam)
    {
        if (operateAisacParam == null)
        {
            return;
        }

        if (operateAisacParam.UseAnimationValue)
        {
            m_ProcessingOperateAisacList.Add(new OperateAisacData(operateAisacParam));
        }
        else
        {
            OperateAisac(operateAisacParam.AisacType, operateAisacParam.TargetCueSheet, operateAisacParam.ConstantValue);
        }
    }

    /// <summary>
    /// AISACを直接制御する。
    /// </summary>
    public void OperateAisac(E_AISAC_TYPE targetAisac, E_CUE_SHEET targetSheet, float value)
    {
        if (m_AisacDict == null || !m_AisacDict.ContainsKey(targetAisac))
        {
            return;
        }

        var source = GetSource(targetSheet);
        if (source == null)
        {
            return;
        }

        var aisac = m_AisacDict[targetAisac];
        source.SetAisacControl(aisac, value);
    }
}

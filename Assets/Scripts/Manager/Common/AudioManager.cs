#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : SingletonMonoBehavior<AudioManager>
{
    #region Define

    [Serializable]
    private struct SourceSet
    {
        public E_CUE_SHEET CueSheet;
        public CriAtomSource Source;
    }

    [Serializable]
    private struct CommonSoundSet
    {
        public E_COMMON_SOUND Type;
        public PlaySoundParam Param;
    }

    private class OperateAisacData
    {
        public OperateAisacParam OperateAisacParam;
        public float NowTime;
        public float CurrentAisacValue;

        public OperateAisacData(OperateAisacParam param, float currentAisacValue)
        {
            OperateAisacParam = param;
            CurrentAisacValue = currentAisacValue;
            NowTime = 0;
        }
    }

    #endregion

    #region Field Inspector

    [SerializeField]
    private CriWareInitializer m_CriWareInitializer;

    [SerializeField]
    private SourceSet[] m_SourceSets;

    [SerializeField]
    private CommonSoundSet[] m_CommonSoundSets;

    #endregion

    #region Field

    private AdxAssetParam m_AdxAssetParam;

    private Dictionary<E_CUE_SHEET, CriAtomSource> m_SourceDict;
    private Dictionary<E_COMMON_SOUND, PlaySoundParam> m_CommonSoundDict;
    private Dictionary<E_AISAC_TYPE, string> m_AisacDict;
    private Dictionary<E_AISAC_TYPE, float> m_BgmAisacValueDict;

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
        m_BgmAisacValueDict = new Dictionary<E_AISAC_TYPE, float>();

        foreach (var aisacSet in adxParam.AisacSets)
        {
            m_AisacDict.Add(aisacSet.AisacType, aisacSet.Name);
            m_BgmAisacValueDict.Add(aisacSet.AisacType, 0f);
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

        m_CommonSoundDict = new Dictionary<E_COMMON_SOUND, PlaySoundParam>();
        foreach (var commonSoundSet in m_CommonSoundSets)
        {
            m_CommonSoundDict.Add(commonSoundSet.Type, commonSoundSet.Param);
        }

        m_ProcessingOperateAisacList = new List<OperateAisacData>();
        m_DestroyOperateAisacList = new List<OperateAisacData>();

        var bgm = SaveDataManager.GetFloat("Bgm", 0.5f);
        var se = SaveDataManager.GetFloat("Se", 0.5f);
        SetBgmVolume(bgm);
        SetSeVolume(se);
    }

    public override void OnFinalize()
    {
        m_DestroyOperateAisacList?.Clear();
        m_ProcessingOperateAisacList?.Clear();
        m_CommonSoundDict?.Clear();
        m_SourceDict?.Clear();

        m_DestroyOperateAisacList = null;
        m_ProcessingOperateAisacList = null;
        m_CommonSoundDict = null;
        m_SourceDict = null;

        m_AdxAssetParam = null;

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        foreach (var data in m_ProcessingOperateAisacList)
        {
            var sheet = data.OperateAisacParam.TargetCueSheet;
            if (sheet != E_CUE_SHEET.BGM)
            {
                Debug.LogWarning("現在アニメーションAISACに対応しているのはBGMシートのみです。");
                m_DestroyOperateAisacList.Add(data);
                continue;
            }

            var aisac = data.OperateAisacParam.AisacType;
            var anim = data.OperateAisacParam.AnimationValue;
            var rate = anim.Evaluate(data.NowTime);
            var value = Mathf.Lerp(data.CurrentAisacValue, data.OperateAisacParam.TargetValue, rate);

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
        source.player.SetStartTime(Math.Max(playSoundParam.StartTime, 0));
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
            var aisacValue = GetCurrentBgmAisacValue(operateAisacParam.AisacType);
            m_ProcessingOperateAisacList.Add(new OperateAisacData(operateAisacParam, aisacValue));
        }
        else
        {
            OperateAisac(operateAisacParam.AisacType, operateAisacParam.TargetCueSheet, operateAisacParam.TargetValue);
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

        if (targetSheet == E_CUE_SHEET.BGM)
        {
            SetCurrentBgmAisacValue(targetAisac, value);
        }
    }

    /// <summary>
    /// AISACをリセットする。
    /// </summary>
    public void ResetAisac()
    {
        OperateAisac(E_AISAC_TYPE.AISAC_BGM, E_CUE_SHEET.BGM, 0);
        OperateAisac(E_AISAC_TYPE.AISAC_HACK, E_CUE_SHEET.BGM, 0);
    }

    private float GetCurrentBgmAisacValue(E_AISAC_TYPE targetAisac)
    {
        if (m_AisacDict == null || !m_BgmAisacValueDict.ContainsKey(targetAisac))
        {
            return -1;
        }

        return m_BgmAisacValueDict[targetAisac];
    }

    private void SetCurrentBgmAisacValue(E_AISAC_TYPE targetAisac, float value)
    {
        if (m_AisacDict == null || !m_BgmAisacValueDict.ContainsKey(targetAisac))
        {
            return;
        }

        m_BgmAisacValueDict[targetAisac] = value;
    }

    /// <summary>
    /// 汎用サウンドを再生する。
    /// </summary>
    public void Play(E_COMMON_SOUND type)
    {
        if (m_CommonSoundDict == null)
        {
            Debug.LogWarning("CommonSoundDict is null.");
            return;
        }

        if (m_CommonSoundDict.TryGetValue(type, out PlaySoundParam param))
        {
            Play(param);
        }
        else
        {
            Debug.LogWarningFormat("指定したタイプのサウンドが登録されていませんでした。 type : {0}", type);
        }
    }

    /// <summary>
    /// BGMグループのボリュームを取得する。
    /// </summary>
    public float GetBgmVolume()
    {
        var count = m_AdxAssetParam.BgmCueSheets.Length;
        float volume = 0;
        foreach (var t in m_AdxAssetParam.BgmCueSheets)
        {
            var bgm = GetSource(t);
            volume += bgm.volume;
        }

        if (count > 0)
        {
            volume /= count;
        }

        SetBgmVolume(count);
        return volume;
    }

    /// <summary>
    /// BGMグループのボリュームを設定する。
    /// </summary>
    public void SetBgmVolume(float value)
    {
        foreach (var t in m_AdxAssetParam.BgmCueSheets)
        {
            var bgm = GetSource(t);
            bgm.volume = value;
        }
    }

    /// <summary>
    /// SEグループのボリュームを取得する。
    /// </summary>
    public float GetSeVolume()
    {
        var count = m_AdxAssetParam.SeCueSheets.Length;
        float volume = 0;
        foreach (var t in m_AdxAssetParam.SeCueSheets)
        {
            var se = GetSource(t);
            volume += se.volume;
        }

        if (count > 0)
        {
            volume /= count;
        }

        SetSeVolume(count);
        return volume;
    }

    /// <summary>
    /// SEグループのボリュームを設定する。
    /// </summary>
    public void SetSeVolume(float value)
    {
        foreach (var t in m_AdxAssetParam.SeCueSheets)
        {
            var se = GetSource(t);
            se.volume = value;
        }
    }
}

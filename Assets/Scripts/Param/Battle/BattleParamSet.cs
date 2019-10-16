using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

/// <summary>
/// バトルで使用するパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/Battle", fileName = "param.battle.asset")]
public class BattleParamSet : ScriptableObject
{
    [SerializeField]
    private BattleRealParamSet m_BattleRealParamSet;
    public BattleRealParamSet BattleRealParamSet => m_BattleRealParamSet;

    [SerializeField]
    private BattleHackingParamSet m_BattleHackingParamSet;
    public BattleHackingParamSet BattleHackingParamSet => m_BattleHackingParamSet;

    [SerializeField]
    private Material m_ColliderMaterial;
    public Material ColliderMaterial => m_ColliderMaterial;

    [Header("Transition")]

    [SerializeField]
    private AnimationCurve m_FadeOutVideoParam;
    public AnimationCurve FadeOutVideoParam => m_FadeOutVideoParam;

    [SerializeField]
    private AnimationCurve m_FadeInVideoParam;
    public AnimationCurve FadeInVideoParam => m_FadeInVideoParam;

    [SerializeField]
    private VideoClip m_TransitionToHackingMovie;
    public VideoClip TransitionToHackingMovie => m_TransitionToHackingMovie;

    [SerializeField]
    private VideoClip m_TransitionToRealMovie;
    public VideoClip TransitionToRealMovie => m_TransitionToRealMovie;

    [SerializeField]
    private string m_TransitionToHackingSeName;
    public string TransitionToHackingSeName => m_TransitionToHackingSeName;

    [SerializeField]
    private string m_TransitionToRealSeName;
    public string TransitionToRealSeName => m_TransitionToRealSeName;

    [Header("BGM")]

    [SerializeField]
    private BattleBgmParamSet m_BgmParamSet;
    public BattleBgmParamSet BgmParamSet => m_BgmParamSet;
}

[Serializable]
public class BattleBgmParamSet
{
    [SerializeField]
    private string m_StageBgmName;
    public string StageBgmName => m_StageBgmName;

    [SerializeField]
    private string m_BossBgmName;
    public string BossBgmName => m_BossBgmName;
}
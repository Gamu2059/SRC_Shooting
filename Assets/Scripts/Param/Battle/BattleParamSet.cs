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
    private BattleRealParamSet m_BattleRealParamSet = default;
    public BattleRealParamSet BattleRealParamSet => m_BattleRealParamSet;

    [SerializeField]
    private BattleHackingParamSet m_BattleHackingParamSet = default;
    public BattleHackingParamSet BattleHackingParamSet => m_BattleHackingParamSet;

    [SerializeField]
    private Material m_ColliderMaterial = default;
    public Material ColliderMaterial => m_ColliderMaterial;

    [Header("Transition")]

    [SerializeField]
    private AnimationCurve m_FadeOutVideoParam = default;
    public AnimationCurve FadeOutVideoParam => m_FadeOutVideoParam;

    [SerializeField]
    private AnimationCurve m_FadeInVideoParam = default;
    public AnimationCurve FadeInVideoParam => m_FadeInVideoParam;

    [SerializeField]
    private VideoClip m_TransitionToHackingMovie = default;
    public VideoClip TransitionToHackingMovie => m_TransitionToHackingMovie;

    [SerializeField]
    private VideoClip m_TransitionToRealMovie = default;
    public VideoClip TransitionToRealMovie => m_TransitionToRealMovie;

    [SerializeField]
    private string m_TransitionToHackingSeName = default;
    public string TransitionToHackingSeName => m_TransitionToHackingSeName;

    [SerializeField]
    private string m_TransitionToRealSeName = default;
    public string TransitionToRealSeName => m_TransitionToRealSeName;

    [Header("BGM")]

    [SerializeField]
    private BattleBgmParamSet m_BgmParamSet = default;
    public BattleBgmParamSet BgmParamSet => m_BgmParamSet;
}

[Serializable]
public class BattleBgmParamSet
{
    [SerializeField]
    private string m_StageBgmName = default;
    public string StageBgmName => m_StageBgmName;

    [SerializeField]
    private string m_BossBgmName = default;
    public string BossBgmName => m_BossBgmName;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

/// <summary>
/// バトルで使用するパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleParamSet", fileName = "param.battle_param_set")]
public class BattleParamSet : ScriptableObject
{
    [SerializeField]
    private BattleRealParamSet m_BattleRealParamSet;
    public BattleRealParamSet BattleRealParamSet => m_BattleRealParamSet;

    [SerializeField]
    private BattleHackingParamSet m_BattleHackingParamSet;
    public BattleHackingParamSet BattleHackingParamSet => m_BattleHackingParamSet;

    [Header("Transition")]

    [SerializeField]
    private AnimationCurve m_FadeOutVideoParam;
    public AnimationCurve FadeOutVideoParam => m_FadeOutVideoParam;

    [SerializeField]
    private AnimationCurve m_FadeInVideoParam;
    public AnimationCurve FadeInVideoParam => m_FadeInVideoParam;

    [SerializeField]
    private AnimationCurve m_FadeOutBgmParam;
    public AnimationCurve FadeOutBgmParam => m_FadeOutBgmParam;

    [SerializeField]
    private AnimationCurve m_FadeInBgmParam;
    public AnimationCurve FadeInBgmParam => m_FadeInBgmParam;

    [SerializeField]
    private VideoClip m_TransitionToHackingMovie;
    public VideoClip TransitionToHackingMovie => m_TransitionToHackingMovie;

    [SerializeField]
    private VideoClip m_TransitionToRealMovie;
    public VideoClip TransitionToRealMovie => m_TransitionToRealMovie;

    [SerializeField]
    private AudioClip m_TransitionToHackingSe;
    public AudioClip TransitionToHackingSe => m_TransitionToHackingSe;

    [SerializeField]
    private AudioClip m_TransitionToRealSe;
    public AudioClip TransitionToRealSe => m_TransitionToRealSe;

    [Header("BGM")]

    [SerializeField]
    private BattleBossBgmParamSet m_BossBgmParamSet;
    public BattleBossBgmParamSet BossBgmParamSet => m_BossBgmParamSet;
}

[Serializable]
public class BattleBossBgmParamSet
{
    [SerializeField]
    private AudioClip m_RealModeIntro;
    public AudioClip RealModeIntro => m_RealModeIntro;

    [SerializeField]
    private AudioClip m_RealModeLoop;
    public AudioClip RealModeLoop => m_RealModeLoop;

    [SerializeField]
    private AudioClip m_HackingModeIntro;
    public AudioClip HackingModeIntro => m_HackingModeIntro;

    [SerializeField]
    private AudioClip m_HackingModeLoop;
    public AudioClip HackingModeLoop => m_HackingModeLoop;
}
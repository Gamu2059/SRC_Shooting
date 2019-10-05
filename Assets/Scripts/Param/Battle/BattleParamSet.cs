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
    private AnimationCurve m_FadeOutParam;
    public AnimationCurve FadeOutParam => m_FadeOutParam;

    [SerializeField]
    private AnimationCurve m_FadeInParam;
    public AnimationCurve FadeInParam => m_FadeInParam;

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
    private AudioClip m_RealModeBgm;
    public AudioClip RealModeBgm => m_RealModeBgm;

    [SerializeField]
    private AudioClip m_HackingModeBgm;
    public AudioClip HackingModeBgm => m_HackingModeBgm;
}

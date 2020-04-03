#pragma warning disable 0649

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

    [Header("Transition")]

    [SerializeField]
    private AnimationCurve m_FadeOutVideoParam = default;
    public AnimationCurve FadeOutVideoParam => m_FadeOutVideoParam;

    [SerializeField]
    private AnimationCurve m_FadeInVideoParam = default;
    public AnimationCurve FadeInVideoParam => m_FadeInVideoParam;

    //[SerializeField]
    //private VideoClip m_ToHackingMovie = default;
    //public VideoClip ToHackingMovie => m_ToHackingMovie;

    [SerializeField]
    private VideoClip m_ToRealMovie = default;
    public VideoClip ToRealMovie => m_ToRealMovie;
}

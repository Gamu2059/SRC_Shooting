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
    private VideoClip m_ToHackingMovie = default;
    public VideoClip ToHackingMovie => m_ToHackingMovie;

    [SerializeField]
    private VideoClip m_ToRealMovie = default;
    public VideoClip ToRealMovie => m_ToRealMovie;

    [SerializeField]
    private PlaySoundParam m_ToHackingSe;
    public PlaySoundParam ToHackingSe => m_ToHackingSe;

    [SerializeField]
    private PlaySoundParam m_ToRealSe;
    public PlaySoundParam ToRealSe => m_ToRealSe;

    [SerializeField]
    private OperateAisacParam m_ToHackingAisac;
    public OperateAisacParam ToHackingAisac => m_ToHackingAisac;

    [SerializeField]
    private OperateAisacParam m_ToRealAisac;
    public OperateAisacParam ToRealAisac => m_ToRealAisac;

    [Header("Game Clear")]

    [SerializeField]
    private PlaySoundParam m_GameClearSe;
    public PlaySoundParam GameClearSe => m_GameClearSe;

    [Header("Game Over")]

    [SerializeField]
    private PlaySoundParam m_GameOverSe;
    public PlaySoundParam GameOverSe => m_GameOverSe;
}

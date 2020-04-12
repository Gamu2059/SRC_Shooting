#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

/// <summary>
/// バトルで使用するパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleCommon/Manager/Battle", fileName = "param.battle.asset")]
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
    private VideoClip m_ToRealMovie;
    public VideoClip ToRealMovie => m_ToRealMovie;
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードのPlayerManagerのパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/BattleHackingPlayer", fileName = "param.battle_hacking_player.asset")]
public class BattleHackingPlayerManagerParamSet : ScriptableObject
{
    [SerializeField, Tooltip("プレイヤーのプレハブ")]
    private BattleHackingPlayerController m_PlayerPrefab;
    public BattleHackingPlayerController PlayerPrefab => m_PlayerPrefab;

    [SerializeField, Tooltip("プレイヤーの基本移動速度")]
    private float m_PlayerBaseMoveSpeed;
    public float PlayerBaseMoveSpeed => m_PlayerBaseMoveSpeed;

    [SerializeField, Tooltip("プレイヤーの低速移動速度")]
    private float m_PlayerSlowMoveSpeed;
    public float PlayerSlowMoveSpeed => m_PlayerSlowMoveSpeed;

    [Header("Effect")]

    [SerializeField]
    private EffectParamSet m_DeadEffectParam;
    public EffectParamSet DeadEffectParam => m_DeadEffectParam;

    [Header("SE"), Tooltip("いずれ各々の行動のパラメータに分散させたい")]

    [SerializeField]
    private PlaySoundParam m_ShotSe;
    public PlaySoundParam ShotSe => m_ShotSe;
}

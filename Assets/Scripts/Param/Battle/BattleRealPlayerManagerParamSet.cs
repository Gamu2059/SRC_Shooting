#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのPlayerManagerのパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/BattleRealPlayer", fileName = "param.battle_real_player.asset")]
public class BattleRealPlayerManagerParamSet : ScriptableObject
{
    [SerializeField, Tooltip("プレイヤーのプレハブ")]
    private BattleRealPlayerController m_PlayerPrefab;
    public BattleRealPlayerController PlayerPrefab => m_PlayerPrefab;

    [SerializeField, Tooltip("ゲーム開始時のプレイヤーの最初の位置")]
    private Vector2 m_InitAppearViewportPosition;
    public Vector2 InitAppearViewportPosition => m_InitAppearViewportPosition;

    [SerializeField, Tooltip("プレイヤーの基本移動速度")]
    private float m_PlayerBaseMoveSpeed;
    public float PlayerBaseMoveSpeed => m_PlayerBaseMoveSpeed;

    [SerializeField, Tooltip("プレイヤーの低速移動速度")]
    private float m_PlayerSlowMoveSpeed;
    public float PlayerSlowMoveSpeed => m_PlayerSlowMoveSpeed;

    [SerializeField]
    private bool m_IsLaserType;
    public bool IsLaserType => m_IsLaserType;

    [SerializeField, Tooltip("チャージエフェクト")]
    private EffectParamSet m_ChargeEffectParam;
    public EffectParamSet ChargeEffectParam => m_ChargeEffectParam;

    [Header("カメラウェーブ")]

    [SerializeField]
    private CameraShakeParam m_LaserShakeParam;
    public CameraShakeParam LaserShakeParam => m_LaserShakeParam;

    [SerializeField]
    private CameraShakeParam m_BombShakeParam;
    public CameraShakeParam BombShakeParam => m_BombShakeParam;

    [Header("SE"), Tooltip("いずれ各々の行動のパラメータに分散させたい")]

    [SerializeField]
    private PlaySoundParam m_ShotSe;
    public PlaySoundParam ShotSe => m_ShotSe;

    [SerializeField]
    private PlaySoundParam m_LaserSe;
    public PlaySoundParam LaserSe => m_LaserSe;

    [SerializeField]
    private PlaySoundParam m_BombSe;
    public PlaySoundParam BombSe => m_BombSe;

    [SerializeField]
    private PlaySoundParam m_ChargeSe;
    public PlaySoundParam ChargeSe => m_ChargeSe;

    [SerializeField]
    private PlaySoundParam m_WeaponChangeSe;
    public PlaySoundParam WeaponChangeSe => m_WeaponChangeSe;

    [SerializeField]
    private PlaySoundParam m_GetItemSe;
    public PlaySoundParam GetItemSe => m_GetItemSe;

    [SerializeField]
    private PlaySoundParam m_DeadSe;
    public PlaySoundParam DeadSe => m_DeadSe;
}

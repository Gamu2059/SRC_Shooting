#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BattleReal.BulletGenerator;

/// <summary>
/// リアルモードのPlayerManagerのパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Manager/BattleRealPlayer", fileName = "param.battle_real_player.asset")]
public class BattleRealPlayerManagerParamSet : ScriptableObject
{
    [SerializeField, Tooltip("プレイヤーのプレハブ")]
    private BattleRealPlayerController m_PlayerPrefab;
    public BattleRealPlayerController PlayerPrefab => m_PlayerPrefab;

    [SerializeField, Tooltip("ゲーム開始時のプレイヤーの最初の位置")]
    private Vector2 m_InitAppearViewportPosition;
    public Vector2 InitAppearViewportPosition => m_InitAppearViewportPosition;

    [SerializeField, Tooltip("プレイヤー復活時のシーケンス")]
    private SequenceGroup m_RespawnSequence;
    public SequenceGroup RespawnSequence => m_RespawnSequence;

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
    private EffectParamSet[] m_ChargeEffectParams;
    public EffectParamSet[] ChargeEffectParams => m_ChargeEffectParams;

    [SerializeField, Tooltip("シールドエフェクト")]
    private EffectParamSet m_ShieldEffectParam;
    public EffectParamSet ShieldEffectParam => m_ShieldEffectParam;

    [SerializeField, Tooltip("死亡エフェクト")]
    private EffectParamSet m_DeadEffectParam;
    public EffectParamSet DeadEffectParam => m_DeadEffectParam;

    [Header("弾ジェネレータ")]

    [SerializeField, Tooltip("通常弾ジェネレータのパラメータ")]
    private PlayerNormalBulletGeneratorParamSet m_NormalBulletGeneratorParamSet;
    public PlayerNormalBulletGeneratorParamSet NormalBulletGeneratorParamSet => m_NormalBulletGeneratorParamSet;

    [SerializeField, Tooltip("レーザージェネレータのパラメータ")]
    private PlayerLaserGeneratorParamSet m_LaserGeneratorParamSet;
    public PlayerLaserGeneratorParamSet LaserGeneratorParamSet => m_LaserGeneratorParamSet;

    [SerializeField, Tooltip("ボムジェネレータのパラメータ")]
    private PlayerBombGeneratorParamSet m_BombGeneratorParamSet;
    public PlayerBombGeneratorParamSet BombGeneratorParamSet => m_BombGeneratorParamSet;

    [Header("カメラウェーブ")]

    [SerializeField]
    private CameraShakeParam m_LaserShakeParam;
    public CameraShakeParam LaserShakeParam => m_LaserShakeParam;

    [SerializeField]
    private CameraShakeParam m_BombShakeParam;
    public CameraShakeParam BombShakeParam => m_BombShakeParam;
}

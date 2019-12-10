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

    [Header("チャージエフェクト関連")]

    [SerializeField, Tooltip("チャージエフェクト")]
    private BattleRealEffectController m_ChargePrefab;
    public BattleRealEffectController ChargePrefab => m_ChargePrefab;

    [SerializeField, Tooltip("チャージエフェクトの発生相対位置")]
    private Vector3 m_ChargeRelatedPos;
    public Vector3 ChargeRelatedPos => m_ChargeRelatedPos;

    [Header("バックウェーブ関連")]

    [SerializeField]
    private BattleRealEffectController m_BackWavePrefab;
    public BattleRealEffectController BackWavePrefab => m_BackWavePrefab;

    [SerializeField, Tooltip("バックウェーブの発生相対位置")]
    private Vector3 m_BackWaveRelatedPos;
    public Vector3 BackWaveRelatedPos => m_BackWaveRelatedPos;

    [Header("カメラウェーブ")]

    [SerializeField]
    private CameraShakeParam m_CameraShakeParam;
    public CameraShakeParam CameraShakeParam => m_CameraShakeParam;
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのプレイヤーのレベルのパラメータセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/PlayerLevel", fileName = "param.player_level.asset")]
public class BattleRealPlayerLevelParamSet : ScriptableObject
{
    [SerializeField]
    private BattleRealPlayerLevel[] m_PlayerLevels;

    public BattleRealPlayerLevel[] PlayerLevels => m_PlayerLevels;

    [SerializeField]
    private BattleCommonDefData m_CommonDefData;
    public BattleCommonDefData CommonDefData => m_CommonDefData;

    [SerializeField]
    private BattleInitData m_Stage0InitData;
    public BattleInitData Stage0InitData => m_Stage0InitData;

    [SerializeField]
    private BattleInitData m_Stage1InitData;
    public BattleInitData Stage1InitData => m_Stage1InitData;
}

/// <summary>
/// 特定のプレイヤーのレベルに関するパラメータ。
/// </summary>
[Serializable]
public class BattleRealPlayerLevel
{
    [SerializeField, Tooltip("次のレベルまでに必要な経験値")]
    private int m_NecessaryExpToLevelUpNextLevel;

    public int NecessaryExpToLevelUpNextLevel => m_NecessaryExpToLevelUpNextLevel;

    [SerializeField, Tooltip("レーザータイプでの通常弾1発のダメージ")]
    private float m_LaserTypeShotDamage;
    public float LaserTypeShotDamage => m_LaserTypeShotDamage;

    [SerializeField, Tooltip("レーザータイプでの通常弾1発のダウンダメージ")]
    private float m_LaserTypeShotDownDamage;
    public float LaserTypeShotDownDamage => m_LaserTypeShotDownDamage;

    [SerializeField, Tooltip("ボムタイプでの通常弾1発のダメージ")]
    private float m_BombTypeShotDamage;
    public float BombTypeShotDamage => m_BombTypeShotDamage;

    [SerializeField, Tooltip("ボムタイプでの通常弾1発のダウンダメージ")]
    private float m_BombTypeShotDownDamage;
    public float BombTypeShotDownDamage => m_BombTypeShotDownDamage;

    [SerializeField, Tooltip("レーザーの1秒あたりのダメージ")]
    private float m_LaserDamagePerSeconds;
    public float LaserDamagePerSeconds => m_LaserDamagePerSeconds;

    [SerializeField, Tooltip("ボムの1発あたりのダメージ")]
    private float m_BombDamage;
    public float BombDamage => m_BombDamage;
}

/// <summary>
/// 共有固定パラメータ。
/// バトル中に使用する共通パラメータをまとめたもの。
/// </summary>
[Serializable]
public class BattleCommonDefData
{
    [SerializeField]
    private int m_MaxPlayerLife;
    public int MaxPlayerLifeNum => m_MaxPlayerLife;

    [SerializeField]
    private int m_MaxLevel;
    public int MaxLevel => m_MaxLevel;

    [SerializeField]
    private int m_MaxEnergyCount;
    public int MaxEnergyCount => m_MaxEnergyCount;

    [SerializeField]
    private float m_MaxEnergyCharge;
    public float MaxEnergyCharge => m_MaxEnergyCharge;

    [SerializeField]
    private float m_HackingSuccessBonus;
    public float HackingSuccessBonus => m_HackingSuccessBonus;
}

/// <summary>
/// 初期値パラメータ。
/// ストーリーモードやチャプターモードでのそれぞれの初期値に対応させるために構造化させた。
/// </summary>
[Serializable]
public class BattleInitData
{
    [SerializeField, Tooltip("初期残機数")]
    private int m_InitPlayerLife;
    public int InitPlayerLife => m_InitPlayerLife;

    [SerializeField, Tooltip("初期レベル")]
    private int m_InitLevel;
    public int InitLevel => m_InitLevel;

    [SerializeField, Tooltip("初期エナジー数")]
    private int m_InitEnergyCount;
    public int InitEnergyCount => m_InitEnergyCount;
}

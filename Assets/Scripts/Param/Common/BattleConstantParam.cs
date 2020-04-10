#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのプレイヤーのレベルのパラメータセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleCommon/BattleConstant", fileName = "param.battle_constant.asset")]
public class BattleConstantParam : ScriptableObject
{
    [SerializeField, Tooltip("初期残機数")]
    private int m_InitLife = 2;
    public int InitLife => m_InitLife;

    [SerializeField, Tooltip("最大残機数")]
    private int m_MaxLife = 5;
    public int MaxLife => m_MaxLife;

    [SerializeField, Tooltip("最大レベル")]
    private int m_MaxLevel = 5;
    public int MaxLevel => m_MaxLevel;

    [SerializeField, Tooltip("最大エナジーストック")]
    private int m_MaxEnergy = 5;
    public int MaxEnergy => m_MaxEnergy;

    [SerializeField, Tooltip("プレイヤーのレベルごとのデータ")]
    private BattleRealPlayerLevelData[] m_PlayerLevelDatas;

    public BattleRealPlayerLevelData[] PlayerLevelDatas => m_PlayerLevelDatas;

    [SerializeField, Tooltip("次のエナジーストックまでに必要なエナジーチャージ量")]
    private int[] m_NecessaryEnergyChargeNextStocks;
    public int[] NecessaryEnergyChargeNextStocks => m_NecessaryEnergyChargeNextStocks;
}

/// <summary>
/// プレイヤーのレベルに関するパラメータ。
/// </summary>
[Serializable]
public class BattleRealPlayerLevelData
{
    [SerializeField, Tooltip("次のレベルまでに必要な経験値")]
    private int m_NecessaryExpNextLevel;
    public int NecessaryExpNextLevel => m_NecessaryExpNextLevel;

    [SerializeField, Tooltip("アイテム吸収範囲の相対スケール")]
    private float m_ItemAtractScale;
    public float ItemAtractScale => m_ItemAtractScale;

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

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleCommon/BattleConstant", fileName = "param.battle_constant.asset")]
public class BattleConstantParam : ScriptableObject
{
    [SerializeField, Tooltip("最大残機数")]
    private int m_MaxLife = 5;
    public int MaxLife => m_MaxLife;

    [Header("Level")]

    [SerializeField, Tooltip("最大レベル")]
    private int m_MaxLevel = 5;
    public int MaxLevel => m_MaxLevel;

    [SerializeField, Tooltip("プレイヤーのレベルごとのデータ")]
    private BattleRealPlayerLevelData[] m_PlayerLevelDatas;

    public BattleRealPlayerLevelData[] PlayerLevelDatas => m_PlayerLevelDatas;
    
    [Header("Energy")]

    [SerializeField, Tooltip("最大エナジーストック")]
    private int m_MaxEnergy = 5;
    public int MaxEnergy => m_MaxEnergy;

    [SerializeField, Tooltip("次のエナジーストックまでに必要なエナジーチャージ量")]
    private int[] m_NecessaryEnergyChargeNextStocks;
    public int[] NecessaryEnergyChargeNextStocks => m_NecessaryEnergyChargeNextStocks;

    [Header("Charge Shot")]
    
    [SerializeField, Tooltip("チャージショットの最大強化レベル")]
    private int m_MaxChargeLevel = 3;
    public int MaxChargeLevel => m_MaxChargeLevel;

    [SerializeField, Tooltip("チャージのレベルごとのデータ")]
    private BattleRealChargeShotLevelData[] m_ChargeLevelDatas;
    public BattleRealChargeShotLevelData[] ChargeLevelDatas => m_ChargeLevelDatas;
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

    [SerializeField]
    private float m_MainShotDamage;
    public float MainShotDamage => m_MainShotDamage;

    [SerializeField]
    private float m_MainShotDownDamage;
    public float MainShotDownDamage => m_MainShotDownDamage;

    [SerializeField]
    private float m_SideShotDamage;
    public float SideShotDamage => m_SideShotDamage;

    [SerializeField]
    private float m_SideShotDownDamage;
    public float SideShotDownDamage => m_SideShotDownDamage;
}

/// <summary>
/// チャージショットのレベルに関するパラメータ。
/// </summary>
[Serializable]
public class BattleRealChargeShotLevelData
{
    [SerializeField, Tooltip("次のチャージ強化レベルまでに必要なチャージ時間")]
    private float m_ChargeTimeNextLevel;
    public float ChargeTimeNextLevel => m_ChargeTimeNextLevel;

    [SerializeField, Tooltip("レーザーの1秒あたりのダメージ")]
    private float m_LaserDamagePerSeconds;
    public float LaserDamagePerSeconds => m_LaserDamagePerSeconds;

    [SerializeField, Tooltip("ボムの1発あたりのダメージ")]
    private float m_BombDamage;
    public float BombDamage => m_BombDamage;

    [SerializeField, Tooltip("弾消し時のスコア")]
    private int m_RemoveBulletScore;
    public int RemoveBulletScore => m_RemoveBulletScore;
}

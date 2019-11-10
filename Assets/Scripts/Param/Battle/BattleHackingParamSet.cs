﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードで使用するパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/BattleHacking", fileName = "param.battle_hacking.asset")]
public class BattleHackingParamSet : ScriptableObject
{
    [SerializeField]
    private BattleHackingPlayerManagerParamSet m_PlayerManagerParamSet;
    public BattleHackingPlayerManagerParamSet PlayerManagerParamSet => m_PlayerManagerParamSet;

    [SerializeField]
    private BattleHackingEnemyManagerParamSet m_EnemyManagerParamSet;
    public BattleHackingEnemyManagerParamSet EnemyManagerParamSet => m_EnemyManagerParamSet;

    [SerializeField]
    private BattleHackingBulletManagerParamSet m_BulletManagerParamSet;
    public BattleHackingBulletManagerParamSet BulletManagerParamSet => m_BulletManagerParamSet;

    [SerializeField]
    private BattleHackingLevelParamSet[] m_LevelParamSets;
    public BattleHackingLevelParamSet[] LevelParamSets => m_LevelParamSets;
}

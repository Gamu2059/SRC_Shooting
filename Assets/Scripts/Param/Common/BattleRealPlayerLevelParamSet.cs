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
}
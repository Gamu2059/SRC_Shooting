#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのEnemyGroupManagerのパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/BattleRealEnemyGroup", fileName = "param.battle_real_enemy_group.asset")]
public class BattleRealEnemyGroupManagerParamSet : ScriptableObject
{
    [SerializeField, Tooltip("敵グループの生成パラメータ")]
    private BattleRealEnemyGroupGenerator m_Generator;
    public BattleRealEnemyGroupGenerator Generator => m_Generator;
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードのボスの基本的なパラメータのセット
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Enemy/BehaviorParam", fileName = "param.behavior.asset")]
public class BattleHackingBossBehaviorParamSet : BattleHackingEnemyBehaviorParamSet
{
    [SerializeField, Tooltip("攻撃の行動パターンの配列")]
    private BattleHackingBossBehaviorUnitParamSet[] m_BehaviorParamSets;
    public BattleHackingBossBehaviorUnitParamSet[] BehaviorParamSets => m_BehaviorParamSets;

    [SerializeField, Tooltip("死亡時の行動パターン")]
    private BattleHackingBossBehaviorUnitParamSet m_DeadBehaviorParamSet;
    public BattleHackingBossBehaviorUnitParamSet DeadBehaviorParamSet => m_DeadBehaviorParamSet;
}

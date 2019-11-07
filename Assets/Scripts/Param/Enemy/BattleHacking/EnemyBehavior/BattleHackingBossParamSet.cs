using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードのボスの基本的なパラメータのセット
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Boss/BaseBehavior", fileName = "param.hacking_boss_base_behavior.asset")]
public class BattleHackingBossParamSet : BattleHackingEnemyBehaviorParamSet
{
    [SerializeField, Tooltip("攻撃の行動パターンの配列")]
    private BattleHackingBossBehaviorParamSet[] m_BehaviorParamSets;
    public BattleHackingBossBehaviorParamSet[] BehaviorParamSets => m_BehaviorParamSets;
}

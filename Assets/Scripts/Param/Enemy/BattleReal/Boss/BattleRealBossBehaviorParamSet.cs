#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスの基本的なパラメータのセット
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Boss/BaseBehavior", fileName = "param.real_boss_base_behavior.asset")]
public class BattleRealBossBehaviorParamSet : BattleRealEnemyBehaviorParamSet
{
    [SerializeField, Tooltip("攻撃の行動パターンの配列")]
    private BattleRealBossBehaviorUnitParamSet[] m_AttackParamSets;
    public BattleRealBossBehaviorUnitParamSet[] AttackParamSets => m_AttackParamSets;

    [SerializeField, Tooltip("ダウンの行動パターンの配列")]
    private BattleRealBossBehaviorUnitParamSet[] m_DownParamSets;
    public BattleRealBossBehaviorUnitParamSet[] DownParamSets => m_DownParamSets;
}

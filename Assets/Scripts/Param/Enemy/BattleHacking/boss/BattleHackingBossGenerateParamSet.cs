#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードのボスの生成パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Boss/BossGenerate", fileName = "param.battle_hacking_boss_generate.asset")]
public class BattleHackingBossGenerateParamSet : BattleHackingEnemyGenerateParamSet
{
    [Header("エフェクト")]

    [SerializeField]
    private EffectParamSet m_DeadEffectParam;
    public EffectParamSet DeadEffectParam => m_DeadEffectParam;
}
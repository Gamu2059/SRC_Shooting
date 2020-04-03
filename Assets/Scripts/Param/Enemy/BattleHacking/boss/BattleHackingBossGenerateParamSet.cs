#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードのボスの生成パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Enemy/BossParamSet", fileName = "param.boss.asset")]
public class BattleHackingBossGenerateParamSet : BattleHackingEnemyGenerateParamSet
{
    [Header("エフェクト")]

    [SerializeField]
    private EffectParamSet m_DeadEffectParam;
    public EffectParamSet DeadEffectParam => m_DeadEffectParam;
}
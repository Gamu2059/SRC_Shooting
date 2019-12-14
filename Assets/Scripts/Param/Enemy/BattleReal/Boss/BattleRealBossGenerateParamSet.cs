#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスの生成パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Boss/BossGenerate", fileName = "param.battle_real_boss_generate.asset")]
public class BattleRealBossGenerateParamSet : BattleRealEnemyGenerateParamSet
{
    [Header("ボス ダウン")]

    [SerializeField]
    private EffectParamSet m_DownEffectParam;
    public EffectParamSet DownEffectParam => m_DownEffectParam;
}

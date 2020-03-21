#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// リアルモードのボスのパラメータセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/BossParamSet", fileName = "param.battle_real_boss.asset")]
public class BattleRealBossParamSet : BattleRealEnemyParamSetBase
{
    [Serializable]
    private class Set
    {
        public E_DIFFICULTY Difficulty;
        public BattleRealBossParam Param;
    }

    [SerializeField]
    private Set[] m_Params;

    public override BattleRealEnemyParamBase GetEnemyParam(E_DIFFICULTY difficulty)
    {
        var set = m_Params.First(s => s.Difficulty == difficulty);
        if (set != null)
        {
            return set.Param;
        }

        return null;
    }
}

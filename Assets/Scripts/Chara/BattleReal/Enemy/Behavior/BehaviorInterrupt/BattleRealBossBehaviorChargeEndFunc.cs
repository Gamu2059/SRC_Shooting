#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ボスのチャージ終了による割り込み終了オプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemySequence/Interrupt/BossChargeEnd", fileName = "boss_charge_end.behavior_interrupt.asset", order = 1000)]
public class BattleRealBossBehaviorChargeEndFunc : BattleRealEnemyBehaviorInterruptEndFunc
{
    public override bool IsInterruptEnd(BattleRealEnemyBase enemy, BattleRealEnemyBehaviorUnit unit)
    {
        if (enemy is BattleRealBossController boss)
        {
            return !boss.IsCharging;
        }

        return false;
    }
}

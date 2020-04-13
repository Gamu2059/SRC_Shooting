#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ボスのチャージ終了による割り込み終了オプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Interrupt/BossChargeEnd", fileName = "param.behavior_interrupt.asset")]
public class BattleRealBossBehaviorChargeEndFunc : BattleRealEnemyBehaviorInterruptFuncBase
{
    public override bool IsInterruptEnd(BattleRealEnemyBase enemy, BattleRealEnemyBehaviorUnitBase unit)
    {
        if (enemy is BattleRealBossController boss)
        {
            return !boss.IsCharging;
        }

        return false;
    }
}

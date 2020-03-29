#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Eventによる割り込み終了オプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemySequence/Interrupt/BattleRealEvent", fileName = "battle_real_event.behavior_interrupt.asset")]
public class BattleRealEnemyBehaviorEventEndFunc : BattleRealEnemyBehaviorInterruptEndFunc
{
    [SerializeField]
    private EventTriggerRootCondition m_InterruptEndCondition;

    public override bool IsInterruptEnd(BattleRealEnemyBase enemy, BattleRealEnemyBehaviorUnit unit)
    {
        if (BattleRealEventManager.Instance == null)
        {
            return false;
        }

        return BattleRealEventManager.Instance.IsMeetRootCondition(ref m_InterruptEndCondition);
    }
}

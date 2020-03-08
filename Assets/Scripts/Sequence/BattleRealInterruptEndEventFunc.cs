#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Eventによる割り込み終了オプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Interrupt/BattleRealEvent", fileName = "battle_real_event.sequence_interrupt.asset")]
public class BattleRealInterruptEndEventFunc : SequenceInterruptEndFunc
{
    [SerializeField]
    private EventTriggerRootCondition m_InterruptEndCondition;

    public override bool IsInterruptEnd(Transform target, SequenceController controller)
    {
        if (BattleRealEventManager.Instance == null)
        {
            return false;
        }

        return BattleRealEventManager.Instance.IsMeetRootCondition(ref m_InterruptEndCondition);
    }
}

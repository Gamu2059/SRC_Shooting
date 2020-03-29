#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Eventを呼ぶためのオプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Option/BattleRealEvent", fileName = "battle_real_event.sequence_option.asset")]
public class BattleRealSequenceEventFunc : SequenceOptionFunc
{
    [SerializeField]
    private BattleRealEventContent m_CallEvent;

    public override void Call(Transform transform)
    {
        if (BattleRealEventManager.Instance == null)
        {
            return;
        }

        BattleRealEventManager.Instance.ExecuteEvent(m_CallEvent);
    }
}

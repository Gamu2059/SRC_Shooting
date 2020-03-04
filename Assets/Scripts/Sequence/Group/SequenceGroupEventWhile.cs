#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// イベントを介してループするグループ。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Group/EventWhile", fileName = "event_while.sequence_group.asset")]

public class SequenceGroupEventWhile : SequenceGroup
{
    [Header("Loop Parameter")]

    [SerializeField]
    private EventTriggerRootCondition m_RoopOutCondition;

    public override bool IsEndGroup()
    {
        if (BattleRealEventManager.Instance == null)
        {
            return true;
        }
        return BattleRealEventManager.Instance.IsMeetRootCondition(ref m_RoopOutCondition);
    }
}

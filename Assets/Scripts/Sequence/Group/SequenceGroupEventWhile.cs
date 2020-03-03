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
    [SerializeField]
    private EventTriggerRootCondition m_RoopOutCondition;

    [SerializeField]
    private BattleRealEventContent m_OnStartEvent;

    [SerializeField]
    private BattleRealEventContent m_OnLoopedEvent;

    [SerializeField]
    private BattleRealEventContent m_OnEndEvent;

    public override void OnStart()
    {
        base.OnStart();

        if (BattleRealEventManager.Instance != null)
        {
            BattleRealEventManager.Instance.ExecuteEvent(m_OnStartEvent);
        }
    }

    public override void OnLooped()
    {
        base.OnLooped();

        if (BattleRealEventManager.Instance != null)
        {
            BattleRealEventManager.Instance.ExecuteEvent(m_OnLoopedEvent);
        }
    }

    public override void OnEnd()
    {
        base.OnEnd();

        if (BattleRealEventManager.Instance != null)
        {
            BattleRealEventManager.Instance.ExecuteEvent(m_OnEndEvent);
        }
    }

    public override bool IsEnd()
    {
        if (BattleRealEventManager.Instance == null)
        {
            return true;
        }
        return BattleRealEventManager.Instance.IsMeetRootCondition(ref m_RoopOutCondition);
    }
}

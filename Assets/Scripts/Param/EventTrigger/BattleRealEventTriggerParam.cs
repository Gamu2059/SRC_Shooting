using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/EventTrigger", fileName = "param.battle_real_event_trigger.asset")]
public class BattleRealEventTriggerParam : ScriptableObject
{
    public string EventTriggerName;
    public EventTriggerRootCondition Condition;
    public BattleRealEventContent[] Contents;
}

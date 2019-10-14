using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// イベントマネージャで使用するパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/EventTrigger", fileName = "param.battle_real_event.asset")]
public class BattleRealEventTriggerParamSet : ScriptableObject
{
    [Serializable]
    public struct EventTriggerParam
    {
        public EventTriggerRootCondition Condition;
        public BattleRealEventContent[] Contents;
    }

    /// <summary>
    /// イベント変数の配列
    /// </summary>
    [SerializeField]
    private EventTriggerVariable[] m_Variables;
    public EventTriggerVariable[] Variables => m_Variables;

    /// <summary>
    /// タイムピリオドの配列
    /// </summary>
    [SerializeField]
    private string[] m_TimePeriodNames;
    public string[] TimePeriodNames => m_TimePeriodNames;

    /// <summary>
    /// イベントの発動条件とその内容の配列
    /// </summary>
    [SerializeField]
    private EventTriggerParam[] m_Params;
    public EventTriggerParam[] Params => m_Params;
}

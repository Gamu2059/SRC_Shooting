using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// イベントマネージャで使用するパラメータのセット。
/// </summary>
[System.Serializable, CreateAssetMenu(menuName = "Param/EventTriggerParamSet", fileName = "EventTriggerParamSet")]
public class EventTriggerParamSet : ScriptableObject
{
    [Serializable]
    public struct EventTriggerParam
    {
        public EventTriggerCondition Condition;
        public EventContent[] Contents;
    }

    /// <summary>
    /// イベント変数の配列
    /// </summary>
    [SerializeField]
    private EventTriggerVariable[] m_Variables;

    /// <summary>
    /// タイムピリオドの配列
    /// </summary>
    [SerializeField]
    private string[] m_TimePeriodNames;

    /// <summary>
    /// イベントの発動条件とその内容の配列
    /// </summary>
    [SerializeField]
    private EventTriggerParam[] m_Params;

    public EventTriggerVariable[] GetVariables()
    {
        return m_Variables;
    }

    public string[] GetTimePeriodNames()
    {
        return m_TimePeriodNames;
    }

    public EventTriggerParam[] GetParams()
    {
        return m_Params;
    }
}

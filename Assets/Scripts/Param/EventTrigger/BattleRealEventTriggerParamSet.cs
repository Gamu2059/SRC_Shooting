﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// イベントマネージャで使用するパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Event/EventTriggerParamSet", fileName = "param.event.asset")]
public class BattleRealEventTriggerParamSet : ScriptableObject
{
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
    private BattleRealEventTriggerParam[] m_Params;
    public BattleRealEventTriggerParam[] Params => m_Params;
}

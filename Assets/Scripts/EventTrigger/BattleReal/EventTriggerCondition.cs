using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum E_VARIABLE_TYPE
{
    INT,
    FLOAT,
    BOOL,
    TIME_PERIOD,
}

[Serializable]
public enum E_COMPARE_TYPE
{
    EQUAL,
    NOT_EQUAL,
    LESS_THAN,
    LESS_THAN_EQUAL,
    MORE_THAN,
    MORE_THAN_EQUAL,
}

[Serializable]
public enum E_BOOL_COMPARE_TYPE
{
    EQUAL,
    NOT_EQUAL,
}

[Serializable]
public enum E_MULTI_CONDITION_TYPE
{
    OR,
    AND,
}

[Serializable]
public enum E_GENERAL_INT_VARIABLE
{
    BOSS_DEFEAT,
    BOSS_RESCUE,
}

[Serializable]
public enum E_GENERAL_TIME_PERIOD
{
    BATTLE_LOADED,
    GAME_START,
}

/// <summary>
/// イベントトリガを発動させるための条件
/// </summary>
[Serializable]
public class EventTriggerCondition
{
    /// <summary>
    /// 判定結果を逆にするかどうか。否定論理演算。
    /// </summary>
    public bool IsReverse;

    public E_VARIABLE_TYPE VariableType;

    public bool UseGeneralIntVariable;

    public E_GENERAL_INT_VARIABLE GeneralIntVariable;

    public bool UseGeneralTimePeriod;

    public E_GENERAL_TIME_PERIOD GeneralTimePeriod;

    public string VariableName;

    public E_COMPARE_TYPE CompareType;

    public E_BOOL_COMPARE_TYPE BoolCompareType;

    public float CompareValue;

    public bool BoolCompareValue;
}

/// <summary>
/// イベントトリガを発動させるルート条件
/// </summary>
[Serializable]
public class EventTriggerRootCondition
{
    /// <summary>
    /// 判定結果を逆にするかどうか。否定論理演算。
    /// </summary>
    public bool IsReverse;

    /// <summary>
    /// 複合条件かどうか
    /// </summary>
    public bool IsMultiCondition;

    public EventTriggerCondition SingleCondition;

    public E_MULTI_CONDITION_TYPE MultiConditionType;

    public EventTriggerCondition[] MultiConditions;
}

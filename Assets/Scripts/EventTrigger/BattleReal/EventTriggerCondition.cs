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

/// <summary>
/// イベントトリガを発動させるための条件
/// </summary>
[Serializable]
public struct EventTriggerCondition
{
    /// <summary>
    /// 判定結果を逆にするかどうか。否定論理演算。
    /// </summary>
    public bool IsReverse;

    public E_VARIABLE_TYPE VariableType;

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
public struct EventTriggerRootCondition
{
    /// <summary>
    /// 判定結果を逆にするかどうか。否定論理演算。
    /// </summary>
    public bool IsReverse;

    /// <summary>
    /// 単体条件かどうか
    /// </summary>
    public bool IsSingleCondition;

    public EventTriggerCondition RootCondition;

    public E_MULTI_CONDITION_TYPE MultiConditionType;

    public EventTriggerCondition[] SubConditions;
}

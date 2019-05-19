using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// イベントトリガを発動させる条件
/// </summary>
[Serializable]
public struct EventTriggerCondition
{
    public enum VARIABLE_TYPE
    {
        INT,
        FLOAT,
        BOOL,
        TIME_PERIOD,
    }

    public enum COMPARE_TYPE
    {
        EQUAL,
        NOT_EQUAL,
        LESS_THAN,
        LESS_THAN_EQUAL,
        MORE_THAN,
        MORE_THAN_EQUAL,
    }

    public enum BOOL_COMPARE_TYPE
    {
        EQUAL,
        NOT_EQUAL,
    }

    public enum MULTI_CONDITION_TYPE
    {
        OR,
        AND,
    }

    /// <summary>
    /// 判定結果を逆にするかどうか。否定論理演算。
    /// </summary>
    public bool IsReverse;

    /// <summary>
    /// 単体条件かどうか
    /// </summary>
    public bool IsSingleCondition;

    public VARIABLE_TYPE VariableType;

    public string VariableName;

    public COMPARE_TYPE CompareType;

    public BOOL_COMPARE_TYPE BoolCompareType;

    public float CompareValue;

    public bool BoolCompareValue;

    public MULTI_CONDITION_TYPE MultiConditionType;

    public EventTriggerCondition[] Conditions;
}

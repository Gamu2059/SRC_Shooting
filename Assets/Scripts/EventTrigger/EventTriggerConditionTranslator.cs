using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// 文字列をEventTriggerの条件式にするクラス。
/// </summary>
public class EventTriggerConditionTranslator
{
    public static EventTriggerCondition TranslateString(string paramData)
    {
        var empty = new EventTriggerCondition();

        if (paramData == null)
        {
            return empty;
        }

        string[] paramDataArray = paramData.Split(';');

        List<EventTriggerCondition> conditions = new List<EventTriggerCondition>();

        foreach (var data in paramDataArray)
        {
            string[] pArray = data.Trim().Split(':');

            if (pArray.Length != 4)
            {
                continue;
            }

            string type = pArray[0].Trim();
            string name = pArray[1].Trim();
            string comp = pArray[2].Trim();
            string value = pArray[3].Trim();

            if (!IsValidVariableType(type))
            {
                continue;
            }

            var vType = GetVariableType(type);
            EventTriggerCondition.E_COMPARE_TYPE compType = EventTriggerCondition.E_COMPARE_TYPE.EQUAL;
            EventTriggerCondition.E_BOOL_COMPARE_TYPE bCompType = EventTriggerCondition.E_BOOL_COMPARE_TYPE.EQUAL;
            float fValue = 0;
            bool bValue = false;

            switch (vType)
            {
                default:
                    if (!IsValidCompareType(comp) || !float.TryParse(value, out fValue))
                    {
                        continue;
                    }

                    compType = GetCompareType(comp);
                    break;
                case EventTriggerCondition.E_VARIABLE_TYPE.BOOL:
                    if (!IsValidBoolCompareType(comp) || !bool.TryParse(value, out bValue))
                    {
                        continue;
                    }

                    bCompType = GetBoolCompareType(comp);
                    break;
            }

            var condition = new EventTriggerCondition();
            condition.IsSingleCondition = true;
            condition.VariableType = vType;
            condition.VariableName = name;
            condition.CompareType = compType;
            condition.BoolCompareType = bCompType;
            condition.CompareValue = fValue;
            condition.BoolCompareValue = bValue;

            conditions.Add(condition);
        }

        if (conditions.Count < 1)
        {
            return empty;
        }
        else if (conditions.Count == 1)
        {
            return conditions[0];
        }

        empty.IsSingleCondition = false;
        empty.MultiConditionType = EventTriggerCondition.E_MULTI_CONDITION_TYPE.AND;
        empty.Conditions = conditions.ToArray();
        return empty;
    }

    private static bool IsValidVariableType(string type)
    {
        switch (type)
        {
            case "I":
            case "F":
            case "B":
            case "TP":
                return true;
        }

        return false;
    }
    private static EventTriggerCondition.E_VARIABLE_TYPE GetVariableType(string type)
    {
        switch (type)
        {
            default:
                return EventTriggerCondition.E_VARIABLE_TYPE.INT;
            case "F":
                return EventTriggerCondition.E_VARIABLE_TYPE.FLOAT;
            case "B":
                return EventTriggerCondition.E_VARIABLE_TYPE.BOOL;
            case "TP":
                return EventTriggerCondition.E_VARIABLE_TYPE.TIME_PERIOD;
        }
    }

    private static bool IsValidCompareType(string comp)
    {
        switch (comp)
        {
            case "EQL":
            case "NOT":
            case "LES":
            case "LEE":
            case "MOR":
            case "MOE":
                return true;
        }

        return false;
    }

    private static bool IsValidBoolCompareType(string comp)
    {
        switch (comp)
        {
            case "EQL":
            case "NOT":
                return true;
        }

        return false;
    }

    private static EventTriggerCondition.E_COMPARE_TYPE GetCompareType(string comp)
    {
        switch (comp)
        {
            default:
                return EventTriggerCondition.E_COMPARE_TYPE.EQUAL;
            case "NOT":
                return EventTriggerCondition.E_COMPARE_TYPE.NOT_EQUAL;
            case "LES":
                return EventTriggerCondition.E_COMPARE_TYPE.LESS_THAN;
            case "LEE":
                return EventTriggerCondition.E_COMPARE_TYPE.LESS_THAN_EQUAL;
            case "MOR":
                return EventTriggerCondition.E_COMPARE_TYPE.MORE_THAN;
            case "MOE":
                return EventTriggerCondition.E_COMPARE_TYPE.MORE_THAN_EQUAL;
        }
    }

    private static EventTriggerCondition.E_BOOL_COMPARE_TYPE GetBoolCompareType(string comp)
    {
        switch (comp)
        {
            default:
                return EventTriggerCondition.E_BOOL_COMPARE_TYPE.EQUAL;
            case "NOT":
                return EventTriggerCondition.E_BOOL_COMPARE_TYPE.NOT_EQUAL;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// 文字列をEventTriggerの条件式にするクラス。
/// </summary>
public class EventTriggerConditionTranslator
{
    public static EventTriggerRootCondition TranslateString(string paramData)
    {
        var empty = new EventTriggerRootCondition();

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
            E_COMPARE_TYPE compType = E_COMPARE_TYPE.EQUAL;
            E_BOOL_COMPARE_TYPE bCompType = E_BOOL_COMPARE_TYPE.EQUAL;
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
                case E_VARIABLE_TYPE.BOOL:
                    if (!IsValidBoolCompareType(comp) || !bool.TryParse(value, out bValue))
                    {
                        continue;
                    }

                    bCompType = GetBoolCompareType(comp);
                    break;
            }

            var condition = new EventTriggerCondition();
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
            empty.IsMultiCondition = false;
            empty.SingleCondition = conditions[0];
            return empty;
        }
        else
        {
            empty.IsMultiCondition = true;
            empty.MultiConditionType = E_MULTI_CONDITION_TYPE.AND;
            empty.MultiConditions = conditions.ToArray();
            return empty;
        }

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
    private static E_VARIABLE_TYPE GetVariableType(string type)
    {
        switch (type)
        {
            default:
                return E_VARIABLE_TYPE.INT;
            case "F":
                return E_VARIABLE_TYPE.FLOAT;
            case "B":
                return E_VARIABLE_TYPE.BOOL;
            case "TP":
                return E_VARIABLE_TYPE.TIME_PERIOD;
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

    private static E_COMPARE_TYPE GetCompareType(string comp)
    {
        switch (comp)
        {
            default:
                return E_COMPARE_TYPE.EQUAL;
            case "NOT":
                return E_COMPARE_TYPE.NOT_EQUAL;
            case "LES":
                return E_COMPARE_TYPE.LESS_THAN;
            case "LEE":
                return E_COMPARE_TYPE.LESS_THAN_EQUAL;
            case "MOR":
                return E_COMPARE_TYPE.MORE_THAN;
            case "MOE":
                return E_COMPARE_TYPE.MORE_THAN_EQUAL;
        }
    }

    private static E_BOOL_COMPARE_TYPE GetBoolCompareType(string comp)
    {
        switch (comp)
        {
            default:
                return E_BOOL_COMPARE_TYPE.EQUAL;
            case "NOT":
                return E_BOOL_COMPARE_TYPE.NOT_EQUAL;
        }
    }
}

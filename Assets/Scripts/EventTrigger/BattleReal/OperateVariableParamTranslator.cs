using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OperateVariableParamに変換するクラス。
/// </summary>
public class OperateVariableParamTranslator
{
    /// <summary>
    /// 文字列からOperateVariableParam配列に変換する。
    /// </summary>
    public static OperateVariableParam[] TranslateFromString(string paramData)
    {
        if (paramData == null)
        {
            return null;
        }

        string[] paramDataArray = paramData.Split(';');

        List<OperateVariableParam> paramList = null;

        foreach (var data in paramDataArray)
        {
            string[] pArray = data.Trim().Split(':');

            if (pArray.Length != 5) continue;

            string variableTypeData = pArray[0].Trim();
            string variableNameData = pArray[1].Trim();
            string operandTypeData = pArray[2].Trim();
            string operandValueTypeData = pArray[3].Trim();
            string operandValueData = pArray[4].Trim();

            if (!IsValidVariableType(variableTypeData)) continue;
            if (!IsValidOperandValueType(operandValueTypeData)) continue;

            E_EVENT_TRIGGER_VARIABLE_TYPE variableType = GetVariableType(variableTypeData);
            E_OPERAND_VALUE_TYPE operandValueType = GetOperandValueType(operandValueTypeData);

            float operandValue = 0;
            bool boolOperandValue = false;

            if (variableType != E_EVENT_TRIGGER_VARIABLE_TYPE.BOOL)
            {
                if (!IsValidOperandType(operandTypeData)) continue;
                if (!float.TryParse(operandValueData, out operandValue)) continue;
            }
            else
            {
                if (!IsValidBoolOperandType(operandTypeData)) continue;
                if (!bool.TryParse(operandValueData, out boolOperandValue)) continue;
            }

            E_OPERAND_TYPE operandType = GetOperandType(operandTypeData);
            E_BOOL_OPERAND_TYPE boolOperandType = GetBoolOperandType(operandTypeData);

            OperateVariableParam param = new OperateVariableParam();
            param.VariableType = variableType;
            param.VariableName = variableNameData;
            param.OperandType = operandType;
            param.BoolOperandType = boolOperandType;
            param.OperandValueName = operandValueData;
            param.OperandValue = operandValue;
            param.BoolOperandValue = boolOperandValue;

            if (paramList == null)
            {
                paramList = new List<OperateVariableParam>();
            }

            paramList.Add(param);
        }

        if (paramList == null || paramList.Count < 1)
        {
            return null;
        }

        return paramList.ToArray();
    }

    private static bool IsValidVariableType(string type)
    {
        switch (type)
        {
            case "I":
            case "F":
            case "B":
                return true;
        }

        return false;
    }

    private static bool IsValidOperandType(string operandType)
    {
        switch (operandType)
        {
            case "SUBS":
            case "ADD":
            case "SUB":
            case "MUL":
            case "DIV":
            case "MOD":
                return true;
        }

        return false;
    }

    private static bool IsValidBoolOperandType(string boolOperandType)
    {
        switch (boolOperandType)
        {
            case "SUBS":
            case "OR":
            case "AND":
            case "XOR":
            case "NOT":
            case "NOR":
            case "NAND":
            case "XNOR":
                return true;
        }

        return false;
    }

    private static bool IsValidOperandValueType(string operandValueType)
    {
        switch (operandValueType)
        {
            case "CST":
            case "VAR":
                return true;
        }

        return false;
    }

    private static E_EVENT_TRIGGER_VARIABLE_TYPE GetVariableType(string type)
    {
        switch (type)
        {
            default:
                return E_EVENT_TRIGGER_VARIABLE_TYPE.INT;
            case "F":
                return E_EVENT_TRIGGER_VARIABLE_TYPE.FLOAT;
            case "B":
                return E_EVENT_TRIGGER_VARIABLE_TYPE.BOOL;
        }
    }

    private static E_OPERAND_TYPE GetOperandType(string operandType)
    {
        switch (operandType)
        {
            default:
                return E_OPERAND_TYPE.SUBSTITUTE;
            case "ADD":
                return E_OPERAND_TYPE.ADD;
            case "SUB":
                return E_OPERAND_TYPE.SUB;
            case "MUL":
                return E_OPERAND_TYPE.MUL;
            case "DIV":
                return E_OPERAND_TYPE.DIV;
            case "MOD":
                return E_OPERAND_TYPE.MOD;
        }
    }

    private static E_BOOL_OPERAND_TYPE GetBoolOperandType(string boolOperandType)
    {
        switch (boolOperandType)
        {
            default:
                return E_BOOL_OPERAND_TYPE.SUBSTITUTE;
            case "OR":
                return E_BOOL_OPERAND_TYPE.OR;
            case "AND":
                return E_BOOL_OPERAND_TYPE.AND;
            case "XOR":
                return E_BOOL_OPERAND_TYPE.XOR;
            case "NOT":
                return E_BOOL_OPERAND_TYPE.NOT;
            case "NOR":
                return E_BOOL_OPERAND_TYPE.NOR;
            case "NAND":
                return E_BOOL_OPERAND_TYPE.NAND;
            case "XNOR":
                return E_BOOL_OPERAND_TYPE.XNOR;
        }
    }

    private static E_OPERAND_VALUE_TYPE GetOperandValueType(string operandValueType)
    {
        switch (operandValueType)
        {
            default:
                return E_OPERAND_VALUE_TYPE.CONSTANT;
            case "VAR":
                return E_OPERAND_VALUE_TYPE.VARIABLE;
        }
    }
}

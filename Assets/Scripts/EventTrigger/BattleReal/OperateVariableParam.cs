using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct OperateVariableParam
{
    public E_EVENT_TRIGGER_VARIABLE_TYPE VariableType;

    public string VariableName;

    public E_OPERAND_TYPE OperandType;

    public E_BOOL_OPERAND_TYPE BoolOperandType;

    public E_OPERAND_VALUE_TYPE OperandValueType;

    public string OperandValueName;

    public float OperandValue;

    public bool BoolOperandValue;
}

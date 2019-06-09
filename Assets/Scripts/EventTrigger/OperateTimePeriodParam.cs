using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct OperateTimePeriodParam
{
    public enum E_OPERAND_TYPE
    {
        COUNT_START,
        STOP,
    }

    public string TimePeriodName;

    public E_OPERAND_TYPE OperandType;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OperateVariableParam
{
    public enum E_OPERAND_VALUE_TYPE
    {
        /// <summary>
        /// 定数を用いる
        /// </summary>
        CONSTANT,

        /// <summary>
        /// 変数を用いる
        /// </summary>
        VARIABLE,
    }

    public enum E_OPERAND_TYPE
    {
        /// <summary>
        /// 代入
        /// </summary>
        SUBSTITUTE,

        /// <summary>
        /// 加算
        /// </summary>
        ADD,

        /// <summary>
        /// 減算
        /// </summary>
        SUB,

        /// <summary>
        /// 乗算
        /// </summary>
        MUL,

        /// <summary>
        /// 除算
        /// </summary>
        DIV,

        /// <summary>
        /// 剰余算
        /// </summary>
        MOD,
    }

    public enum E_BOOL_OPERAND_TYPE
    {
        /// <summary>
        /// 代入
        /// </summary>
        SUBSTITUTE,

        /// <summary>
        /// 論理和
        /// </summary>
        OR,

        /// <summary>
        /// 論理積
        /// </summary>
        AND,

        /// <summary>
        /// 排他的論理和
        /// </summary>
        XOR,

        /// <summary>
        /// 否定論理
        /// </summary>
        NOT,

        /// <summary>
        /// 否定論理和
        /// </summary>
        NOR,

        /// <summary>
        /// 否定論理積
        /// </summary>
        NAND,

        /// <summary>
        /// 排他的否定論理和
        /// </summary>
        XNOR,
    }

    public E_EVENT_TRIGGER_VARIABLE_TYPE VariableType;

    public string VariableName;

    public E_OPERAND_TYPE OperandType;

    public E_BOOL_OPERAND_TYPE BoolOperandType;

    public E_OPERAND_VALUE_TYPE OperandValueType;

    public string OperandValueName;

    public float OperandValue;

    public bool BoolOperandValue;
}

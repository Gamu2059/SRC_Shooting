using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベント変数を操作するパラメータ。
/// </summary>
[System.Serializable]
public struct EventTriggerVariableOperateParam
{
    public enum E_OPERAND_TYPE
    {
        /// <summary>
        /// 代入
        /// </summary>
        SUBSTITUTION,

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

    public enum E_BOOL_OPERATE_TYPE
    {
        /// <summary>
        /// 代入
        /// </summary>
        SUBSTITUTION,

        /// <summary>
        /// イベント変数の値を反転
        /// </summary>
        NOT,

        /// <summary>
        /// 論理和
        /// </summary>
        OR,

        /// <summary>
        /// 論理積
        /// </summary>
        AND,
    }

    public E_EVENT_TRIGGER_VARIABLE_TYPE VariableType;
    public string VariableName;
    public E_OPERAND_TYPE OperandType;
    public E_BOOL_OPERATE_TYPE BoolOperandType;
    public float OperandValue;
    public bool BoolOperandValue;
}

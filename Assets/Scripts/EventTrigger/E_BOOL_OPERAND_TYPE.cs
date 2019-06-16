using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OperateVariableParamで用いる論理演算の種類。
/// </summary>
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

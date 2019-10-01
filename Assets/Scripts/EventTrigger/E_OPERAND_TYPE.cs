using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OperateVariableParamで用いる演算の種類。
/// </summary>
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

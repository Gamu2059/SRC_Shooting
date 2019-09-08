using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OperateVariableParamで用いる参照値の種類。
/// </summary>
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

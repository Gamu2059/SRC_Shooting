using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

/// <summary>
/// Utageの会話中にリアルモードのイベント変数を制御するコマンド。<br/>
/// <br/>
/// Command : EventVar<br/>
/// Parameter :<br/>
/// arg1 :<br/>
/// 操作対象の変数の種類を指定します。<br/>
/// INT or FLOAT or BOOL のいずれかを文字列で指定します。<br/>
/// 
/// <br/>
/// arg2 :<br/>
/// 操作対象の変数名を指定します。<br/>
/// 
/// <br/>
/// arg3 :<br/>
/// 対象への操作の種類を指定します。<br/>
/// arg1でINTかFLOATを指定した場合<br/>
///     SUBSTITUTE : arg5の値を代入します。<br/>
///     ADD        : arg5の値で加算します。<br/>
///     SUB        : arg5の値で減算します。<br/>
///     MUL        : arg5の値で乗算します。<br/>
///     DIV        : arg5の値で除算します。<br/>
///     MOD        : arg5の値で剰余算します。<br/>
/// 
/// arg1でBOOLを指定した場合<br/>
///     SUBSTITUTE : arg5の値を代入します。<br/>
///     OR         : arg5の値と論理和を取ります。<br/>
///     AND        : arg5の値と論理積を取ります。<br/>
///     XOR        : arg5の値と排他的論理和を取ります。<br/>
///     NOT        : arg5の値を否定して代入します。<br/>
///     NOR        : arg5の値と否定論理和を取ります。<br/>
///     NAND       : arg5の値と否定論理積を取ります。<br/>
///     XNOR       : arg5の値と排他的否定論理和を取ります。<br/>
/// 
/// <br/>
/// arg4 :<br/>
/// 操作対象へ操作する値の種類を指定します。<br/>
/// CONSTANT or VARIABLE のいずれかを文字列で指定します。<br/>
/// 
/// <br/>
/// arg5 :<br/>
/// 操作対象へ操作する値を指定します。<br/>
/// arg4でVARIABLEを指定した場合<br/>
///     arg1で指定した型と同じ型の変数の名前を指定します。<br/>
/// <br/>
/// arg4でCONSTANTを指定した場合<br/>
///     arg1でINTを指定した場合は、整数値を指定します。<br/>
///     arg1でFLOATを指定した場合は、実数値を指定します。<br/>
///     arg1でBOOLを指定した場合は、trueかfalseを指定します。<br/>
/// </summary>
public class BattleRealEventVariableCommand : AdvCommand
{
    /// <summary>
    /// コマンドチェックで無効と判断された場合は、DoCommandで処理しない。
    /// </summary>
    private bool m_IsInvalidCommad;

    private E_EVENT_TRIGGER_VARIABLE_TYPE m_VariableType;
    private string m_VariableName;
    private E_OPERAND_TYPE m_IntFloatOperandType;
    private E_BOOL_OPERAND_TYPE m_BoolOperandType;
    private E_OPERAND_VALUE_TYPE m_OperandValueType;
    private string m_OperandValueName;
    private float m_IntFloatOperandValue;
    private bool m_BoolOperandValue;

    public BattleRealEventVariableCommand(StringGridRow row) : base(row)
    {
        m_IsInvalidCommad = false;

        var arg1 = ParseCellOptional<string>(AdvColumnName.Arg1, null);
        var arg2 = ParseCellOptional<string>(AdvColumnName.Arg2, null);
        var arg3 = ParseCellOptional<string>(AdvColumnName.Arg3, null);
        var arg4 = ParseCellOptional<string>(AdvColumnName.Arg4, null);
        var arg5 = ParseCellOptional<string>(AdvColumnName.Arg5, null);

        if (!CheckArgEmpty(arg1, arg2, arg3, arg4, arg5))
        {
            Debug.LogErrorFormat("[{0}] : 引数が無効です。", GetType().Name);
            m_IsInvalidCommad = true;
            return;
        }

        arg1 = arg1.Trim();
        arg2 = arg2.Trim();
        arg3 = arg3.Trim();
        arg4 = arg4.Trim();
        arg5 = arg5.Trim();

        if (!ParseArg1(arg1))
        {
            m_IsInvalidCommad = true;
            return;
        }

        m_VariableName = arg2;

        if (m_VariableType == E_EVENT_TRIGGER_VARIABLE_TYPE.BOOL)
        {
            if (!ParseBoolArg3(arg3))
            {
                m_IsInvalidCommad = true;
                return;
            }
        }
        else
        {
            if (!ParseIntFloatArg3(arg3))
            {
                m_IsInvalidCommad = true;
                return;
            }
        }

        if (!ParseArg4(arg4))
        {
            m_IsInvalidCommad = true;
            return;
        }

        if (m_OperandValueType == E_OPERAND_VALUE_TYPE.VARIABLE)
        {
            m_OperandValueName = arg5;
        }
        else
        {
            if (!ParseArg5(arg5))
            {
                m_IsInvalidCommad = true;
                return;
            }
        }
    }

    public override void DoCommand(AdvEngine engine)
    {
        if (m_IsInvalidCommad)
        {
            return;
        }

        var param = new OperateVariableParam();
        param.VariableType = m_VariableType;
        param.VariableName = m_VariableName;
        param.OperandType = m_IntFloatOperandType;
        param.BoolOperandType = m_BoolOperandType;
        param.OperandValueType = m_OperandValueType;
        param.OperandValueName = m_OperandValueName;
        param.OperandValue = m_IntFloatOperandValue;
        param.BoolOperandValue = m_BoolOperandValue;
        BattleRealEventManager.Instance.ExecuteEvent(new BattleRealEventContent()
        {
            EventType = BattleRealEventContent.E_EVENT_TYPE.OPERATE_VARIABLE,
            OperateVariableParams = new[] {param}
        });
    }

    private bool CheckArgEmpty(params string[] args)
    {
        if (args == null)
        {
            return false;
        }

        foreach (var arg in args)
        {
            if (string.IsNullOrEmpty(arg) || string.IsNullOrWhiteSpace(arg))
            {
                return false;
            }
        }

        return true;
    }

    private bool ParseArg1(string arg1)
    {
        switch (arg1)
        {
            case "INT":
                m_VariableType = E_EVENT_TRIGGER_VARIABLE_TYPE.INT;
                return true;
            case "FLOAT":
                m_VariableType = E_EVENT_TRIGGER_VARIABLE_TYPE.FLOAT;
                return true;
            case "BOOL":
                m_VariableType = E_EVENT_TRIGGER_VARIABLE_TYPE.BOOL;
                return true;
            default:
                Debug.LogErrorFormat("[{0}] : Arg1の値が変数の型に合致しません。 Arg1 : {1}", GetType().Name, arg1);
                return false;
        }
    }

    private bool ParseIntFloatArg3(string arg3)
    {
        switch (arg3)
        {
            case "SUBSTITUTE":
                m_IntFloatOperandType = E_OPERAND_TYPE.SUBSTITUTE;
                return true;
            case "ADD":
                m_IntFloatOperandType = E_OPERAND_TYPE.ADD;
                return true;
            case "SUB":
                m_IntFloatOperandType = E_OPERAND_TYPE.SUB;
                return true;
            case "MUL":
                m_IntFloatOperandType = E_OPERAND_TYPE.MUL;
                return true;
            case "DIV":
                m_IntFloatOperandType = E_OPERAND_TYPE.DIV;
                return true;
            case "MOD":
                m_IntFloatOperandType = E_OPERAND_TYPE.MOD;
                return true;
            default:
                Debug.LogErrorFormat("[{0}] : Arg3の演算タイプが変数の型に合致しません。 Arg3 : {1}", GetType().Name, arg3);
                return false;
        }
    }

    private bool ParseBoolArg3(string arg3)
    {
        switch (arg3)
        {
            case "SUBSTITUTE":
                m_BoolOperandType = E_BOOL_OPERAND_TYPE.SUBSTITUTE;
                return true;
            case "OR":
                m_BoolOperandType = E_BOOL_OPERAND_TYPE.OR;
                return true;
            case "AND":
                m_BoolOperandType = E_BOOL_OPERAND_TYPE.AND;
                return true;
            case "XOR":
                m_BoolOperandType = E_BOOL_OPERAND_TYPE.XOR;
                return true;
            case "NOT":
                m_BoolOperandType = E_BOOL_OPERAND_TYPE.NOT;
                return true;
            case "NOR":
                m_BoolOperandType = E_BOOL_OPERAND_TYPE.NOR;
                return true;
            case "NAND":
                m_BoolOperandType = E_BOOL_OPERAND_TYPE.NAND;
                return true;
            case "XNOR":
                m_BoolOperandType = E_BOOL_OPERAND_TYPE.XNOR;
                return true;
            default:
                Debug.LogErrorFormat("[{0}] : Arg3の演算タイプが変数の型に合致しません。 Arg3 : {1}", GetType().Name, arg3);
                return false;
        }
    }

    private bool ParseArg4(string arg4)
    {
        switch (arg4)
        {
            case "CONSTANT":
                m_OperandValueType = E_OPERAND_VALUE_TYPE.CONSTANT;
                return true;
            case "VARIABLE":
                m_OperandValueType = E_OPERAND_VALUE_TYPE.VARIABLE;
                return true;
            default:
                Debug.LogErrorFormat("[{0}] : Arg4の値が変数の型に合致しません。 Arg4 : {1}", GetType().Name, arg4);
                return false;
        }
    }

    private bool ParseArg5(string arg5)
    {
        switch (m_VariableType)
        {
            case E_EVENT_TRIGGER_VARIABLE_TYPE.INT:
                if (int.TryParse(arg5, out int intResult))
                {
                    m_IntFloatOperandValue = intResult;
                    return true;
                }
                Debug.LogErrorFormat("[{0}] : Arg5の値をint値にパースできません。 Arg5 : {1}", GetType().Name, arg5);
                return false;
            case E_EVENT_TRIGGER_VARIABLE_TYPE.FLOAT:
                if (float.TryParse(arg5, out float floatResult))
                {
                    m_IntFloatOperandValue = floatResult;
                    return true;
                }
                Debug.LogErrorFormat("[{0}] : Arg5の値をfloat値にパースできません。 Arg5 : {1}", GetType().Name, arg5);
                return false;
            case E_EVENT_TRIGGER_VARIABLE_TYPE.BOOL:
                if (bool.TryParse(arg5, out bool boolResult))
                {
                    m_BoolOperandValue = boolResult;
                    return true;
                }
                Debug.LogErrorFormat("[{0}] : Arg5の値をbool値にパースできません。 Arg5 : {1}", GetType().Name, arg5);
                return false;
        }

        return false;
    }
}

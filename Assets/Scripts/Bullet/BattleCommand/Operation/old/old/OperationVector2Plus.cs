using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2型の加算を表すクラス。
/// </summary>
public class OperationVector2Plus : OperationVector2Base1
{

    /// <summary>
    /// 足す値の配列
    /// </summary>
    private OperationVector2Base1[] m_OperationArray;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public OperationVector2Plus(OperationVector2Base1[] operationArray)
    {
        m_OperationArray = operationArray;
    }


    public override Vector2 GetResult()
    {
        Vector2 result = Vector2.zero;

        foreach (OperationVector2Base1 operation in m_OperationArray)
        {
            result += operation.GetResult();
        }

        return result;
    }
}





//public static void Plus(OperationVector2Base operation, Vector2 value)
//{
//    operation = new OperationVector2Plus(operation, value);
//}


///// <summary>
///// 加える値
///// </summary>
//private Vector2 m_Value;


///// <summary>
///// 足す値1
///// </summary>
//private OperationVector2Base m_Operation1;


///// <summary>
///// 足す値2
///// </summary>
//private OperationVector2Base m_Operation2;


///// <summary>
///// コンストラクタ
///// </summary>
//public OperationVector2Plus(OperationVector2Base operation, Vector2 value)
//{
//    m_Operation = operation;
//    m_Value = value;
//}


///// <summary>
///// コンストラクタ
///// </summary>
//public OperationVector2Plus(OperationVector2Base operation1, OperationVector2Base operation2)
//{
//    m_Operation1 = operation1;
//    m_Operation2 = operation2;
//}


//public override Vector2 GetResult()
//{
//    return m_Operation1.GetResult() + m_Operation2.GetResult();
//}

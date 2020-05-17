#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の演算を委譲する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/delegate", fileName = "OperationIntDelegate", order = 0)]
[System.Serializable]
public class OperationIntDelegate : OperationIntBase
{

    /// <summary>
    /// 委譲された演算
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Operation;


    public override float GetResultFloat()
    {
        return m_Operation.GetResultFloat();
    }


    public override int GetResultInt()
    {
        return m_Operation.GetResultInt();
    }
}
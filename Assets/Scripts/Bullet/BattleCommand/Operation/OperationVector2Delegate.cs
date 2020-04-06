#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2型の演算を委譲する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/delegate", fileName = "OperationVector2Delegate", order = 0)]
[System.Serializable]
public class OperationVector2Delegate : OperationVector2Base
{

    /// <summary>
    /// 委譲された演算
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Operation;


    public override Vector2 GetResultVector2()
    {
        return m_Operation.GetResultVector2();
    }
}
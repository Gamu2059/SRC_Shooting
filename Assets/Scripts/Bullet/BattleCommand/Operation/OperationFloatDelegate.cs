#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の演算を委譲する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/delegate", fileName = "OperationFloatDelegate", order = 0)]
[System.Serializable]
public class OperationFloatDelegate : OperationFloatBase
{

    /// <summary>
    /// 委譲された演算
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Operation;


    public override float GetResultFloat()
    {
        return m_Operation.GetResultFloat();
    }
}
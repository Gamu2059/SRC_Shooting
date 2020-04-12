#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2つのInt型の値が等しいかどうかのbool型を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/bool/equalInt", fileName = "OperationBoolEqualInt", order = 0)]
[System.Serializable]
public class OperationBoolEqualInt : OperationBoolBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_IntA;

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_IntB;


    public override bool GetResultBool()
    {
        return m_IntA.GetResultInt() == m_IntB.GetResultInt();
    }
}

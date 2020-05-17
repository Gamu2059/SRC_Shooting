#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float値を2πで割った余りを求める演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/remainderByTwoPi", fileName = "OperationFloatRemainder", order = 0)]
[System.Serializable]
public class OperationFloatRemainderByTwoPi : OperationFloatBase
{

    /// <summary>
    /// 割られる数
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Float;


    public override float GetResultFloat()
    {
        return m_Float.GetResultFloat() % (2 * Mathf.PI);
    }
}
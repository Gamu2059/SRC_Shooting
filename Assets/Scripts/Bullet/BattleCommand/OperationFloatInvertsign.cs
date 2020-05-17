#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float値の符号を反転させる演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/invertsign", fileName = "OperationFloatInvertsign", order = 0)]
[System.Serializable]
public class OperationFloatInvertsign : OperationFloatBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Float;


    public override float GetResultFloat()
    {
        return -1 * m_Float.GetResultFloat();
    }
}
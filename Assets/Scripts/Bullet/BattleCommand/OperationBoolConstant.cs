#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bool型の定数を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/bool/constant", fileName = "OperationBoolConstant", order = 0)]
[System.Serializable]
public class OperationBoolConstant : OperationBoolBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private bool m_Value;


    public override bool GetResultBool()
    {
        return m_Value;
    }
}

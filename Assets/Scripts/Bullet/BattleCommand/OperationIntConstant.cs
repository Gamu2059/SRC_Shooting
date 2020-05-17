using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の定数を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/constant", fileName = "OperationIntConstant", order = 0)]
[System.Serializable]
public class OperationIntConstant : OperationIntBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    public int m_Value;


    public override int GetResultInt()
    {
        return m_Value;
    }


    public override float GetResultFloat()
    {
        return m_Value;
    }
}

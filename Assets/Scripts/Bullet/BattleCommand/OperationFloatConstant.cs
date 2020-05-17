using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の定数を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/constant", fileName = "OperationFloatConstant", order = 0)]
[System.Serializable]
public class OperationFloatConstant : OperationFloatBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    public float m_Value;


    public override float GetResultFloat()
    {
        return m_Value;
    }
}

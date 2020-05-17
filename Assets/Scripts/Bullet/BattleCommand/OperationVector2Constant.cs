using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2型の定数を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/constant", fileName = "OperationVector2Constant", order = 0)]
[System.Serializable]
public class OperationVector2Constant : OperationVector2Base
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    public Vector2 m_Value;


    public override Vector2 GetResultVector2()
    {
        return m_Value;
    }
}

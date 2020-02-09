using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2型の変数を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/variable", fileName = "OperationVector2Variable", order = 0)]
[System.Serializable]
public class OperationVector2Variable : OperationVector2Base
{

    /// <summary>
    /// 値
    /// </summary>
    //[SerializeField]
    private Vector2 m_Value;


    public override Vector2 GetResultVector2()
    {
        return m_Value;
    }


    /// <summary>
    /// 値を書き換える
    /// </summary>
    public void SetValueVector2(Vector2 value)
    {
        m_Value = value;
    }
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bool型の値によって条件分岐させ、Vector2型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/junction/bool", fileName = "OperationVector2JuncBool", order = 0)]
[System.Serializable]
public class OperationVector2JuncBool : OperationVector2Base
{

    /// <summary>
    /// この値によって条件分岐する
    /// </summary>
    [SerializeField]
    private OperationBoolBase[] m_If;

    /// <summary>
    /// trueだった場合の値
    /// </summary>
    [SerializeField]
    private OperationVector2Base[] m_True;

    /// <summary>
    /// falseだった場合の値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_False;


    public override Vector2 GetResultVector2()
    {
        for (int i = 0;i < m_If.Length;i++)
        {
            if (m_If[i].GetResultBool())
            {
                return m_True[i].GetResultVector2();
            }
        }

        return m_False.GetResultVector2();
    }
}
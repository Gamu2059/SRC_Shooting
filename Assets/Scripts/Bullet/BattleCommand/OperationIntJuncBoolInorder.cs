#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bool型の値によって条件分岐させ、順番通りのint型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/junction/boolInorder", fileName = "OperationIntJuncBoolInorder", order = 0)]
[System.Serializable]
public class OperationIntJuncBoolInorder : OperationIntBase
{

    /// <summary>
    /// この値によって条件分岐する
    /// </summary>
    [SerializeField]
    private OperationBoolBase[] m_If;


    public override float GetResultFloat()
    {
        return GetResultInt();
    }


    public override int GetResultInt()
    {
        for (int i = 0; i < m_If.Length; i++)
        {
            if (m_If[i].GetResultBool())
            {
                return i;
            }
        }

        return m_If.Length;
    }
}
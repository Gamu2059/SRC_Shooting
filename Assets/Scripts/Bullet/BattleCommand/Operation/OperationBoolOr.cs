#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 論理演算ORを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/bool/or", fileName = "OperationBoolOr", order = 0)]
[System.Serializable]
public class OperationBoolOr : OperationBoolBase
{

    /// <summary>
    /// 値の配列
    /// </summary>
    [SerializeField]
    private OperationBoolBase[] m_ValueArray;


    public override bool GetResultBool()
    {
        bool resultBool = false;

        foreach (OperationBoolBase value in m_ValueArray)
        {
            resultBool = resultBool || value.GetResultBool();
        }

        return resultBool;
    }
}

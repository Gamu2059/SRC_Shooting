#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 論理演算ANDを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/bool/and", fileName = "OperationBoolAnd", order = 0)]
[System.Serializable]
public class OperationBoolAnd : OperationBoolBase
{

    /// <summary>
    /// 値の配列
    /// </summary>
    [SerializeField]
    private OperationBoolBase[] m_And;


    public override bool GetResultBool()
    {
        bool resultBool = true;

        foreach (OperationBoolBase value in m_And)
        {
            resultBool = resultBool && value.GetResultBool();
        }

        return resultBool;
    }
}

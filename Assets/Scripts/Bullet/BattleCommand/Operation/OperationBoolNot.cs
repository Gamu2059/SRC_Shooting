#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 論理演算NOTを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/bool/not", fileName = "OperationBoolNot", order = 0)]
[System.Serializable]
public class OperationBoolNot : OperationBoolBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationBoolBase m_Not;


    public override bool GetResultBool()
    {
        return !m_Not.GetResultBool();
    }
}

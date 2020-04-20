#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bool値を代入する、一度しかループしないfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/once/bool", fileName = "ForOnceBool", order = 0)]
[System.Serializable]
public class ForOnceBool : ForOnceBase
{

    /// <summary>
    /// この変数に代入する
    /// </summary>
    [SerializeField]
    private OperationBoolVariable m_BoolVariable;

    /// <summary>
    /// 代入する値
    /// </summary>
    [SerializeField]
    private OperationBoolBase m_BoolValue;


    public override void Setup()
    {

    }


    public override void Do()
    {
        m_BoolVariable.Value = m_BoolValue.GetResultBool();
    }
}
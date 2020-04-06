#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int値を代入する、一度しかループしないfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/once/int", fileName = "ForOnceInt", order = 0)]
[System.Serializable]
public class ForOnceInt : ForOnceBase
{

    /// <summary>
    /// この変数に代入する
    /// </summary>
    [SerializeField]
    private OperationIntVariable m_IntVariable;

    /// <summary>
    /// 代入する値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_IntValue;


    public override void Setup()
    {

    }


    public override void Do()
    {
        m_IntVariable.Value = m_IntValue.GetResultInt();
    }
}
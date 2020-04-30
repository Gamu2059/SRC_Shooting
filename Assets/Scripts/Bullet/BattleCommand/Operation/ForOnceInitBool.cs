#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 開始時に一度だけbool値を代入する、ループしないfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/onceInit/bool", fileName = "ForOnceInitBool", order = 0)]
[System.Serializable]
public class ForOnceInitBool : ForOnceBase
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
        m_BoolVariable.Value = m_BoolValue.GetResultBool();
    }


    public override void Do()
    {

    }
}
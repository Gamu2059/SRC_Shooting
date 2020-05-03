#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 開始時に一度だけfloat値を代入する、ループしないfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/onceInit/float", fileName = "ForOnceInitFloat", order = 0)]
[System.Serializable]
public class ForOnceInitFloat : ForOnceBase
{

    /// <summary>
    /// この変数に代入する
    /// </summary>
    [SerializeField]
    private OperationFloatVariable m_FloatVariable;

    /// <summary>
    /// 代入する値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_FloatValue;


    public override void Setup()
    {
        m_FloatVariable.Value = m_FloatValue.GetResultFloat();
    }


    public override void Do()
    {

    }
}
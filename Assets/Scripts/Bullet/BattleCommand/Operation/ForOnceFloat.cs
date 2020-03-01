using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float値を代入する、一度しかループしないfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/once/float", fileName = "ForOnceFloat", order = 0)]
[System.Serializable]
public class ForOnceFloat : ForOnceBase
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

    }


    public override void Do()
    {
        m_FloatVariable.Value = m_FloatValue.GetResultFloat();
    }
}
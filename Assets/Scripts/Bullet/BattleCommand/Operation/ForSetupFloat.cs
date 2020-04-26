#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float値を代入する、初期化の処理を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/forSetup/float", fileName = "ForSetupFloat", order = 0)]
[System.Serializable]
public class ForSetupFloat : ForSetupBase
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
    private OperationFloatBase m_Float;


    public override void Setup()
    {
        m_FloatVariable.Value = m_Float.GetResultFloat();
    }
}
#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2値を代入する、初期化の処理を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/forSetup/Vector2", fileName = "ForSetupVector2", order = 0)]
[System.Serializable]
public class ForSetupVector2 : ForSetupBase
{

    /// <summary>
    /// この変数に代入する
    /// </summary>
    [SerializeField]
    private OperationVector2Variable m_Vector2Variable;

    /// <summary>
    /// 代入する値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Vector2;


    public override void Setup()
    {
        m_Vector2Variable.Value = m_Vector2.GetResultVector2();
    }
}
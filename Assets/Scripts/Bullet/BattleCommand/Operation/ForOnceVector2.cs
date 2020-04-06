#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2値を代入する、一度しかループしないfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/once/Vector2", fileName = "ForOnceVector2", order = 0)]
[System.Serializable]
public class ForOnceVector2 : ForOnceBase
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
    private OperationVector2Base m_Vector2Value;


    public override void Setup()
    {

    }


    public override void Do()
    {
        m_Vector2Variable.Value = m_Vector2Value.GetResultVector2();
    }
}
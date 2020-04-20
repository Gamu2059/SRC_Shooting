#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 開始時に一度だけVector2値を代入する、、ループしないfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/onceInit/Vector2", fileName = "ForOnceInitVector2", order = 0)]
[System.Serializable]
public class ForOnceInitVector2 : ForOnceBase
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
        m_Vector2Variable.Value = m_Vector2Value.GetResultVector2();
    }


    public override void Do()
    {

    }
}
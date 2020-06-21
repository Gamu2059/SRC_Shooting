#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 列挙したint値を代入していくfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/enum/int", fileName = "ForEnumInt", order = 0)]
[System.Serializable]
public class ForEnumInt : ForBase
{

    /// <summary>
    /// この変数に代入する
    /// </summary>
    [SerializeField]
    private OperationIntVariable m_Variable;

    /// <summary>
    /// 代入していく値の配列
    /// </summary>
    [SerializeField]
    private OperationIntBase[] m_Int;

    /// <summary>
    /// 現在のインデックス
    /// </summary>
    private int m_Index;


    public override void Setup()
    {

    }

    public override void Init()
    {
        m_Index = 0;
    }

    public override bool IsTrue()
    {
        // インデックスの値が配列数に収まっているなら
        if (m_Index < m_Int.Length)
        {
            // 代入する
            m_Variable.Value = m_Int[m_Index].GetResultInt();

            return true;
        }
        // インデックスの値が配列数以上になってしまっているなら
        else
        {
            return false;
        }
    }

    public override void Process()
    {
        m_Index++;
    }
}
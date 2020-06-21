#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 列挙したfloat値を代入していくfor文を表すクラス。（未使用）
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/enum/float", fileName = "ForEnumFloat", order = 0)]
[System.Serializable]
public class ForEnumFloat : ForBase
{

    /// <summary>
    /// この変数に代入する
    /// </summary>
    [SerializeField]
    private OperationFloatVariable m_Variable;

    /// <summary>
    /// 代入していく値の配列
    /// </summary>
    [SerializeField]
    private OperationFloatBase[] m_Float;

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
        if (m_Index < m_Float.Length)
        {
            // 代入する
            m_Variable.Value = m_Float[m_Index].GetResultFloat();

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
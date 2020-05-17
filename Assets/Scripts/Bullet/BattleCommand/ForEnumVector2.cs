#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 列挙したVector2値を代入していくfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/enum/Vector2", fileName = "ForEnumVector2", order = 0)]
[System.Serializable]
public class ForEnumVector2 : ForBase
{

    /// <summary>
    /// この変数に代入する
    /// </summary>
    [SerializeField]
    private OperationVector2Variable m_Variable;

    /// <summary>
    /// 代入していく値の配列
    /// </summary>
    [SerializeField]
    private OperationVector2Base[] m_Vector2;

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
        if (m_Index < m_Vector2.Length)
        {
            // 代入する
            m_Variable.Value = m_Vector2[m_Index].GetResultVector2();

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
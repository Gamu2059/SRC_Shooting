#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 初期値が0、増分が1であるfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/zeroone", fileName = "ForZeroone", order = 0)]
[System.Serializable]
public class ForZeroone : ForBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationIntVariable m_Value;

    /// <summary>
    /// ループの回数
    /// </summary>
    [SerializeField]
    private OperationIntBase m_TimeNum;


    public override void Setup()
    {

    }


    public override void Init()
    {
        m_Value.Value = 0;
    }


    public override bool IsTrue()
    {
        return m_Value.Value < m_TimeNum.GetResultInt();
    }


    public override void Process()
    {
        m_Value.Value++;
    }
}





//public override int GetValueInt()
//{
//    return m_Value.Value;
//}

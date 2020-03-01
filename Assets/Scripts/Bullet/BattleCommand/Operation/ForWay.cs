using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// way弾を発射するときなどのためのfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/way", fileName = "ForWay", order = 0)]
[System.Serializable]
public class ForWay : ForBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationFloatVariable m_Value;

    /// <summary>
    /// 中心の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Center;

    /// <summary>
    /// 全体の幅
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Range;

    /// <summary>
    /// ループの回数
    /// </summary>
    [SerializeField]
    private OperationIntBase m_TimeNum;

    /// <summary>
    /// インデックス
    /// </summary>
    [SerializeField]
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
        int timeNum = m_TimeNum.GetResultInt();

        if (m_Index < timeNum)
        {
            float range = m_Range.GetResultFloat();
            m_Value.Value = m_Center.GetResultFloat() - range / 2 + m_Index * range / (timeNum - 1);
            return true;
        }
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
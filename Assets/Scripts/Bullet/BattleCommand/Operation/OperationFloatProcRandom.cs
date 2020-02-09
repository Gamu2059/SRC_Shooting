using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の操作のある乱数の演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/process/random", fileName = "OperationFloatProcRandom", order = 0)]
[System.Serializable]
public class OperationFloatProcRandom : OperationFloatProcBase
{

    /// <summary>
    /// 値
    /// </summary>
    //[SerializeField]
    private float m_Value;

    /// <summary>
    /// 最小値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Min;

    /// <summary>
    /// 最大値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Max;


    public override float GetResultFloat()
    {
        return m_Value;
    }


    public override void Init()
    {
        m_Value = Random.Range(m_Min.GetResultFloat(), m_Max.GetResultFloat());
    }


    public override void Process()
    {
        m_Value = Random.Range(m_Min.GetResultFloat(), m_Max.GetResultFloat());
    }
}

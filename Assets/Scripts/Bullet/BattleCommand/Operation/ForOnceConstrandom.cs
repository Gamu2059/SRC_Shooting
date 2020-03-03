using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 開始時に一度だけランダムなfloat値を生成する、ループしないfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/once/constrandom", fileName = "ForOnceConstrandom", order = 0)]
[System.Serializable]
public class ForOnceConstrandom : ForOnceBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationFloatVariable m_FloatValue;

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


    public override void Setup()
    {
        m_FloatValue.Value = Random.Range(m_Min.GetResultFloat(), m_Max.GetResultFloat());
    }


    public override void Do()
    {

    }
}
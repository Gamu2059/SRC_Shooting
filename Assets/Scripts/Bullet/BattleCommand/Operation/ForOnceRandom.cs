using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ランダムなfloat値を生成する、一度しかループしないfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/once/random", fileName = "ForOnceRandom", order = 0)]
[System.Serializable]
public class ForOnceRandom : ForOnceBase
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


    public override void Do()
    {
        m_FloatValue.Value = Random.Range(m_Min.GetResultFloat(), m_Max.GetResultFloat());
    }
}





//public override int GetValueInt()
//{
//    return m_Value.Value;
//}

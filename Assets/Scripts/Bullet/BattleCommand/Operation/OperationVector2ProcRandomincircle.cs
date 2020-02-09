using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2型の操作のある円内の乱ベクトルの演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/process/randomincircle", fileName = "OperationVector2ProcRandomincircle", order = 0)]
[System.Serializable]
public class OperationVector2ProcRandomincircle : OperationVector2ProcBase
{

    /// <summary>
    /// 値
    /// </summary>
    //[SerializeField]
    private Vector2 m_Value;

    /// <summary>
    /// 半径
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Radius;


    public override Vector2 GetResultVector2()
    {
        return m_Value;
    }


    public override void Init()
    {
        Process();
    }


    public override void Process()
    {
        m_Value = Random.insideUnitCircle * m_Radius.GetResultFloat();
    }
}

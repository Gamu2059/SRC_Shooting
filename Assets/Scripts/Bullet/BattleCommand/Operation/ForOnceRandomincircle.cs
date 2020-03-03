using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 円内のランダムなVector2値を生成する、一度しかループしないfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/once/randomincircle", fileName = "ForOnceRandomincircle", order = 0)]
[System.Serializable]
public class ForOnceRandomincircle : ForOnceBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationVector2Variable Vector2Value;

    /// <summary>
    /// 半径
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Radius;


    public override void Setup()
    {

    }


    public override void Do()
    {
        Vector2Value.Value = Random.insideUnitCircle * m_Radius.GetResultFloat();
    }
}





//public override int GetValueInt()
//{
//    return m_Value.Value;
//}

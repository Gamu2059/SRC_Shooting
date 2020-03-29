using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾が持つfloat型の配列の1要素を取得するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/readonly/bulletFloat", fileName = "BulletFloat", order = 0)]
[System.Serializable]
public class BulletFloat : OperationFloatBase
{
    /// <summary>
    /// 現在ロードされている、弾が持つfloat型の配列
    /// </summary>
    public static float[] FloatArray { set; private get; }

    /// <summary>
    /// 配列のインデックス
    /// </summary>
    [SerializeField]
    private int m_Index;


    public override float GetResultFloat()
    {
        return FloatArray[m_Index];
    }
}

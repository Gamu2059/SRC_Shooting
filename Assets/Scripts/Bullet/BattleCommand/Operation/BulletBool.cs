#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾が持つbool型の配列の1要素を取得するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/readonly/bulletBool", fileName = "BulletBool", order = 0)]
[System.Serializable]
public class BulletBool : OperationBoolBase
{
    /// <summary>
    /// 現在ロードされている、弾が持つbool型の配列
    /// </summary>
    public static bool[] BoolArray { set; private get; }

    /// <summary>
    /// 現在ロードされている、弾が持つ変更可能なbool型の配列
    /// </summary>
    public static bool[] BoolArrayChangeable { set; private get; }

    /// <summary>
    /// 参照するのは変更可能な方の配列かどうか
    /// </summary>
    [SerializeField]
    private bool m_IsChangeable;

    /// <summary>
    /// 配列のインデックス
    /// </summary>
    [SerializeField]
    private int m_Index;


    public override bool GetResultBool()
    {
        if (!m_IsChangeable)
        {
            return BoolArray[m_Index];
        }
        else
        {
            return BoolArrayChangeable[m_Index];
        }
    }
}

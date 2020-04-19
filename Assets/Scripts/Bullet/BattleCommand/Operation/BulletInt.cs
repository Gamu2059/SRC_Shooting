#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾が持つint型の配列の1要素を取得するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/readonly/bulletInt", fileName = "BulletInt", order = 0)]
[System.Serializable]
public class BulletInt : OperationIntBase
{
    /// <summary>
    /// 現在ロードされている、弾が持つint型の配列
    /// </summary>
    public static int[] IntArray { set; private get; }

    /// <summary>
    /// 現在ロードされている、弾が持つ変更可能なint型の配列
    /// </summary>
    public static int[] IntArrayChangeable { set; private get; }

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


    public override float GetResultFloat()
    {
        return GetResultInt();
    }

    public override int GetResultInt()
    {
        if (!m_IsChangeable)
        {
            return IntArray[m_Index];
        }
        else
        {
            return IntArrayChangeable[m_Index];
        }
    }
}

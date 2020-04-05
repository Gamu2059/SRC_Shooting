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
        return IntArray[m_Index];
    }
}

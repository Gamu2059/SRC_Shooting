#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾が持つVector2型の配列の1要素を取得するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/readonly/bulletVector2", fileName = "BulletVector2", order = 0)]
[System.Serializable]
public class BulletVector2 : OperationVector2Base
{
    /// <summary>
    /// 現在ロードされている、弾が持つfloat型の配列
    /// </summary>
    public static Vector2[] Vector2Array { set; private get; }

    /// <summary>
    /// 配列のインデックス
    /// </summary>
    [SerializeField]
    private int m_Index;


    public override Vector2 GetResultVector2()
    {
        return Vector2Array[m_Index];
    }
}

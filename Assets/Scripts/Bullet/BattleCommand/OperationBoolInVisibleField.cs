#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハッキングのフィールドの見える範囲内であるかどうかのbool型を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/bool/inVisibleField", fileName = "OperationBoolInVisibleField", order = 0)]
[System.Serializable]
public class OperationBoolInVisibleField : OperationBoolBase
{

    /// <summary>
    /// Vector2型の値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Vector2;


    public override bool GetResultBool()
    {
        Vector2 value = m_Vector2.GetResultVector2();

        return
        BulletVisibleRange.XMin <= value.x && value.x <= BulletVisibleRange.XMax &&
        BulletVisibleRange.YMin <= value.y && value.y <= BulletVisibleRange.YMax;
    }
}

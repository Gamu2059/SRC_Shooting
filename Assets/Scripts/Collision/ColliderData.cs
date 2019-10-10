using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 衝突判定情報。
/// </summary>
[Serializable]
public class ColliderData
{
    /// <summary>
    /// 衝突判定の名前。
    /// </summary>
    public string CollideName;

    /// <summary>
    /// 矩形か楕円か。
    /// </summary>
    public E_COLLIDER_SHAPE ColliderType;

    /// <summary>
    /// 中心座標。
    /// </summary>
    public Vector2 CenterPos;

    /// <summary>
    /// サイズ。
    /// </summary>
    public Vector2 Size;

    /// <summary>
    /// 回転(度数法)。
    /// </summary>
    public float Angle;

    /// <summary>
    /// 衝突したか。
    /// </summary>
    public bool IsCollide;
}

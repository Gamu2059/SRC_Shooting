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
    public ColliderTransform Transform;

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

    /// <summary>
    /// この当たり判定の外側の矩形
    /// </summary>
    public Rect OutSideRect;
}
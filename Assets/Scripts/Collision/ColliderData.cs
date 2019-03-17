using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 衝突判定情報。
/// </summary>
[System.Serializable]
public struct ColliderData
{
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
}
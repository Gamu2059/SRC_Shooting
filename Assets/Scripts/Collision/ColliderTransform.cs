using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 衝突判定に用いるトランスフォームの情報。
/// </summary>
[System.Serializable]
public struct ColliderTransform
{
	/// <summary>
	/// 衝突判定に用いるトランスフォームの形状。
	/// </summary>
	public E_COLLIDER_SHAPE ColliderType;

	/// <summary>
	/// 衝突判定に用いるトランスフォーム。
	/// </summary>
	public Transform Transform;
}
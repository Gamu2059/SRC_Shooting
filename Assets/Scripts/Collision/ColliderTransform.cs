using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 衝突判定に用いるトランスフォームの情報。
/// </summary>
[Serializable]
public class ColliderTransform
{
    public enum E_DIRECTION
    {
        VERTICAL,
        HORIZONTAL,
    }

    /// <summary>
    /// 衝突判定の名前。
    /// </summary>
    public string CollideName;

	/// <summary>
	/// 衝突判定に用いるトランスフォームの形状。
	/// </summary>
	public E_COLLIDER_SHAPE ColliderType;

    /// <summary>
    /// 基準にする向き
    /// </summary>
    public E_DIRECTION Direction;

    /// <summary>
    /// デバッグ描画時の太さ。
    /// </summary>
    public float ColliderWidth = 5;

    /// <summary>
    /// デバッグ描画時の色。
    /// </summary>
    public Color ColliderColor;

	/// <summary>
	/// 衝突判定に用いるトランスフォーム。
	/// </summary>
	public Transform Transform;
}
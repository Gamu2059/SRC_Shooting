using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 衝突情報を管理するコンポーネント。
/// </summary>
public class BattleObjectCollider : MonoBehaviour
{
	/// <summary>
	/// 衝突判定を保持する。
	/// </summary>
	[SerializeField]
	private ColliderTransform[] m_ColliderTransforms;

	/// <summary>
	/// これは、他の弾にもヒットする弾かどうか。
	/// </summary>
	[SerializeField]
	private bool m_CanHitOtherBullet;

	public ColliderData[] GetColliderData()
	{
		int hitNum = m_ColliderTransforms.Length;
		var colliders = new ColliderData[hitNum];

		for( int i = 0; i < hitNum; i++ )
		{
			Transform t = m_ColliderTransforms[i].Transform;
			var c = new ColliderData();
			c.CenterPos = new Vector2( t.position.x, t.position.z );
			c.Size = new Vector2( t.lossyScale.x, t.lossyScale.z );
			c.Angle = -t.eulerAngles.y;
			c.ColliderType = m_ColliderTransforms[i].ColliderType;

			colliders[i] = c;
		}

		return colliders;
	}

	public bool CanHitBullet()
	{
		return m_CanHitOtherBullet;
	}
}

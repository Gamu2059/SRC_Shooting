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
	/// 衝突情報トランスフォームを保持する。
	/// </summary>
	[SerializeField]
	private ColliderTransform[] m_ColliderTransforms;

    /// <summary>
    /// コライダーデータを生成する。
    /// </summary>
    /// <returns></returns>
	public ColliderData[] CreateColliderData()
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
            c.CollideName = m_ColliderTransforms[i].CollideName;

			colliders[i] = c;
		}

		return colliders;
	}
}

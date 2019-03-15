﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 弾と弾道パラメータをリスト保持するパラメータアセット。
/// </summary>
[CreateAssetMenu( menuName = "Param/Stage Param/Bullet Set Param", fileName = "BulletSetParam", order = 0 )]
[Serializable]
public class BulletSetParam : ScriptableObject
{
	[SerializeField, Tooltip( "弾のプレハブの配列" )]
	private BulletController[] m_BulletPrefabs;

	[SerializeField, Tooltip( "弾道パラメータの配列" )]
	private BulletParam[] m_BulletParams;

	/// <summary>
	/// BulletPrefab配列を取得する。
	/// </summary>
	public BulletController[] GetBulletPrefabs()
	{
		return m_BulletPrefabs;
	}

	/// <summary>
	/// BulletParam配列を取得する。
	/// </summary>
	public BulletParam[] GetBulletParams()
	{
		return m_BulletParams;
	}
}

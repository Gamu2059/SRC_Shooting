using System.Collections;
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

    [SerializeField, Tooltip("ボムのプレハブの配列")]
    private BulletController[] m_BombPrefabs;

    [SerializeField, Tooltip("ボムのパラメータの配列")]
    private BulletParam[] m_BombParams;

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

    /// <summary>
    /// BombPrefabs配列を取得する。
    /// </summary>
    public BulletController[] GetBombPrefabs()
    {
        return m_BombPrefabs;
    }

    /// <summary>
    /// BombParams配列を取得する。
    /// </summary>
    public BulletParam[] GetBombParams()
    {
        return m_BombParams;
    }

    /// <summary>
    /// BulletPrefabの個数を取得する。
    /// </summary>
    public int GetBulletPrefabsCount()
    {
        if (m_BulletPrefabs == null)
        {
            return 0;
        }

        return m_BulletPrefabs.Length;
    }

    /// <summary>
    /// 指定したインデックスのBulletPrefabを取得する。
    /// </summary>
    public BulletController GetBulletPrefab(int bulletIndex = 0)
    {
        int count = GetBulletPrefabsCount();

        if (count < 1)
        {
            return null;
        }

        bulletIndex = Mathf.Clamp(bulletIndex, 0, count);
        return m_BulletPrefabs[bulletIndex];
    }

    /// <summary>
	/// BulletParamの個数を取得する。
	/// </summary>
	public int GetBulletParamsCount()
    {
        if (m_BulletParams == null)
        {
            return 0;
        }

        return m_BulletParams.Length;
    }

    /// <summary>
    /// 指定したインデックスのBulletParamを取得する。
    /// </summary>
    public BulletParam GetBulletParam(int bulletParamIndex = 0)
    {
        int count = GetBulletParamsCount();

        if (count < 1)
        {
            return null;
        }

        bulletParamIndex = Mathf.Clamp(bulletParamIndex, 0, count);
        return m_BulletParams[bulletParamIndex];
    }

    /// <summary>
    /// BombPrefabの個数を取得する。
    /// </summary>
    public int GetBombPrefabsCount()
    {
        if (m_BombPrefabs == null)
        {
            return 0;
        }

        return m_BombPrefabs.Length;
    }

    /// <summary>
    /// 指定したインデックスのBombPrefabを取得する。
    /// </summary>
    public BulletController GetBombPrefab(int bombIndex = 0)
    {
        int count = GetBombPrefabsCount();

        if (count < 1)
        {
            return null;
        }

        bombIndex = Mathf.Clamp(bombIndex, 0, count);
        return m_BombPrefabs[bombIndex];
    }

    /// <summary>
	/// BombParamの個数を取得する。
	/// </summary>
	public int GetBombParamsCount()
    {
        if (m_BombParams == null)
        {
            return 0;
        }

        return m_BombParams.Length;
    }

    /// <summary>
    /// 指定したインデックスのBombParamを取得する。
    /// </summary>
    public BulletParam GetBombParam(int bombParamIndex = 0)
    {
        int count = GetBombParamsCount();

        if (count < 1)
        {
            return null;
        }

        bombParamIndex = Mathf.Clamp(bombParamIndex, 0, count);
        return m_BombParams[bombParamIndex];
    }
}

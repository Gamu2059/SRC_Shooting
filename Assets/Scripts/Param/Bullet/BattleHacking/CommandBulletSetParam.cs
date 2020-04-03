#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードの弾と弾道パラメータをリスト保持するパラメータアセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Bullet/BulletSet", fileName = "param.bullet_set.asset", order = 0)]
public class CommandBulletSetParam : ScriptableObject
{
    [SerializeField, Tooltip("弾のプレハブの配列")]
    private BattleHackingFreeTrajectoryBulletController[] m_BulletPrefabs;

    [SerializeField, Tooltip("弾道パラメータの配列")]
    private BulletParam[] m_BulletParams;

    /// <summary>
    /// BulletPrefab配列を取得する。
    /// </summary>
    public BattleHackingFreeTrajectoryBulletController[] GetBulletPrefabs()
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

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// コマンドイベントの弾と弾道パラメータをリスト保持するパラメータアセット。
/// </summary>
[CreateAssetMenu(menuName = "Param/Stage Param/Command Bullet Set Param", fileName = "CommandBulletSetParam", order = 0)]
[Serializable]
public class CommandBulletSetParam : ScriptableObject
{
    [SerializeField, Tooltip("弾のプレハブの配列")]
    private CommandBulletController[] m_BulletPrefabs;

    [SerializeField, Tooltip("弾道パラメータの配列")]
    private BulletParam[] m_BulletParams;

    /// <summary>
    /// BulletPrefab配列を取得する。
    /// </summary>
    public CommandBulletController[] GetBulletPrefabs()
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

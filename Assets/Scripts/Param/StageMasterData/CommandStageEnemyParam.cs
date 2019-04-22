using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ステージに出現する敵のデータを保持するパラメータアセット。
/// </summary>
[CreateAssetMenu(menuName = "Param/Stage Param/CommandStage Enemy Param", fileName = "CommandStageEnemyParam", order = 1)]
[Serializable]
public class CommandStageEnemyParam : ScriptableObject
{
    [SerializeField, Tooltip("敵の見た目のプレハブ配列")]
    private GameObject[] m_EnemyPrefabs;

    [SerializeField, Tooltip("敵の動作の配列")]
    private CommandEnemyController[] m_EnemyControllers;

    [SerializeField, Tooltip("敵が使う弾のパラメータセット配列")]
    private CommandBulletSetParam[] m_BulletSets;

    /// <summary>
    /// 敵の見た目のプレハブ配列を取得する。
    /// </summary>
    public GameObject[] GetEnemyPrefabs()
    {
        return m_EnemyPrefabs;
    }

    /// <summary>
    /// 敵の動作の配列を取得する。
    /// </summary>
    public CommandEnemyController[] GetEnemyControllers()
    {
        return m_EnemyControllers;
    }

    /// <summary>
    /// 敵が使う弾のパラメータセット配列を取得する。
    /// </summary>
    public CommandBulletSetParam[] GetBulletSets()
    {
        return m_BulletSets;
    }
}

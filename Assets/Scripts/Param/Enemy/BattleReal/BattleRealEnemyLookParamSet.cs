using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵の見た目のパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleRealEnemy/EnemyLook", fileName = "param.battle_real_enemy_look")]
public class BattleRealEnemyLookParamSet : ScriptableObject
{
    [SerializeField, Tooltip("敵のプールID これの名前が同じならば、プールから取得するモデルを再利用できます")]
    private string m_EnemyId;
    public string EnemyId => m_EnemyId;

    [SerializeField, Tooltip("敵のプレハブ")]
    private EnemyController m_EnemyPrefab;
    public EnemyController EnemyPrefab => m_EnemyPrefab;
}
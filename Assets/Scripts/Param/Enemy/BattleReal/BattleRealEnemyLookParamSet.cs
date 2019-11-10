using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵の見た目のパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyLook", fileName = "param.battle_real_enemy_look.asset")]
public class BattleRealEnemyLookParamSet : ScriptableObject
{
    [SerializeField, Tooltip("敵のプールID これの名前が同じならば、プールから取得するモデルを再利用できます")]
    private string m_LookId = default;
    public string LookId => m_LookId;

    [SerializeField, Tooltip("敵のプレハブ")]
    private GameObject m_EnemyPrefab = default;
    public GameObject EnemyPrefab => m_EnemyPrefab;
}
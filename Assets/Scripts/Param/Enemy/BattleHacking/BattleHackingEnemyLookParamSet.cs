#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードの敵の見た目のパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Enemy/EnemyLook", fileName = "param.battle_hacking_enemy_look.asset")]
public class BattleHackingEnemyLookParamSet : ScriptableObject
{
    [SerializeField, Tooltip("敵のプールID これの名前が同じならば、プールから取得するモデルを再利用できます")]
    private string m_LookId;
    public string LookId => m_LookId;

    [SerializeField, Tooltip("敵のプレハブ")]
    private GameObject m_EnemyPrefab;
    public GameObject EnemyPrefab => m_EnemyPrefab;
}

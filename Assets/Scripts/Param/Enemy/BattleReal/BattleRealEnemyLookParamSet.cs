#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵の見た目のパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyLook", fileName = "param.enemy_look.asset")]
public class BattleRealEnemyLookParamSet : ScriptableObject
{
    [SerializeField, Tooltip("敵のプールID これの名前が同じならば、プールから取得するモデルを再利用できます")]
    private string m_LookId;
    public string LookId => m_LookId;

    [SerializeField, Tooltip("敵のプレハブ")]
    private GameObject m_EnemyPrefab;
    public GameObject EnemyPrefab => m_EnemyPrefab;
}
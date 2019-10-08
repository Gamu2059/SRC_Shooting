using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵の生成パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleRealEnemy/EnemyGenerate", fileName = "param.battle_real_enemy_generate")]
public class BattleRealEnemyGenerateParamSet : ScriptableObject
{
    [SerializeField, Tooltip("敵の体力")]
    private int m_Hp;
    public int Hp => m_Hp;

    // ドロップアイテム

    // 撃破時イベント

    [SerializeField, Tooltip("敵の振る舞いパラメータのセット")]
    private BattleRealEnemyBehaviorParamSet m_EnemyBehaviorParamSet;
    public BattleRealEnemyBehaviorParamSet EnemyBehaviorParamSet => m_EnemyBehaviorParamSet;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵の生成パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyGenerate", fileName = "param.battle_real_enemy_generate.asset")]
public class BattleRealEnemyGenerateParamSet : ScriptableObject
{
    [SerializeField, Tooltip("敵の体力")]
    private int m_Hp;
    public int Hp => m_Hp;

    [SerializeField, Tooltip("ドロップアイテム")]
    private ItemCreateParam m_ItemCreateParam;
    public ItemCreateParam ItemCreateParam => m_ItemCreateParam;

    // 撃破時イベント

    [SerializeField, Tooltip("敵の振る舞いパラメータのセット")]
    private BattleRealEnemyBehaviorParamSet m_EnemyBehaviorParamSet;
    public BattleRealEnemyBehaviorParamSet EnemyBehaviorParamSet => m_EnemyBehaviorParamSet;
}
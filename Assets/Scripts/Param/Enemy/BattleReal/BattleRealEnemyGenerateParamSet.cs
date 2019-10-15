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

    [SerializeField, Tooltip("撃破時の獲得スコア")]
    private int m_Score;
    public int Score => m_Score;

    [SerializeField, Tooltip("ドロップアイテム")]
    private ItemCreateParam m_ItemCreateParam;
    public ItemCreateParam ItemCreateParam => m_ItemCreateParam;

    [SerializeField, Tooltip("撃破時のイベント")]
    private BattleRealEventContent[] m_DefeatEvents;
    public BattleRealEventContent[] DefeatEvents => m_DefeatEvents;

    [SerializeField, Tooltip("敵の振る舞いパラメータのセット")]
    private BattleRealEnemyBehaviorParamSet m_EnemyBehaviorParamSet;
    public BattleRealEnemyBehaviorParamSet EnemyBehaviorParamSet => m_EnemyBehaviorParamSet;
}
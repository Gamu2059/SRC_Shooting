using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードの敵の生成パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Enemy/EnemyGenerate", fileName = "param.battle_hacking_enemy_generate.asset")]
public class BattleHackingEnemyGenerateParamSet : ScriptableObject
{
    [SerializeField, Tooltip("敵の体力")]
    private int m_Hp;
    public int Hp => m_Hp;

    [SerializeField, Tooltip("撃破時の獲得スコア")]
    private int m_Score;
    public int Score => m_Score;

    [SerializeField, Tooltip("ボスかどうか")]
    private bool m_IsBoss;
    public bool IsBoss => m_IsBoss;
}
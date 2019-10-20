using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyBehavior/ToPlayer", fileName = "behavior.battle_real_enemy_to_player.asset")]
public class BattleRealEnemyToPlayerParamSet : BattleRealEnemyUturnParamSet
{
    [Header("To Player Param")]

    [SerializeField, Tooltip("最初の直線移動時の補完係数")]
    private float m_StraightLerp;
    public float StraightLerp => m_StraightLerp;

    [SerializeField, Tooltip("最初の直線移動にかける時間")]
    private float m_StraightDuration;
    public float StraightDuration => m_StraightDuration;

    [SerializeField, Tooltip("WAITステータスの時の待機時間")]
    private float m_WaitTime;
    public float WaitTime => m_WaitTime;
}

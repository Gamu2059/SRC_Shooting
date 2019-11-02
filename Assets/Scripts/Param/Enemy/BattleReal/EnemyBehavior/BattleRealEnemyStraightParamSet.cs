using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyBehavior/Straight", fileName = "behavior.battle_real_enemy_straight.asset")]

public class BattleRealEnemyStraightParamSet : BattleRealEnemyBehaviorParamSet
{
    [Header("Move Param")]

    [SerializeField]
    private Vector3 m_MoveDirection;
    public Vector3 MoveDirection => m_MoveDirection;

    [SerializeField]
    private AnimationCurve m_MoveDistance;
    public AnimationCurve MoveDistance => m_MoveDistance;

    [Header("Shot Param")]

    [SerializeField]
    private EnemyShotParam m_ShotParam;
    public EnemyShotParam ShotParam => m_ShotParam;
}

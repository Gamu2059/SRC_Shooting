using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName ="Param/BattleReal/Enemy/EnemyBehavior/NWayPointMove",fileName ="behavior.nway_point_move.asset")]
public class NWayPointMoveEnemyParamSet : PointMoveEnemyParamSet
{
    [Header("NWay param")]

    [SerializeField]
    private int m_Num = default;
    public int Num => m_Num;

    [SerializeField]
    private float m_Radius = default;
    public float Radius => m_Radius;

    [SerializeField]
    private float m_AngleOffset = default;
    public float AngleOffset => m_AngleOffset;
}

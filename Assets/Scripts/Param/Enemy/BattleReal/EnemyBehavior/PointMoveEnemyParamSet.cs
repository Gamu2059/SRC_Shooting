using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializeField]
public class Destination
{
    [SerializeField]
    public Vector3 m_Destination;

    [SerializeField]
    public AnimationCurve m_MoveSpeedCurve;
}

[Serializable, CreateAssetMenu(menuName ="Param/BattleREal/Enemy/EnemyBehavior/PointMove", fileName ="behavior.point_move.asset")]
public class PointMoveEnemyParamSet : BattleRealEnemyBehaviorParamSet
{
    [Header("Move Param")]

    [SerializeField]
    private Destination[] m_Destinations;
    public Destination[] Destinations => m_Destinations;

    [SerializeField]
    private Destination m_Exit;
    public Destination Exit => m_Exit;

    [Header("Shot Param")]
    [SerializeField]
    private float m_ShotOffset = default;
    public float ShotOffset => m_ShotOffset;

    [SerializeField]
    private float m_ShotStop = default;
    public float ShotStop => m_ShotStop;

    [SerializeField]
    private EnemyShotParam m_ShotParam = default;
    public EnemyShotParam ShotParam => m_ShotParam;

    [Header("Ring Param")]
    private bool m_IsStartAnimationOnStart = default;
    public bool IsStartAnimationOnStart => m_IsStartAnimationOnStart;


}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// INF-C-761の第2段階の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/INF-C-761/Phase2", fileName = "phase2.inf_c_761.asset", order = 2)]
public class InfC761Phase2Behavior : BattleRealEnemyBehaviorUnit
{
    #region Field Inspector Readonly!

    [SerializeField]
    private Vector3 m_BasePos = default;
    public Vector3 BasePos => m_BasePos;

    [SerializeField]
    private float m_StartDuration;
    public float StartDuration => m_StartDuration;

    [Header("Move Param")]
    [SerializeField]
    private BossMoveParam[] m_MoveParams;
    public BossMoveParam[] MoveParams => m_MoveParams;

    [SerializeField]
    private float[] m_Amplitudes;
    public float[] Amplitudes => m_Amplitudes;

    [SerializeField]
    private float[] m_NextMoveWaitTimes;
    public float[] NextMoveWaitTimes => m_NextMoveWaitTimes;

    [SerializeField]
    private float[] m_MoveDurations;
    public float[] MoveDurations => m_MoveDurations;

    [SerializeField]
    private AnimationCurve[] m_NormalizedRates;
    public AnimationCurve[] NormalizedRates => m_NormalizedRates;

    [SerializeField]
    private float[] m_GenericDurations;
    public float[] GenericDurations => m_GenericDurations;

    [Header("Shot Param")]
    [SerializeField]
    private float[] m_ShotCoolTimes;
    public float[] ShotCoolTimes => m_ShotCoolTimes;

    [SerializeField]
    private EnemyShotParam[] m_ShotParams = default;
    public EnemyShotParam[] ShotParams => m_ShotParams;

    [SerializeField]
    private Vector3[] m_ShotOffSets;
    public Vector3[] ShotOffSets => m_ShotOffSets;

    [SerializeField]
    private int m_NumberOfShotRapidPLookSmallRectBullet;
    public int NumberOfShotRapidPLookSmallRectBullet => m_NumberOfShotRapidPLookSmallRectBullet;

    #endregion
}

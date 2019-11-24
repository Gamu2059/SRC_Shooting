#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// INF-C-761の一つ目の行動パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/INF-C-761/Phase1Old", fileName = "param.inf_c_761_phase_1_OLD.asset")]
public class InfC761Phase1ParamSetOld : BattleRealBossBehaviorParamSet
{
    [SerializeField]
    private Vector3 m_BasePos;
    public Vector3 BasePos => m_BasePos;

    [SerializeField]
    private float m_Amplitude;
    public float Amplitude => m_Amplitude;

    [SerializeField]
    private AnimationCurve m_NormalizedRate;
    public AnimationCurve NormalizedRate => m_NormalizedRate;

    [SerializeField]
    private float m_NextMoveWaitTime;
    public float NextMoveWaitTime => m_NextMoveWaitTime;

    [SerializeField]
    private float m_StartDuration;
    public float StartDuration => m_StartDuration;

    [SerializeField]
    private float m_MoveDuration;
    public float MoveDuration => m_MoveDuration;

    [Header("Shot Param")]

    [SerializeField]
    private EnemyShotParam[] m_ShotParams;
    public EnemyShotParam[] ShotParams => m_ShotParams;

    [SerializeField]
    private Vector3 m_LeftShotOffset;
    public Vector3 LeftShotOffset => m_LeftShotOffset;

    [SerializeField]
    private Vector3 m_RightShotOffset;
    public Vector3 RigthShotOffset => m_RightShotOffset;

    [SerializeField]
    private Vector3 m_CenterShotOffset;
    public Vector3 CenterShotOffset => m_CenterShotOffset;

    [SerializeField]
    private int m_NShotsNum;
    public int NShotsNum => m_NShotsNum;

    [SerializeField]
    private float m_NShotsInterval;
    public float NShotsInterval => m_NShotsInterval;
}

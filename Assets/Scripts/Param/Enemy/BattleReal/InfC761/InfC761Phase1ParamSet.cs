using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// INF-C-761の一つ目の行動パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/INF-C-761/Phase1", fileName = "param.inf_c_761_phase_1.asset")]
public class InfC761Phase1ParamSet : BattleRealBossBehaviorParamSet
{
    [SerializeField]
    private Vector3 m_BasePos = default;
    public Vector3 BasePos => m_BasePos;

    [SerializeField]
    private float m_Amplitude = default;
    public float Amplitude => m_Amplitude;

    [SerializeField]
    private AnimationCurve m_NormalizedRate = default;
    public AnimationCurve NormalizedRate => m_NormalizedRate;

    [SerializeField]
    private float m_NextMoveWaitTime = default;
    public float NextMoveWaitTime => m_NextMoveWaitTime;

    [SerializeField]
    private float m_StartDuration = default;
    public float StartDuration => m_StartDuration;

    [SerializeField]
    private float m_MoveDuration = default;
    public float MoveDuration => m_MoveDuration;

    [Header("Shot Param")]

    [SerializeField]
    private EnemyShotParam[] m_ShotParams = default;
    public EnemyShotParam[] ShotParams => m_ShotParams;

    [SerializeField]
    private Vector3 m_LeftShotOffset = default;
    public Vector3 LeftShotOffset => m_LeftShotOffset;

    [SerializeField]
    private Vector3 m_RightShotOffset = default;
    public Vector3 RigthShotOffset => m_RightShotOffset;

    [SerializeField]
    private Vector3 m_CenterShotOffset = default;
    public Vector3 CenterShotOffset => m_CenterShotOffset;

    [SerializeField]
    private int m_NShotsNum = default;
    public int NShotsNum => m_NShotsNum;

    [SerializeField]
    private float m_NShotsInterval = default;
    public float NShotsInterval => m_NShotsInterval;
}

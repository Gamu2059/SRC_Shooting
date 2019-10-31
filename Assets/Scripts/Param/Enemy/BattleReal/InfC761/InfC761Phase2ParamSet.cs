﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// INF-C-761の二つ目の行動パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/INF-C-761/Phase2", fileName = "param.inf_c_761_phase_2.asset")]
public class InfC761Phase2ParamSet : BattleRealBossBehaviorParamSet
{
    [Serializable]
    public class NShotsPreset
    {
        [SerializeField]
        private int m_NShotsNum;
        public int NShotsNum => m_NShotsNum;

        [SerializeField]
        private float m_NShotsDelay;
        public float NShotsDelay => m_NShotsDelay;
    }

    [SerializeField]
    private Vector3 m_BasePos;
    public Vector3 BasePos => m_BasePos;

    [SerializeField]
    private float m_Radius;
    public float Radius => m_Radius;

    [SerializeField]
    private float m_ArcAngle;
    public float ArcAngle => m_ArcAngle;

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

    [Header("N_Shots Presets"), Tooltip("連射数、連射間隔のセットを保持します")]

    [SerializeField]
    private NShotsPreset[] m_NShotsPresets;
    public NShotsPreset[] NShotsPresets => m_NShotsPresets;
}

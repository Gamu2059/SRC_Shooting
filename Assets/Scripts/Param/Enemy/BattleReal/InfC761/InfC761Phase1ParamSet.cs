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
    private Vector2 m_BasePos;
    public Vector2 BasePos => m_BasePos;

    [SerializeField]
    private float m_Amplitude;
    public float Amplitude => m_Amplitude;

    [SerializeField]
    private AnimationCurve m_NormalizedRate;
    public AnimationCurve NormalizedRate => m_NormalizedRate;

    [SerializeField]
    private float m_NextMoveWaitTime;
    public float NextMoveWaitTime => m_NextMoveWaitTime;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// INF-C-761の一つ目の行動パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/INF-C-761/PhaseN", fileName = "param.inf_c_761_phase_N.asset")]
public class InfC761BehaviorParamSet : BattleRealBossBehaviorParamSet
{
    /// <summary>
    /// ボスの移動についての設定
    /// Destination[0],Destination[1]...Destination[N]
    /// と移動した後どう動くか
    /// </summary>
    [Serializable]
    public enum E_LOOP_SETTING{
        /// <summary>
        /// 一方通行
        /// Destination[N]まで動いたらもう移動しない
        /// </summary>
        ONE_WAY,
        /// <summary>
        /// 往復
        /// Destination[N],Destination[N-1]...Destination[0]と移動
        /// </summary>
        ROUND_TRIP,
        /// <summary>
        /// ループ
        /// BasePosに戻りDestination[0]からの移動を再度繰り返す
        /// </summary>
        LOOP,
    }

    [SerializeField]
    private Vector3 m_BasePos;
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
    private EnemyShotParam[] m_ShotParams;
    public EnemyShotParam[] ShotParams => m_ShotParams;

    [SerializeField]
    private Vector3[] m_ShotOffSets;
    public Vector3[] ShotOffSets => m_ShotOffSets;
}
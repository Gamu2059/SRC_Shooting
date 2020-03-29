using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, Obsolete]
public class AnimationCurveEnemyParamSet : BattleRealEnemyBehaviorParamSet
{
    [Header("Move Param")]

    [SerializeField]
    private AnimationCurve m_SpeedCurve = default;
    public AnimationCurve SpeedCurve => m_SpeedCurve;

    [SerializeField]
    private AnimationCurve m_AngleSpeedCurve = default;
    public AnimationCurve AngleSpeedCurve => m_AngleSpeedCurve;

    [Header("Shot Param")]

    [SerializeField, Tooltip("出現から最初の発射までのオフセット")]
    private float m_ShotOffset = default;
    public float ShotOffset => m_ShotOffset;

    [SerializeField, Tooltip("「最初に発射してから」撃ち止めるまでの時間")]
    private float m_ShotStop = default;
    public float ShotStop => m_ShotStop;

    [SerializeField]
    private EnemyShotParam m_ShotParam = default;
    public EnemyShotParam ShotParam => m_ShotParam;

    [Header("Ring Param")]

    [SerializeField, Tooltip("出現時にリングアニメーションを再生するかどうか。falseの場合、最初に弾を撃つ瞬間に再生する。")]
    private bool m_IsStartAnimationOnStart = default;
    public bool IsStartAnimationOnStart => m_IsStartAnimationOnStart;
}

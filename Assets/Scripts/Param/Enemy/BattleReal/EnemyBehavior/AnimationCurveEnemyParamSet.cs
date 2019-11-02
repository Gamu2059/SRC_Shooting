using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyBehavior/AnimationCurve", fileName = "behavior.animation_curve.asset")]

public class AnimationCurveEnemyParamSet : BattleRealEnemyBehaviorParamSet
{
    [Header("Move Param")]

    [SerializeField]
    private AnimationCurve m_SpeedCurve;
    public AnimationCurve SpeedCurve => m_SpeedCurve;

    [SerializeField]
    private AnimationCurve m_AngleSpeedCurve;
    public AnimationCurve AngleSpeedCurve => m_AngleSpeedCurve;

    [Header("Shot Param")]

    [SerializeField, Tooltip("出現から最初の発射までのオフセット")]
    private float m_ShotOffset;
    public float ShotOffset => m_ShotOffset;

    [SerializeField, Tooltip("「最初に発射してから」撃ち止めるまでの時間")]
    private float m_ShotStop;
    public float ShotStop => m_ShotStop;

    [SerializeField]
    private EnemyShotParam m_ShotParam;
    public EnemyShotParam ShotParam => m_ShotParam;
}

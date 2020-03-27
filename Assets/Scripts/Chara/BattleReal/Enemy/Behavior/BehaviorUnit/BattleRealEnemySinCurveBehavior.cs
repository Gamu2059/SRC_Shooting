#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Sinカーブで動く敵の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemySequence/Unit/SinCurve", fileName = "sin_curve.behavior_unit.asset", order = 20)]
public class BattleRealEnemySinCurveBehavior : BattleRealEnemyBehaviorUnit
{
    #region Field Inspector

    [Header("Sin Curve Parameter")]

    [SerializeField, Tooltip("カーブの軸となる角度(度数法)。 0なら上、90なら右、180なら下、270なら左")]
    private float m_AxisAngle;
    public float AxisAngle => m_AxisAngle;

    [SerializeField, Tooltip("振幅")]
    private float m_Amplitude;
    public float Amplitude => m_Amplitude;

    [SerializeField, Tooltip("周波数")]
    private float m_Frequency;
    public float Frequency => m_Frequency;

    [SerializeField, Tooltip("初期位相(度数法)")]
    private float m_InitPhaseAngle;
    public float InitPhaseAngle => m_InitPhaseAngle;

    [SerializeField, Tooltip("1周期で進む距離")]
    private float m_WaveLength;
    public float WaveLength => m_WaveLength;

    [SerializeField, Tooltip("継続時間")]
    private float m_Duration;
    public float Duration => m_Duration;

    #endregion

    #region Field

    protected Vector3 m_StartPosition;
    protected float m_InitPhase;
    protected float m_Sin;
    protected float m_Cos;

    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        m_StartPosition = Enemy.transform.position;
        m_InitPhase = InitPhaseAngle * Mathf.Deg2Rad;
        var rad = (-AxisAngle + 90) * Mathf.Deg2Rad;
        m_Sin = Mathf.Sin(rad);
        m_Cos = Mathf.Cos(rad);

        Move();
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Move();
    }

    protected override bool IsEnd()
    {
        return CurrentTime >= Duration;
    }

    #endregion

    protected virtual void Move()
    {
        var x = WaveLength * Frequency * CurrentTime;
        var y = Amplitude * Mathf.Sin(Frequency * CurrentTime + m_InitPhase);
        var deltaPos = Vector3.zero;
        deltaPos.x = x * m_Cos - y * m_Sin;
        deltaPos.z = x * m_Sin + y * m_Cos;
        Enemy.transform.position = m_StartPosition + deltaPos;
    }
}

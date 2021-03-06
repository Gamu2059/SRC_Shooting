﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// アニメーションカーブによって動く敵グループの挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGroup/Behavior/Unit/AnimationCurve", fileName = "param.enemy_group_behavior.asset")]
public class BattleRealEnemyGroupAnimationCurveBehavior : BattleRealEnemyGroupBehaviorUnitBase
{
    #region Field Inspector

    [Header("Move Param")]

    [SerializeField]
    private AnimationCurve m_SpeedCurve;
    public AnimationCurve SpeedCurve => m_SpeedCurve;

    [SerializeField]
    private float m_SpeedCurveScale = 1;
    public float SpeedCurveScale => m_SpeedCurveScale;

    [SerializeField]
    private AnimationCurve m_AngleSpeedCurve;
    public AnimationCurve AngleSpeedCurve => m_AngleSpeedCurve;

    [SerializeField]
    private float m_AngleSpeedCurveScale = 1;
    public float AngleSpeedCurveScale => m_AngleSpeedCurveScale;

    #endregion

    #region Field

    private Vector3 m_StartPosition;
    private float m_StartAngle;
    private float m_MoveTimeCount;
    private float m_NowSpeed;
    private float m_NowAngleSpeed;

    #endregion

    #region Game Cycle

    public override void OnStart()
    {
        base.OnStart();

        m_StartPosition = EnemyGroup.transform.localPosition;
        m_StartAngle = EnemyGroup.transform.localEulerAngles.y;

        m_MoveTimeCount = 0;
        m_NowSpeed = 0;
        m_NowAngleSpeed = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Move();
        m_MoveTimeCount += Time.deltaTime;
    }

    #endregion

    private void Move()
    {
        m_NowSpeed = SpeedCurveScale * SpeedCurve.Evaluate(m_MoveTimeCount);
        m_NowAngleSpeed = AngleSpeedCurveScale * AngleSpeedCurve.Evaluate(m_MoveTimeCount);

        var transform = EnemyGroup.transform;
        var pos = transform.forward * m_NowSpeed * Time.deltaTime + transform.position;
        var angle = transform.eulerAngles;
        angle.y += m_NowAngleSpeed * Time.deltaTime;
        transform.SetPositionAndRotation(pos, Quaternion.Euler(angle));
    }
}

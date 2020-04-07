#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// アニメーションカーブによって動く敵の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Unit/AnimationCurve", fileName = "param.behavior_unit.asset")]
public class BattleRealEnemyAnimationCurveBehavior : BattleRealEnemyBehaviorUnitBase
{
    #region Field Inspector

    [Header("Animation Curve Parameter")]

    [SerializeField]
    private AnimationCurve m_SpeedCurve;
    public AnimationCurve SpeedCurve => m_SpeedCurve;

    [SerializeField, Tooltip("SpeedCurveにスケール補正を掛ける。判定させたい場合は-1等を指定するとよい。")]
    private float m_SpeedCurveScale = 1f;
    protected float SpeedCurveScale => m_SpeedCurveScale;

    [SerializeField]
    private AnimationCurve m_AngleSpeedCurve;
    protected AnimationCurve AngleSpeedCurve => m_AngleSpeedCurve;

    [SerializeField, Tooltip("AngleSpeedCurveにスケール補正を掛ける。判定させたい場合は-1等を指定するとよい。")]
    private float m_AngleSpeedCurveScale = 1f;
    protected float AngleSpeedCurveScale => m_AngleSpeedCurveScale;

    #endregion

    #region Field

    protected float m_NowSpeed;
    protected float m_NowAngleSpeed;
    protected float m_Duration;

    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        m_NowSpeed = 0;
        m_NowAngleSpeed = 0;

        // 速度アニメーションの経過時間をキャッシュする
        m_Duration = SpeedCurve.Duration();

        // 移動方向へと向きが補正されるのでfalseにする
        Enemy.IsLookMoveDir = false;

        Move();
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Move();
    }

    protected override bool IsEnd()
    {
        return CurrentTime >= m_Duration;
    }

    #endregion

    protected virtual void Move()
    {
        m_NowSpeed = SpeedCurveScale * SpeedCurve.Evaluate(CurrentTime);
        m_NowAngleSpeed = AngleSpeedCurveScale * AngleSpeedCurve.Evaluate(CurrentTime);

        var transform = Enemy.transform;
        transform.position = transform.forward * m_NowSpeed * Time.deltaTime + transform.position;
        var angle = transform.eulerAngles;
        angle.y += m_NowAngleSpeed * Time.deltaTime;
        transform.eulerAngles = angle;
    }
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ある座標を指定してその円周上を動く敵の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Unit/CircleMove", fileName = "param.behavior_unit.asset", order = 40)]
public class BattleRealEnemyCircleMoveBehavior : BattleRealEnemyBehaviorUnitBase
{
    #region Field Inspector

    [Header("Circle Move Parameter")]

    [SerializeField, Tooltip("円の中心座標")]
    private Vector3 m_CenterPosition;
    protected Vector3 CenterPosition => m_CenterPosition;

    [SerializeField, Tooltip("指定した円の中心座標がワールド座標なのかローカル座標なのか")]
    private Space m_CenterPositionSpaceType;
    protected Space CenterPositionSpaceType => m_CenterPositionSpaceType;

    [SerializeField]
    private float m_Duration;
    protected float Duration => m_Duration;

    [SerializeField, Tooltip("初期角速度。ただし、反時計周りを正の速度とする。")]
    private float m_InitAngleSpeed;
    protected float InitAngleSpeed => m_InitAngleSpeed;

    [SerializeField, Tooltip("角加速度のカーブ")]
    private AnimationCurve m_AngleAccelCurve;
    protected AnimationCurve AngleAccelCurve => m_AngleAccelCurve;

    [SerializeField, Tooltip("角加速度のカーブに掛ける補正スケール")]
    private float m_AngleAccelCurveScale = 1;
    protected float AngleAccelCurveScale => m_AngleAccelCurveScale;

    [SerializeField, Tooltip("半径変更カーブを使用するかどうか")]
    private bool m_UseRadiusCurve = false;
    protected bool UseRadiusCurve => m_UseRadiusCurve;

    [SerializeField, Tooltip("半径変更カーブ。初期半径を1、円の中心を0として半径を変更できる。0-1の範囲外にも設定可能。")]
    private AnimationCurve m_RadiusCurve;
    protected AnimationCurve RadiusCurve => m_RadiusCurve;

    [Space()]

    [SerializeField, Tooltip("敵そのものの回転を適用するかどうか")]
    private bool m_UseRotation;
    protected bool UseRotation => m_UseRotation;

    [SerializeField, Tooltip("敵そのものの回転速度")]
    private float m_RotateAngleSpeed;
    protected float RotateAngleSpeed => m_RotateAngleSpeed;

    #endregion

    #region Field

    private Vector3 m_ActCenterPosition;
    private float m_Radius;
    private float m_CurrentPositionAngle;
    private float m_CurrentPositionAngleSpeed;
    private float m_CurrentEulerAngle;

    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        if (CenterPositionSpaceType == Space.World)
        {
            m_ActCenterPosition = Enemy.transform.WorldPositionToLocalPosition(CenterPosition);
        }
        else
        {
            m_ActCenterPosition = CenterPosition;
        }
        m_ActCenterPosition.y = Enemy.transform.localPosition.y;

        var t = Enemy.transform;
        var delta = t.localPosition - m_ActCenterPosition;
        m_Radius = delta.magnitude;
        m_CurrentPositionAngle = Mathf.Atan2(delta.z, delta.x).RadToDeg();
        m_CurrentPositionAngleSpeed = InitAngleSpeed;

        if (UseRotation)
        {
            m_CurrentEulerAngle = Enemy.transform.localEulerAngles.y;
        }
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Move(deltaTime);
    }

    protected override bool IsEnd()
    {
        return CurrentTime >= Duration;
    }

    #endregion

    private void Move(float deltaTime)
    {
        var phase = m_CurrentPositionAngle.DegToRad();
        var radius = (UseRadiusCurve ? RadiusCurve.Evaluate(CurrentTime) : 1) * m_Radius;
        Enemy.transform.localPosition = m_ActCenterPosition + new Vector3(Mathf.Cos(phase), 0, Mathf.Sin(phase)) * radius;

        m_CurrentPositionAngle += m_CurrentPositionAngleSpeed * deltaTime;
        var accel = AngleAccelCurveScale * AngleAccelCurve.Evaluate(CurrentTime);
        m_CurrentPositionAngleSpeed += accel * deltaTime;

        if (UseRotation)
        {
            var angle = Enemy.transform.localEulerAngles;
            angle.y = m_CurrentEulerAngle;
            Enemy.transform.localEulerAngles = angle;
            m_CurrentEulerAngle += RotateAngleSpeed * deltaTime;
        }
    }
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ある座標を中心として、その円周上を移動する。
/// Space.Worldしか許容しない。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Unit/PolarAnimation", fileName = "polar_animation.sequence_unit.asset", order = 2)]
public class SequenceUnitPolarAnimation : SequenceUnit
{
    #region Define

    [Serializable]
    private enum E_SPEED_TYPE
    {
        /// <summary>
        /// 1周する時間が半径によらず一定である速度
        /// </summary>
        ANGLE_SPEED,

        /// <summary>
        /// 円周上の移動量が半径によらず一定である速度
        /// </summary>
        CIRCUMFERENTIAL_SPEED,
    }

    #endregion

    #region Inspector Field

    [Header("Polar Animation Parameter")]

    [SerializeField, Tooltip("円運動の中心座標")]
    private Vector3 m_CenterPosition;

    [SerializeField, Tooltip("円運動の回転軸(Transform:Rotationと使い方は同じ)")]
    private Vector3 m_PolarRotation;

    [SerializeField, Tooltip("円の半径")]
    private float m_Radius;

    [SerializeField, Tooltip("最初にどの角度にいるか")]
    private float m_AngleOffset;

    [SerializeField, Tooltip("円周上の軌道を回る速度のタイプ")]
    private E_SPEED_TYPE m_SpeedType;

    [Header("Speed Parameter")]

    [SerializeField, Tooltip("回転の初速")]
    private float m_StartSpeed;

    [SerializeField, Tooltip("回転の加速度")]
    private float m_Accel;

    [SerializeField, Tooltip("速さの上限 絶対値")]
    private float m_MaxAbsSpeed;

    [Header("Look Forward Parameter")]

    [SerializeField, Tooltip("自動で進行方向を向くように計算するかどうか")]
    private bool m_UseAutoLookForward;

    [SerializeField, Tooltip("進行方向を向いた上で、さらにオフセットで回転を掛けるかどうか")]
    private bool m_UseLookOffset;

    [SerializeField, Tooltip("自動で向いた後に適用する向きのオフセット 適用順序はZXY")]
    private Vector3 m_LookOffset;

    #endregion

    #region Field

    private Matrix4x4 m_UpAxisMatrix;
    private Vector3 m_Up;
    private float m_CurrentAngle;
    private float m_CurrentSpeed;

    #endregion

    protected override void OnStart()
    {
        base.OnStart();

        m_UpAxisMatrix = Matrix4x4.Rotate(Quaternion.Euler(m_PolarRotation.x, m_PolarRotation.y, m_PolarRotation.z));
        m_Up = m_UpAxisMatrix.MultiplyVector(Vector3.up);
        m_CurrentAngle = m_AngleOffset * Mathf.Deg2Rad;
        m_CurrentSpeed = m_StartSpeed;
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        m_CurrentSpeed += m_Accel * deltaTime;
        m_CurrentSpeed = Mathf.Clamp(m_CurrentSpeed, -m_MaxAbsSpeed, m_MaxAbsSpeed);

        m_CurrentAngle += GetSpeed(m_CurrentSpeed) * deltaTime;
        m_CurrentAngle %= Mathf.PI * 2;

        Vector3 position;
        Quaternion rotation;
        Apply(m_UpAxisMatrix, m_Up, m_CurrentSpeed, m_CurrentAngle, Target.rotation, out position, out rotation);
        Target.SetPositionAndRotation(position, rotation);
    }

    private float GetSpeed(float speed)
    {
        switch(m_SpeedType)
        {
            case E_SPEED_TYPE.ANGLE_SPEED:
                return speed * Mathf.Deg2Rad;
            case E_SPEED_TYPE.CIRCUMFERENTIAL_SPEED:
                return speed / m_Radius;
        }

        return 0;
    }

    private void Apply(Matrix4x4 upAxis, Vector3 up, float currentSpeed, float currentAngle, Quaternion defaultRotation, out Vector3 position, out Quaternion rotation)
    {
        // Y軸を回転基準にしているという前提で変換する
        var x = m_Radius * Mathf.Cos(currentAngle);
        var z = m_Radius * Mathf.Sin(currentAngle);

        // 円周上の座標
        var polarPosition = upAxis.MultiplyVector(new Vector3(x, 0, z));

        // 座標を定める
        position = polarPosition + m_CenterPosition;

        if (!m_UseAutoLookForward)
        {
            rotation = defaultRotation;
            return;
        }

        var forward = Vector3.Cross(polarPosition, up) * Mathf.Sign(currentSpeed);

        if (!m_UseLookOffset)
        {
            rotation = Quaternion.LookRotation(forward, up);
            return;
        }

        var right = Vector3.Cross(up, forward);
        var zMat = Matrix4x4.Rotate(Quaternion.AngleAxis(m_LookOffset.z, forward));
        var xMat = Matrix4x4.Rotate(Quaternion.AngleAxis(m_LookOffset.x, right));
        var yMat = Matrix4x4.Rotate(Quaternion.AngleAxis(m_LookOffset.y, up));

        var f = yMat.MultiplyVector(xMat.MultiplyVector(zMat.MultiplyVector(forward)));
        var u = yMat.MultiplyVector(xMat.MultiplyVector(zMat.MultiplyVector(up)));
        rotation = Quaternion.LookRotation(f, u);
    }

    public override void GetStartTransform(Transform target, out Space spaceType, out Vector3 position, out Vector3 rotate)
    {
        var upAxisMatrix = Matrix4x4.Rotate(Quaternion.Euler(m_PolarRotation.x, m_PolarRotation.y, m_PolarRotation.z));
        var up = upAxisMatrix.MultiplyVector(Vector3.up);

        Vector3 pos;
        Quaternion rotation;
        Apply(upAxisMatrix, up, m_StartSpeed, m_AngleOffset * Mathf.Deg2Rad, target.rotation, out pos, out rotation);
        spaceType = Space.World;
        position = pos;
        rotate = rotation.eulerAngles;
    }

    private void OnValidate()
    {
        m_SpaceType = Space.World;
    }
}

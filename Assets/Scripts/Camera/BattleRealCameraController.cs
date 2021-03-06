﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リアルモードのカメラコントローラ。
/// </summary>
public class BattleRealCameraController : ControllableMonoBehavior
{
    #region Field Inspector

    [SerializeField]
    private Camera m_Camera;
    public Camera Camera => m_Camera;

    [SerializeField]
    private SequenceController m_SequenceController;

    #endregion

    #region Field

    private float m_XAmp;
    private float m_YAmp;
    private float m_Freq;
    private float m_Dec;
    private float m_Duration;

    private bool m_IsShaking;
    private float m_ShakeTimeCount;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_SequenceController.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_SequenceController.OnFinalize();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
        m_SequenceController.OnStart();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_SequenceController.OnUpdate();

        if (m_IsShaking)
        {
            if (m_ShakeTimeCount < m_Duration)
            {
                var x = m_XAmp * Mathf.Sin(m_Freq * m_ShakeTimeCount);
                var y = m_YAmp * Mathf.Sin(m_Freq * m_ShakeTimeCount);

                var pos = m_Camera.transform.localPosition;
                pos.x = x;
                pos.z = y;
                m_Camera.transform.localPosition = pos;

                m_XAmp *= (1 - m_Dec * Time.deltaTime);
                m_YAmp *= (1 - m_Dec * Time.deltaTime);
                m_ShakeTimeCount += Time.deltaTime;
            }
            else
            {
                StopShake();
            }
        }
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        m_SequenceController.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        m_SequenceController.OnFixedUpdate();
    }

    #endregion

    public void BuildSequence(SequenceGroup sequenceGroup)
    {
        m_SequenceController.BuildSequence(sequenceGroup);
    }

    public void Shake(CameraShakeParam shakeParam)
    {
        m_IsShaking = true;
        m_ShakeTimeCount = 0;
        m_XAmp = shakeParam.XAmp;
        m_YAmp = shakeParam.YAmp;
        m_Freq = shakeParam.Freq;
        m_Dec = shakeParam.Dec;
        m_Duration = shakeParam.Duration;
    }

    public void StopShake()
    {
        m_IsShaking = false;
        m_Camera.transform.localPosition = Vector3.zero;
    }
}

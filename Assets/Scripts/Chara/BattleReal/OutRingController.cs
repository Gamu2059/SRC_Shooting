using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutRingController : ControllableMonoBehavior
{
    #region Field Inspector

    [SerializeField]
    private Transform m_BaseRing;

    [SerializeField]
    private Transform m_MidRing;

    [SerializeField]
    private Transform m_OutRing;

    [SerializeField]
    private float m_BaseSpeed;

    [SerializeField]
    private float m_MidSpeed;

    [SerializeField]
    private float m_OutSpeed;

    [SerializeField]
    private AnimationCurve m_ScaleCurve;

    #endregion

    #region Field

    private bool m_IsStartAnimation;
    private float m_NowBaseAngle;
    private float m_NowMidAngle;
    private float m_NowOutAngle;
    private float m_NowScaleTimeCount;

    #endregion

    #region Game Cycle

    public override void OnStart()
    {
        base.OnStart();

        m_IsStartAnimation = false;
        m_NowBaseAngle = 0;
        m_NowMidAngle = 0;
        m_NowOutAngle = 0;
        m_NowScaleTimeCount = 0;

        SetAngle(m_BaseRing, 0);
        SetAngle(m_MidRing, 0);
        SetAngle(m_OutRing, 0);
        transform.localScale = Vector3.zero;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (!m_IsStartAnimation)
        {
            return;
        }

        var scale = m_ScaleCurve.Evaluate(m_NowScaleTimeCount);
        transform.localScale = Vector3.one * scale;

        SetAngle(m_BaseRing, m_NowBaseAngle);
        SetAngle(m_MidRing, m_NowMidAngle);
        SetAngle(m_OutRing, m_NowOutAngle);

        m_NowScaleTimeCount += Time.fixedDeltaTime;
        m_NowBaseAngle += m_BaseSpeed * Time.fixedDeltaTime;
        m_NowMidAngle += m_MidSpeed * Time.fixedDeltaTime;
        m_NowOutAngle += m_OutSpeed * Time.fixedDeltaTime;
        m_NowBaseAngle %= 360;
        m_NowMidAngle %= 360;
        m_NowOutAngle %= 360;
    }

    private void SetAngle(Transform t, float a)
    {
        var e = t.localEulerAngles;
        e.z = a;
        t.localEulerAngles = e;
    }

    #endregion

    public void StartAnimation()
    {
        if (m_IsStartAnimation)
        {
            return;
        }

        m_IsStartAnimation = true;
    }
}

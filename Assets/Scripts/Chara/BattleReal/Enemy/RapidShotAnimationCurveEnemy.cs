using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidShotAnimationCurveEnemy : AnimationCurveEnemy
{
    private bool m_IsRapidShot;
    private float m_RapidShotTimeCount;
    private float m_RapidShotInterval;

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();

        if(BehaviorParamSet is RapidShotAnimationCurveEnemyParamSet paramSet)
        {
            m_SpeedCurve = paramSet.SpeedCurve;
            m_AngleSpeedCurve = paramSet.AngleSpeedCurve;
            m_ShotParam = paramSet.ShotParam;
            m_ShotOffset = paramSet.ShotOffset;
            m_ShotStop = paramSet.ShotStop;
            m_IsRingAnimationOnStart = paramSet.IsStartAnimationOnStart;

            m_RapidShotInterval = paramSet.RapidShotInterval;
        }
    }

    public override void OnStart()
    {
        base.OnStart();
        m_IsRapidShot = false;
        m_RapidShotTimeCount = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        Move();
        Shot();

        m_MoveTimeCount += Time.fixedDeltaTime;

        if (m_IsCountOffset)
        {
            m_ShotTimeCount += Time.fixedDeltaTime;
        }
        else
        {
            if (!m_IsRapidShot)
            {
                m_RapidShotTimeCount += Time.fixedDeltaTime;
            }
            else
            {
                m_ShotTimeCount += Time.fixedDeltaTime;
                m_ShotStopTimeCount += Time.fixedDeltaTime;
            }
        }
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void Shot()
    {
        if (m_IsCountOffset)
        {
            if(m_ShotTimeCount >= m_ShotOffset)
            {
                m_ShotTimeCount = m_ShotParam.Interval;
                m_RapidShotTimeCount = m_RapidShotInterval;
                m_IsCountOffset = false;

                if (!m_IsRingAnimationOnStart)
                {
                    StartOutRingAnimation();
                }
            }
        }
        else
        {
            if(!m_IsRapidShot && m_RapidShotTimeCount >= m_RapidShotInterval)
            {
                m_RapidShotTimeCount = 0;
                m_IsRapidShot = true;
            }else if (m_ShotStopTimeCount < m_ShotStop && m_ShotTimeCount >= m_ShotParam.Interval)
            {
                m_ShotTimeCount = 0;
                OnShot(m_ShotParam);
            }else if(m_ShotStopTimeCount >= m_ShotStop)
            {
                m_ShotStopTimeCount = 0;
                m_IsRapidShot = false;
            }
        }
    }

    protected override void OnShot(EnemyShotParam param)
    {
        base.OnShot(param);
    }
}

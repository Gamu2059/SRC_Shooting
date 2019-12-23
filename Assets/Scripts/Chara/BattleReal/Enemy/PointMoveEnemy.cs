using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMoveEnemy : BattleRealEnemyController
{
    protected int m_NextDestinationIndex;
    protected Destination[] m_Destinations;
    protected Vector3 m_NextDestinaion;
    protected AnimationCurve m_MoveSpeedCurve;
    protected Destination m_Exit;

    protected EnemyShotParam m_ShotParam;
    protected float m_ShotOffset;
    protected float m_ShotStop;
    protected bool m_IsRingAnimationOnStart;

    protected Vector3 m_StartPosition;
    protected float m_StartAngle;
    protected float m_MoveTimeCount;
    protected float m_ShotTimeCount;
    protected float m_ShotStopTimeCount;
    protected bool m_IsCountOffset;

    protected float m_NowSpeed;

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();
        if(BehaviorParamSet is PointMoveEnemyParamSet paramSet)
        {
            m_Destinations = paramSet.Destinations;
            m_Exit = paramSet.Exit;
            m_ShotParam = paramSet.ShotParam;
            m_ShotOffset = paramSet.ShotOffset;
            m_ShotStop = paramSet.ShotStop;
            m_IsRingAnimationOnStart = paramSet.IsStartAnimationOnStart;
        }
    }

    public override void OnStart()
    {
        base.OnStart();

        m_NextDestinationIndex = 0;
        m_NextDestinaion = m_Destinations[m_NextDestinationIndex].m_Destination;
        m_MoveSpeedCurve = m_Destinations[m_NextDestinationIndex].m_MoveSpeedCurve;

        m_StartPosition = transform.localPosition;
        m_StartAngle = transform.localEulerAngles.y;

        m_IsCountOffset = true;
        m_ShotTimeCount = 0;
        m_ShotStopTimeCount = 0;
        m_MoveTimeCount = 0;
        m_IsLookMoveDir = false;

        if (m_IsRingAnimationOnStart)
        {
            StartOutRingAnimation();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        Move();
        Shot();

        m_ShotTimeCount += Time.fixedDeltaTime;
        m_MoveTimeCount += Time.fixedDeltaTime;

        if (m_IsCountOffset)
        {
            m_ShotStopTimeCount += Time.fixedDeltaTime;
        }
    }

    protected virtual void Move()
    {
        m_NowSpeed = m_MoveSpeedCurve.Evaluate(m_MoveTimeCount);

        transform.position = m_NextDestinaion.normalized * m_NowSpeed * Time.fixedDeltaTime + transform.position;

        if(m_MoveTimeCount >= m_MoveSpeedCurve.length)
        {
            if(m_NextDestinationIndex < m_Destinations.Length)
            {
                m_NextDestinationIndex++;
                m_NextDestinaion = m_Destinations[m_NextDestinationIndex].m_Destination;
                m_MoveSpeedCurve = m_Destinations[m_NextDestinationIndex].m_MoveSpeedCurve;
            }
            else
            {
                m_NextDestinaion = m_Exit.m_Destination;
                m_MoveSpeedCurve = m_Exit.m_MoveSpeedCurve;
            }
        }

    }

    protected virtual void Shot()
    {
        if (m_IsCountOffset)
        {
            if(m_ShotTimeCount >= m_ShotParam.Interval)
            {
                m_ShotTimeCount = m_ShotParam.Interval;
                m_IsCountOffset = false;

                if (!m_IsRingAnimationOnStart)
                {
                    StartOutRingAnimation();
                }
            }
        }
        else
        {
            if(m_ShotStopTimeCount < m_ShotStop && m_ShotTimeCount >= m_ShotParam.Interval)
            {
                m_ShotTimeCount = 0;
                OnShot(m_ShotParam);
            }
        }
    }

    protected virtual void OnShot(EnemyShotParam param)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurveEnemy : BattleRealEnemyController
{
    protected AnimationCurve m_SpeedCurve;
    protected AnimationCurve m_AngleSpeedCurve;
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
    protected float m_NowAngleSpeed;

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();

        if (BehaviorParamSet is AnimationCurveEnemyParamSet paramSet)
        {
            m_SpeedCurve = paramSet.SpeedCurve;
            m_AngleSpeedCurve = paramSet.AngleSpeedCurve;
            m_ShotParam = paramSet.ShotParam;
            m_ShotOffset = paramSet.ShotOffset;
            m_ShotStop = paramSet.ShotStop;
            m_IsRingAnimationOnStart = paramSet.IsStartAnimationOnStart;
        }
    }

    public override void OnStart()
    {
        base.OnStart();

        m_StartPosition = transform.localPosition;
        m_StartAngle = transform.localEulerAngles.y;

        m_IsCountOffset = true;
        m_ShotTimeCount = 0;
        m_ShotStopTimeCount = 0;
        m_MoveTimeCount = 0;

        m_NowSpeed = 0;
        m_NowAngleSpeed = 0;

        m_IsLookMoveDir = false;

        if(m_IsRingAnimationOnStart)
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

        if (!m_IsCountOffset)
        {
            m_ShotStopTimeCount += Time.fixedDeltaTime;
        }
    }

    protected virtual void Move()
    {
        m_NowSpeed = m_SpeedCurve.Evaluate(m_MoveTimeCount);
        m_NowAngleSpeed = m_AngleSpeedCurve.Evaluate(m_MoveTimeCount);

        transform.position = transform.forward * m_NowSpeed * Time.fixedDeltaTime + transform.position;
        var angle = transform.eulerAngles;
        angle.y += m_NowAngleSpeed * Time.fixedDeltaTime;
        transform.eulerAngles = angle;
    }

    protected virtual void Shot()
    {
        if (m_IsCountOffset)
        {
            if (m_ShotTimeCount >= m_ShotOffset)
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
            if (m_ShotStopTimeCount < m_ShotStop && m_ShotTimeCount >= m_ShotParam.Interval)
            {
                m_ShotTimeCount = 0;
                OnShot(m_ShotParam);
            }
        }
    }

    protected virtual void OnShot(EnemyShotParam param)
    {
        int num = param.Num;
        float angle = param.Angle;
        var spreadAngles = GetBulletSpreadAngles(num, angle);
        var shotParam = new BulletShotParam(this);
        shotParam.Position = transform.position;
        shotParam.Rotation = transform.eulerAngles;

        var correctAngle = 0f;
        if (param.IsPlayerLook)
        {
            var player = BattleRealPlayerManager.Instance.Player;
            var delta = player.transform.position - transform.position;
            correctAngle = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg;
        }

        for (int i = 0; i < num; i++)
        {
            var bullet = BulletController.ShotBullet(shotParam);
            bullet.SetRotation(new Vector3(0, spreadAngles[i] + correctAngle, 0));
        }

        AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot01Se);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Phase1 : BattleRealBossBehavior
{
    public enum E_PHASE
    {
        START,
        MOVE_TO_LEFT,
        MOVE_TO_RIGHT,
        WAIT_ON_LEFT,
        WAIT_ON_RIGHT,
    }

    private InfC761Phase1ParamSet m_ParamSet;
    private E_PHASE m_Phase;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;

    private float m_Duration;
    private float m_TimeCount;

    private float m_ShotTimeCount;

    public InfC761Phase1(BattleRealEnemyController enemy, BattleRealBossBehaviorParamSet paramSet) : base(enemy, paramSet)
    {
        m_ParamSet = paramSet as InfC761Phase1ParamSet;
    }

    /// <summary>
    /// この行動パターンに入った瞬間に呼び出される
    /// </summary>
    public override void OnStart()
    {
        base.OnStart();

        var initPos = Enemy.transform.position;

        m_Phase = E_PHASE.START;
        m_MoveStartPos = initPos;
        m_MoveEndPos = m_ParamSet.BasePos;
        m_TimeCount = 0;
        m_Duration = m_ParamSet.StartDuration;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        OnMove();
        OnShot();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        m_TimeCount += Time.fixedDeltaTime;
        m_ShotTimeCount += Time.fixedDeltaTime;
    }

    /// <summary>
    /// この行動パターンから他のパターンになった時に呼び出される
    /// </summary>
    public override void OnEnd()
    {
        base.OnEnd();
    }

    private Vector3 GetMovePosition()
    {
        var rate = m_ParamSet.NormalizedRate;
        var duration = rate.keys[rate.keys.Length - 1].time;
        var t = rate.Evaluate(m_TimeCount * duration / m_Duration);
        return Vector3.Lerp(m_MoveStartPos, m_MoveEndPos, t);
    }

    private void OnMove()
    {
        switch (m_Phase)
        {
            case E_PHASE.START:
                SetPosition(GetMovePosition());
                if (m_TimeCount >= m_Duration)
                {
                    m_Phase = E_PHASE.MOVE_TO_RIGHT;
                    m_MoveStartPos = m_ParamSet.BasePos;
                    m_MoveEndPos = m_ParamSet.BasePos + Vector3.right * m_ParamSet.Amplitude / 2f;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.MoveDuration;
                }
                break;

            case E_PHASE.MOVE_TO_LEFT:
                SetPosition(GetMovePosition());
                if (m_TimeCount >= m_Duration)
                {
                    m_Phase = E_PHASE.WAIT_ON_LEFT;
                    m_Duration = m_ParamSet.NextMoveWaitTime;
                    m_TimeCount = 0;
                }
                break;

            case E_PHASE.WAIT_ON_LEFT:
                if (m_TimeCount >= m_Duration)
                {
                    m_Phase = E_PHASE.MOVE_TO_RIGHT;
                    m_MoveStartPos = m_ParamSet.BasePos - Vector3.right * m_ParamSet.Amplitude / 2f;
                    m_MoveEndPos = m_ParamSet.BasePos + Vector3.right * m_ParamSet.Amplitude / 2f;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.MoveDuration;
                }
                break;

            case E_PHASE.MOVE_TO_RIGHT:
                SetPosition(GetMovePosition());
                if (m_TimeCount >= m_Duration)
                {
                    m_Phase = E_PHASE.WAIT_ON_RIGHT;
                    m_Duration = m_ParamSet.NextMoveWaitTime;
                    m_TimeCount = 0;
                }
                break;

            case E_PHASE.WAIT_ON_RIGHT:
                if (m_TimeCount >= m_Duration)
                {
                    m_Phase = E_PHASE.MOVE_TO_LEFT;
                    m_MoveStartPos = m_ParamSet.BasePos + Vector3.right * m_ParamSet.Amplitude / 2f;
                    m_MoveEndPos = m_ParamSet.BasePos - Vector3.right * m_ParamSet.Amplitude / 2f;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.MoveDuration;
                }
                break;
        }
    }
    protected virtual void OnShot(EnemyShotParam param, Vector3 shotPosition, bool isPlayerLook = false)
    {
        int num = param.Num;
        float angle = param.Angle;
        var spreadAngles = CharaController.GetBulletSpreadAngles(num, angle);
        var shotParam = new BulletShotParam();
        shotParam.Position = shotPosition + Enemy.transform.position;

        var correctAngle = 0f;
        if (isPlayerLook)
        {
            var player = BattleRealPlayerManager.Instance.Player;
            var delta = player.transform.position - Enemy.transform.position;
            correctAngle = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg + 180;
        }

        for (int i = 0; i < num; i++)
        {
            var bullet = Shot(shotParam);
            bullet.SetRotation(new Vector3(0, spreadAngles[i] + correctAngle, 0), E_RELATIVE.RELATIVE);
        }
    }

    private void OnShot()
    {
        switch (m_Phase)
        {
            case E_PHASE.START:
                break;

            case E_PHASE.MOVE_TO_LEFT:
                if (m_ShotTimeCount >= m_ParamSet.ShotParam.Interval)
                {
                    m_ShotTimeCount = 0;
                    OnShot(m_ParamSet.ShotParam, m_ParamSet.LeftShotOffset);
                    OnShot(m_ParamSet.ShotParam, m_ParamSet.RigthShotOffset);
                }
                break;

            case E_PHASE.WAIT_ON_LEFT:
                break;

            case E_PHASE.MOVE_TO_RIGHT:
                if (m_ShotTimeCount >= m_ParamSet.ShotParam.Interval)
                {
                    m_ShotTimeCount = 0;
                    OnShot(m_ParamSet.ShotParam, m_ParamSet.LeftShotOffset);
                    OnShot(m_ParamSet.ShotParam, m_ParamSet.RigthShotOffset);
                }
                break;

            case E_PHASE.WAIT_ON_RIGHT:
                break;
        }
    }
}

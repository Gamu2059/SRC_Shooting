using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Phase2 : BattleRealBossBehavior
{
    public enum E_PHASE
    {
        START,
        MOVE_TO_LEFT,
        MOVE_TO_RIGHT,
        WAIT_ON_LEFT,
        WAIT_ON_RIGHT,
    }

    public enum E_SHOT_PHASE{
        NONE,
        DIRECTION_AND_PLOOK,
    }

    private InfC761Phase2ParamSet m_ParamSet;
    private E_PHASE m_Phase;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;
    private float m_MoveStartAngle;
    private float m_MoveEndAngle;

    private float m_Duration;
    private float m_TimeCount;

    private float m_ShotTimeCount;

    private E_SHOT_PHASE m_ShotPhase;

    private float m_DirShotTimeCount;

    private float m_PLookShotTimeCount;
    public InfC761Phase2(BattleRealEnemyController enemy, BattleRealBossBehaviorParamSet paramSet) : base(enemy, paramSet)
    {
        m_ParamSet = paramSet as InfC761Phase2ParamSet;
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
        m_MoveEndPos = GetArcPosition(0);
        m_TimeCount = 0;
        m_Duration = m_ParamSet.StartDuration;
        m_ShotPhase = E_SHOT_PHASE.NONE;
        m_DirShotTimeCount = 0;
        m_PLookShotTimeCount = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        OnMove();
        OnShot();
    }

    private void CalcShotCount(){
        switch (m_ShotPhase)
        {
            case E_SHOT_PHASE.NONE:
                m_ShotTimeCount += Time.fixedDeltaTime;
                break;
            case E_SHOT_PHASE.DIRECTION_AND_PLOOK:
                m_DirShotTimeCount += Time.fixedDeltaTime;
                m_PLookShotTimeCount += Time.fixedDeltaTime;
                break;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        m_TimeCount += Time.fixedDeltaTime;
        CalcShotCount();
    }

    /// <summary>
    /// この行動パターンから他のパターンになった時に呼び出される
    /// </summary>
    public override void OnEnd()
    {
        base.OnEnd();
    }

    /// <summary>
    /// 基準座標の真下を0°として円弧上の座標を取得する。
    /// </summary>
    private Vector3 GetArcPosition(float angle)
    {
        angle -= 90;
        var rad = angle * Mathf.Deg2Rad;

        var x = Mathf.Cos(rad) * m_ParamSet.Radius + m_ParamSet.BasePos.x;
        var z = Mathf.Sin(rad) * m_ParamSet.Radius + m_ParamSet.BasePos.z;

        return new Vector3(x, 0, z);
    }
    private Vector3 GetMovePosition()
    {
        var rate = m_ParamSet.NormalizedRate;
        var duration = rate.keys[rate.keys.Length - 1].time;
        var t = rate.Evaluate(m_TimeCount * duration / m_Duration);
        return Vector3.Lerp(m_MoveStartPos, m_MoveEndPos, t);
    }

    private float GetMoveAngle()
    {
        var rate = m_ParamSet.NormalizedRate;
        var duration = rate.keys[rate.keys.Length - 1].time;
        var t = rate.Evaluate(m_TimeCount * duration / m_Duration);
        return Mathf.Lerp(m_MoveStartAngle, m_MoveEndAngle, t);
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
                    m_MoveStartAngle = 0f;
                    m_MoveEndAngle = m_ParamSet.ArcAngle / 2f;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.MoveDuration;
                }
                break;

            case E_PHASE.MOVE_TO_LEFT:
                SetPosition(GetArcPosition(GetMoveAngle()));
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
                    m_MoveStartAngle = -m_ParamSet.ArcAngle / 2f;
                    m_MoveEndAngle = m_ParamSet.ArcAngle / 2f;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.MoveDuration;
                }
                break;

            case E_PHASE.MOVE_TO_RIGHT:
                SetPosition(GetArcPosition(GetMoveAngle()));
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
                    m_MoveStartAngle = m_ParamSet.ArcAngle / 2f;
                    m_MoveEndAngle = -m_ParamSet.ArcAngle / 2f;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.MoveDuration;
                }
                break;
        }
    }
    protected virtual void OnShot(EnemyShotParam param, Vector3 shotPosition, int bulletParamIndex, bool isPlayerLook = false)
    {
        int num = param.Num;
        float angle = param.Angle;
        var spreadAngles = CharaController.GetBulletSpreadAngles(num, angle);
        var shotParam = new BulletShotParam();
        shotParam.Position = shotPosition + Enemy.transform.position;
        shotParam.BulletParamIndex = bulletParamIndex;

        var correctAngle = 0f;
        if (isPlayerLook)
        {
            var player = BattleRealPlayerManager.Instance.Player;
            var delta = player.transform.position - (Enemy.transform.position + shotPosition);
            correctAngle = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg + 180;
        }

        for (int i = 0; i < num; i++)
        {
            var bullet = Shot(shotParam);
            bullet.SetRotation(new Vector3(0, spreadAngles[i] + correctAngle, 0), E_RELATIVE.RELATIVE);
        }
    }

    private Vector3 CalcShotOffset(Vector3 offset){
        var res = offset;
        if(res.x > 0){
            res.x += 0.1f * Mathf.Sin(m_TimeCount);
        }else{
            res.x += -0.1f * Mathf.Sin(m_TimeCount);
        }
        return res;
    }

    private void DirectionAndPLookShot(){
        if (m_DirShotTimeCount >= m_ParamSet.ShotParams[0].Interval)
        {
            m_DirShotTimeCount = 0;

            OnShot(m_ParamSet.ShotParams[0], CalcShotOffset(m_ParamSet.LeftShotOffset), 0);
            OnShot(m_ParamSet.ShotParams[0], CalcShotOffset(m_ParamSet.RigthShotOffset), 0);
        }

        if(m_PLookShotTimeCount >= m_ParamSet.ShotParams[1].Interval){
            m_PLookShotTimeCount = 0;
            OnShot(m_ParamSet.ShotParams[1], m_ParamSet.CenterShotOffset, 2, true);
        }
    }

    private void DoShot(){
        switch (m_ShotPhase)
        {
            case E_SHOT_PHASE.NONE:
                if (m_ShotTimeCount >= m_ParamSet.ShotParams[0].Interval)
                {
                    m_ShotTimeCount = 0;
                    m_ShotPhase = E_SHOT_PHASE.DIRECTION_AND_PLOOK;
                }
                break;
            case E_SHOT_PHASE.DIRECTION_AND_PLOOK:
                DirectionAndPLookShot();
                break;
        }
    }

    private void OnShot()
    {
        switch (m_Phase)
        {
            case E_PHASE.START:
                break;

            case E_PHASE.MOVE_TO_LEFT:
                DoShot();
                break;

            case E_PHASE.WAIT_ON_LEFT:
                DoShot();
                break;

            case E_PHASE.MOVE_TO_RIGHT:
                DoShot();
                break;

            case E_PHASE.WAIT_ON_RIGHT:
                DoShot();
                break;
        }
    }
}

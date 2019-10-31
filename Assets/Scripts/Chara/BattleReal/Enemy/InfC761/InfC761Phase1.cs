using System.Collections;
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

    private enum E_SHOT_PHASE{
        NONE,
        N_SHOTS,
        N_WAY,
    }

    private InfC761Phase1ParamSet m_ParamSet;
    private E_PHASE m_Phase;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;

    private float m_Duration;
    private float m_TimeCount;

    private float m_ShotTimeCount;

    private E_SHOT_PHASE m_ShotPhase;

    private float m_NShotsCount;

    private float m_NShotsInterval;

    private int m_NShotsTime;

    private int m_NShotsNum;

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
        m_ShotPhase = E_SHOT_PHASE.NONE;
        m_NShotsNum = m_ParamSet.NShotsNum;
        m_NShotsCount = 0.0f;
        m_NShotsInterval = m_ParamSet.NShotsInterval;
        m_NShotsTime = 0;
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

        switch(m_ShotPhase){
            case E_SHOT_PHASE.NONE:
                m_ShotTimeCount += Time.fixedDeltaTime;
                break;
            case E_SHOT_PHASE.N_SHOTS:
                m_NShotsCount += Time.fixedDeltaTime;
                break;
            case E_SHOT_PHASE.N_WAY:
                m_ShotTimeCount += Time.fixedDeltaTime;
                break;
        }

        this.PlayerLookNShots();
        this.NWayShot();
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


    private void PlayerLookNShots(){
        if(m_ShotPhase == E_SHOT_PHASE.N_SHOTS){
            if(m_NShotsTime >= m_NShotsNum){
                m_NShotsCount = 0.0f;
                m_NShotsTime = 0;
                m_ShotTimeCount = 0;
                SetShotPhase(E_SHOT_PHASE.N_WAY);
            }else if(m_NShotsCount >= m_NShotsInterval){
                m_NShotsCount = 0.0f;
                m_NShotsTime++;
                OnShot(m_ParamSet.ShotParams[0], m_ParamSet.LeftShotOffset, 0, true);
                OnShot(m_ParamSet.ShotParams[0], m_ParamSet.RigthShotOffset, 0, true);
            }
        }
    }

    private void NWayShot(){
        if(m_ShotPhase == E_SHOT_PHASE.N_WAY){
            if(m_ShotTimeCount >= m_ParamSet.ShotParams[1].Interval){
                m_ShotTimeCount = 0;
                OnShot(m_ParamSet.ShotParams[1], m_ParamSet.CenterShotOffset, 1);
                SetShotPhase(E_SHOT_PHASE.NONE);
            }
            
        }
    }


    private void SetShotPhase(E_SHOT_PHASE shotPhase){
        m_ShotPhase = shotPhase;
    }

    private void OnShot()
    {
        switch (m_Phase)
        {
            case E_PHASE.START:
                
                break;

            case E_PHASE.MOVE_TO_LEFT:
                if (m_ShotTimeCount >= m_ParamSet.ShotParams[1].Interval && m_ShotPhase == E_SHOT_PHASE.NONE)
                {
                    m_ShotTimeCount = 0;
                    SetShotPhase(E_SHOT_PHASE.N_WAY);
                }
                break;

            case E_PHASE.WAIT_ON_LEFT:
                if (m_ShotTimeCount >= m_ParamSet.ShotParams[0].Interval && m_ShotPhase == E_SHOT_PHASE.NONE)
                {
                    m_ShotTimeCount = 0;
                    SetShotPhase(E_SHOT_PHASE.N_SHOTS);
                }
                break;

            case E_PHASE.MOVE_TO_RIGHT:
                if (m_ShotTimeCount >= m_ParamSet.ShotParams[1].Interval && m_ShotPhase == E_SHOT_PHASE.NONE)
                {
                    m_ShotTimeCount = 0;
                    SetShotPhase(E_SHOT_PHASE.N_WAY);
                }
                break;

            case E_PHASE.WAIT_ON_RIGHT:
                if (m_ShotTimeCount >= m_ParamSet.ShotParams[0].Interval && m_ShotPhase == E_SHOT_PHASE.NONE)
                {
                    m_ShotTimeCount = 0;
                    SetShotPhase(E_SHOT_PHASE.N_SHOTS);
                }
                break;
        }
    }
}

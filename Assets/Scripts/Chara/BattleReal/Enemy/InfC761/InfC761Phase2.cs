using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Phase2 : BattleRealBossBehavior
{
    public enum E_PHASE{
        START,
        MOVE_TO_EDGE0,
        WAIT_ON_EDGE0,
        MOVE_TO_EDGE1,
        WAIT_ON_EDGE1,
        MOVE_TO_EDGE2,
        WAIT_ON_EDGE2,
        MOVE_TO_EDGE3,
        WAIT_ON_EDGE3,
        MOVE_TO_EDGE4,
        WAIT_ON_EDGE4,
        APPROACH_TO_PLAYER,
        WAIT_ON_NEAR_PLAYER,
        MOVE_TO_ABOVE,
        WAIT_ON_ABOVE,
    }

    private InfC761Phase2ParamSet m_ParamSet;
    private E_PHASE m_Phase;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;

    private float m_Duration;
    private float m_TimeCount;

    public InfC761Phase2(BattleRealEnemyController enemy, BattleRealBossBehaviorParamSet paramSet) : base(enemy, paramSet){
        m_ParamSet = paramSet as InfC761Phase2ParamSet;
    }

    public override void OnStart(){
        base.OnStart();

        var initPos = Enemy.transform.position;

        m_Phase = E_PHASE.START;
        m_MoveStartPos = initPos;
        m_MoveEndPos = m_ParamSet.BasePos;
        
        m_TimeCount = 0;
        m_Duration = m_ParamSet.StartDuration;
    }

    public override void OnUpdate(){
        base.OnUpdate();
        OnMove();
        OnShot();
    }

    public override void OnFixedUpdate(){
        base.OnFixedUpdate();

        m_TimeCount += Time.fixedDeltaTime;
    }

    public override void OnEnd(){
        base.OnEnd();
    }

    private Vector3 GetMovePosition(int normarizedRateIndex)
    {
        var rate = m_ParamSet.NormalizedRates[normarizedRateIndex];
        var duration = rate.keys[rate.keys.Length - 1].time;
        var t = rate.Evaluate(m_TimeCount * duration / m_Duration);
        return Vector3.Lerp(m_MoveStartPos, m_MoveEndPos, t);
    }

    private void SetMove(E_PHASE nextPhase, float nextDuration, Vector3 start, Vector3 end, BossMoveParam.E_COORD coord){
        m_Phase = nextPhase;
        m_TimeCount = 0;
        m_Duration = nextDuration;
        m_MoveStartPos = start;
        if(coord == BossMoveParam.E_COORD.ABSOLUTE){
            m_MoveEndPos = end;
        }else{
            m_MoveEndPos = start + end;
        }
    }

    private void SetWait(E_PHASE nextPhase, float nextDuration){
        m_Phase = nextPhase;
        m_TimeCount = 0;
        m_Duration = nextDuration;
    }

    private void OnMove(){
        switch (m_Phase)
        {
            case E_PHASE.START:
                SetPosition(GetMovePosition(0));
                if(m_TimeCount >= m_Duration){
                    SetMove(E_PHASE.MOVE_TO_EDGE0, m_ParamSet.MoveDurations[0], Enemy.transform.position, m_ParamSet.MoveParams[0].Destination, m_ParamSet.MoveParams[0].Coordinate);
                }
                break;
            case E_PHASE.MOVE_TO_EDGE0:
                SetPosition(GetMovePosition(2));
                if(m_TimeCount >= m_Duration){
                    SetWait(E_PHASE.WAIT_ON_EDGE0, m_ParamSet.NextMoveWaitTimes[0]);
                }
                break;
            case E_PHASE.WAIT_ON_EDGE0:
                if(m_TimeCount >= m_Duration){
                    SetMove(E_PHASE.MOVE_TO_EDGE1, m_ParamSet.MoveDurations[1], Enemy.transform.position, m_ParamSet.MoveParams[1].Destination, m_ParamSet.MoveParams[1].Coordinate);
                }
                break;
            case E_PHASE.MOVE_TO_EDGE1:
                SetPosition(GetMovePosition(2));
                if(m_TimeCount >= m_Duration){
                    SetWait(E_PHASE.WAIT_ON_EDGE1, m_ParamSet.NextMoveWaitTimes[1]);
                }
                break;
            case E_PHASE.WAIT_ON_EDGE1:
                if(m_TimeCount >= m_Duration){
                    SetMove(E_PHASE.MOVE_TO_EDGE2, m_ParamSet.MoveDurations[2], Enemy.transform.position, m_ParamSet.MoveParams[2].Destination, m_ParamSet.MoveParams[2].Coordinate);
                }
                break;
            case E_PHASE.MOVE_TO_EDGE2:
                SetPosition(GetMovePosition(2));
                if(m_TimeCount >= m_Duration){
                    SetWait(E_PHASE.WAIT_ON_EDGE2, m_ParamSet.NextMoveWaitTimes[2]);
                }
                break;
            case E_PHASE.WAIT_ON_EDGE2:
                if(m_TimeCount >= m_Duration){
                    SetMove(E_PHASE.MOVE_TO_EDGE3, m_ParamSet.MoveDurations[3], Enemy.transform.position, m_ParamSet.MoveParams[3].Destination, m_ParamSet.MoveParams[3].Coordinate);
                }
                break;
            case E_PHASE.MOVE_TO_EDGE3:
                SetPosition(GetMovePosition(2));
                if(m_TimeCount >= m_Duration){
                    SetWait(E_PHASE.WAIT_ON_EDGE3, m_ParamSet.NextMoveWaitTimes[3]);
                }
                break;
            case E_PHASE.WAIT_ON_EDGE3:
                if(m_TimeCount >= m_Duration){
                    SetMove(E_PHASE.MOVE_TO_EDGE4, m_ParamSet.MoveDurations[4], Enemy.transform.position, m_ParamSet.MoveParams[4].Destination, m_ParamSet.MoveParams[4].Coordinate);
                }
                break;
            case E_PHASE.MOVE_TO_EDGE4:
                SetPosition(GetMovePosition(2));
                if(m_TimeCount >= m_Duration){
                    SetWait(E_PHASE.WAIT_ON_EDGE4, m_ParamSet.NextMoveWaitTimes[4]);
                }
                break;
            case E_PHASE.WAIT_ON_EDGE4:
                if(m_TimeCount >= m_Duration){
                    float x = BattleRealPlayerManager.Instance.Player.transform.position.x;
                    float z = m_ParamSet.MoveParams[5].Destination.z;
                    Vector3 dest = new Vector3(x,0f,z);
                    SetMove(E_PHASE.APPROACH_TO_PLAYER, m_ParamSet.MoveDurations[5], Enemy.transform.position, dest, BossMoveParam.E_COORD.ABSOLUTE);
                }
                break;
            case E_PHASE.APPROACH_TO_PLAYER:
                SetPosition(GetMovePosition(1));
                if(m_TimeCount >= m_Duration){
                    SetWait(E_PHASE.WAIT_ON_NEAR_PLAYER, m_ParamSet.NextMoveWaitTimes[5]);
                }
                break;
            case E_PHASE.WAIT_ON_NEAR_PLAYER:
                if(m_TimeCount >= m_Duration){
                    SetMove(E_PHASE.MOVE_TO_ABOVE, m_ParamSet.MoveDurations[6], Enemy.transform.position, m_ParamSet.MoveParams[6].Destination, m_ParamSet.MoveParams[6].Coordinate);
                }
                break;
            case E_PHASE.MOVE_TO_ABOVE:
                SetPosition(GetMovePosition(2));
                if(m_TimeCount >= m_Duration){
                    SetWait(E_PHASE.WAIT_ON_ABOVE, m_ParamSet.NextMoveWaitTimes[6]);
                }
                break;
            case E_PHASE.WAIT_ON_ABOVE:
                if(m_TimeCount >= m_Duration){
                    SetMove(E_PHASE.MOVE_TO_EDGE0, m_ParamSet.MoveDurations[0], Enemy.transform.position, m_ParamSet.MoveParams[0].Destination, m_ParamSet.MoveParams[0].Coordinate);
                }
                break;
        }
    }

    protected virtual void OnShot(EnemyShotParam param, Vector3 shotPosition, int bulletIndex, int bulletParamIndex)
    {
        int num = param.Num;
        float angle = param.Angle;
        var spreadAngles = CharaController.GetBulletSpreadAngles(num, angle);
        var shotParam = new BulletShotParam();
        shotParam.Position = shotPosition + Enemy.transform.position;
        shotParam.BulletParamIndex = bulletParamIndex;
        shotParam.BulletIndex = bulletIndex;

        var correctAngle = 0f;
        if (param.IsPlayerLook)
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

    private void OnShot(){
        switch (m_Phase)
        {
            case E_PHASE.START:
                break;
            case E_PHASE.MOVE_TO_EDGE0:
                break;
            case E_PHASE.WAIT_ON_EDGE0:
                break;
            case E_PHASE.MOVE_TO_EDGE1:
                break;
            case E_PHASE.WAIT_ON_EDGE1:
                break;
            case E_PHASE.MOVE_TO_EDGE2:
                break;
            case E_PHASE.WAIT_ON_EDGE2:
                break;
            case E_PHASE.MOVE_TO_EDGE3:
                break;
            case E_PHASE.WAIT_ON_EDGE3:
                break;
            case E_PHASE.MOVE_TO_EDGE4:
                break;
            case E_PHASE.WAIT_ON_EDGE4:
                break;
            case E_PHASE.APPROACH_TO_PLAYER:
                break;
            case E_PHASE.WAIT_ON_NEAR_PLAYER:
                break;
            case E_PHASE.MOVE_TO_ABOVE:
                break;
            case E_PHASE.WAIT_ON_ABOVE:
                break;
        }
    }
}

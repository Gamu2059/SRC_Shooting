using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
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

    public enum E_SHOT_EDGE{
        RIGHT,
        LEFT,
    }

    private InfC761Phase2ParamSet m_ParamSet;
    private E_PHASE m_Phase;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;

    private float m_Duration;
    private float m_TimeCount;

    private bool m_IsShotLargeBulletToUnder;
    private float m_ShotLargeBulletToUnderTimeCount;

    private bool m_IsShotSmallPLookBullet;
    private float m_ShotSmallPLookBulletTimeCount;
    private float m_StopShotSmallPLookBulletTimeCount;

    private float m_Shot2WayLargeBulletTimeCount;

    private E_SHOT_EDGE m_ShotEdge;
    private bool m_IsShotPLookSmallRectBullet;
    private float m_ShotPLookSmallRectBulletTimeCount;
    private float m_ShotRapidPLookSmallRectBulletTimeCount;
    private float m_NumberOfShotRapidPLookSmallRectBullet;

    private bool m_IsShotLargeRectBulletToTotalDirection;
    private float m_ShotLargeRectBulletToTotalDirectionTimeCount;

    private bool m_IsShotBasicRectBulletToTotalDirection;
    private float m_ShotBasicRectBulletToTotalDirectionTimeCount;

    private float m_ShotBasicCirBulletToTotalDirectionTimeCount;

    public InfC761Phase2(BattleRealEnemyBase enemy, BattleRealBossBehaviorUnitParamSet paramSet) : base(enemy, paramSet){
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

        m_IsShotLargeBulletToUnder = false;
        m_ShotLargeBulletToUnderTimeCount = 0;

        m_IsShotSmallPLookBullet = false;
        m_ShotSmallPLookBulletTimeCount = 0;
        m_StopShotSmallPLookBulletTimeCount = 0;

        m_Shot2WayLargeBulletTimeCount = 0;

        m_ShotEdge = E_SHOT_EDGE.LEFT;
        m_IsShotPLookSmallRectBullet = false;
        m_ShotPLookSmallRectBulletTimeCount = 0;
        m_ShotRapidPLookSmallRectBulletTimeCount = 0;
        m_NumberOfShotRapidPLookSmallRectBullet = 0;

        m_IsShotLargeRectBulletToTotalDirection = false;
        m_ShotLargeRectBulletToTotalDirectionTimeCount = 0;

        m_IsShotBasicRectBulletToTotalDirection = false;
        m_ShotBasicRectBulletToTotalDirectionTimeCount = 0;

        m_ShotBasicCirBulletToTotalDirectionTimeCount = 0;
    }

    public override void OnUpdate(){
        base.OnUpdate();
        OnMove();
        OnShot();
    }

    public override void OnFixedUpdate(){
        base.OnFixedUpdate();

        m_TimeCount += Time.fixedDeltaTime;

        if(m_IsShotLargeBulletToUnder){
            m_ShotLargeBulletToUnderTimeCount += Time.fixedDeltaTime;
        }

        if(m_IsShotSmallPLookBullet){
            m_StopShotSmallPLookBulletTimeCount += Time.fixedDeltaTime;
            m_ShotSmallPLookBulletTimeCount += Time.fixedDeltaTime;
        }

        if(m_Phase == E_PHASE.MOVE_TO_ABOVE){
            m_Shot2WayLargeBulletTimeCount += Time.fixedDeltaTime;
        }

        if(m_Phase == E_PHASE.MOVE_TO_ABOVE){
            if(m_IsShotPLookSmallRectBullet){
                m_ShotRapidPLookSmallRectBulletTimeCount += Time.fixedDeltaTime;
            }else{
                m_ShotPLookSmallRectBulletTimeCount += Time.fixedDeltaTime;
            }
        }

        if(m_IsShotLargeRectBulletToTotalDirection){
            m_ShotLargeRectBulletToTotalDirectionTimeCount += Time.fixedDeltaTime;
        }

        if(m_IsShotBasicRectBulletToTotalDirection){
            m_ShotBasicRectBulletToTotalDirectionTimeCount += Time.fixedDeltaTime;
        }

        if(m_Phase == E_PHASE.APPROACH_TO_PLAYER){
            m_ShotBasicCirBulletToTotalDirectionTimeCount += Time.fixedDeltaTime;
        }
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
                    if(!m_IsShotSmallPLookBullet){
                        m_IsShotSmallPLookBullet = true;
                    }
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
                    //SetMove(E_PHASE.MOVE_TO_EDGE0, m_ParamSet.MoveDurations[0], Enemy.transform.position, m_ParamSet.MoveParams[0].Destination, m_ParamSet.MoveParams[0].Coordinate);
                    float x = BattleRealPlayerManager.Instance.Player.transform.position.x;
                    float z = m_ParamSet.MoveParams[5].Destination.z;
                    Vector3 dest = new Vector3(x,0f,z);
                    SetMove(E_PHASE.APPROACH_TO_PLAYER, m_ParamSet.MoveDurations[5], Enemy.transform.position, dest, BossMoveParam.E_COORD.ABSOLUTE);
                    if(!m_IsShotSmallPLookBullet){
                        m_IsShotSmallPLookBullet = true;
                    }
                    m_ShotBasicCirBulletToTotalDirectionTimeCount = 0;
                }
                break;
        }
    }

    protected virtual void OnShot(EnemyShotParam param, Vector3 shotPosition, int bulletIndex, int bulletParamIndex)
    {
        int num = param.Num;
        float angle = param.Angle;
        var spreadAngles = BattleRealCharaController.GetBulletSpreadAngles(num, angle);
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

    private void ShotBulletToTotalDirection(){
        if(!m_IsShotLargeRectBulletToTotalDirection){
            m_IsShotLargeRectBulletToTotalDirection = true;
        }else if(m_ShotLargeRectBulletToTotalDirectionTimeCount >= m_ParamSet.ShotParams[4].Interval){
            m_ShotLargeRectBulletToTotalDirectionTimeCount = 0;
            OnShot(m_ParamSet.ShotParams[4],m_ParamSet.ShotOffSets[6], 38, 7);
            AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot02Se);
        }
        else if(m_TimeCount >= m_Duration){
            m_IsShotLargeRectBulletToTotalDirection = false;
        }

        if(!m_IsShotBasicRectBulletToTotalDirection){
            m_IsShotBasicRectBulletToTotalDirection = true;
        }else if(m_ShotBasicRectBulletToTotalDirectionTimeCount >= m_ParamSet.ShotParams[5].Interval){
            m_ShotBasicRectBulletToTotalDirectionTimeCount = 0;
            OnShot(m_ParamSet.ShotParams[5], m_ParamSet.ShotOffSets[6], 41, 7);
            AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot02Se);
        }
        else if(m_TimeCount >= m_Duration){
            m_IsShotBasicRectBulletToTotalDirection = false;
        }
    }

    private void ShotLargeBulletToUnder(){
        if(m_IsShotLargeBulletToUnder && m_ShotLargeBulletToUnderTimeCount >= m_ParamSet.ShotParams[0].Interval){
            m_ShotLargeBulletToUnderTimeCount = 0;
            OnShot(m_ParamSet.ShotParams[0],m_ParamSet.ShotOffSets[0], 33, 2);
            AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot02Se);
            m_IsShotLargeBulletToUnder = false;
        }
    }

    private void OnShot(){
        switch (m_Phase)
        {
            case E_PHASE.START:
                break;
            case E_PHASE.MOVE_TO_EDGE0:
                if(!m_IsShotLargeBulletToUnder){
                    m_IsShotLargeBulletToUnder = true;
                }
                ShotBulletToTotalDirection();
                break;
            case E_PHASE.WAIT_ON_EDGE0:
                ShotLargeBulletToUnder();
                break;
            case E_PHASE.MOVE_TO_EDGE1:
                if(!m_IsShotLargeBulletToUnder){
                    m_IsShotLargeBulletToUnder = true;
                }
                ShotBulletToTotalDirection();
                break;
            case E_PHASE.WAIT_ON_EDGE1:
                ShotLargeBulletToUnder();
                break;
            case E_PHASE.MOVE_TO_EDGE2:
                if(!m_IsShotLargeBulletToUnder){
                    m_IsShotLargeBulletToUnder = true;
                }
                ShotBulletToTotalDirection();
                break;
            case E_PHASE.WAIT_ON_EDGE2:
                ShotLargeBulletToUnder();
                break;
            case E_PHASE.MOVE_TO_EDGE3:                
                if(!m_IsShotLargeBulletToUnder){
                    m_IsShotLargeBulletToUnder = true;
                }
                ShotBulletToTotalDirection();
                break;
            case E_PHASE.WAIT_ON_EDGE3:
                ShotLargeBulletToUnder();
                break;
            case E_PHASE.MOVE_TO_EDGE4:                
                if(!m_IsShotLargeBulletToUnder){
                    m_IsShotLargeBulletToUnder = true;
                }
                ShotBulletToTotalDirection();
                break;
            case E_PHASE.WAIT_ON_EDGE4:
                ShotLargeBulletToUnder();
                break;
            case E_PHASE.APPROACH_TO_PLAYER:

                if(m_IsShotSmallPLookBullet && m_ShotSmallPLookBulletTimeCount >= m_ParamSet.ShotParams[1].Interval){
                    m_ShotSmallPLookBulletTimeCount = 0;
                    OnShot(m_ParamSet.ShotParams[1], m_ParamSet.ShotOffSets[1], 21, 0);
                    OnShot(m_ParamSet.ShotParams[1], m_ParamSet.ShotOffSets[2], 21, 0);
                    AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot02Se);
                }
                if(m_StopShotSmallPLookBulletTimeCount >= m_ParamSet.ShotCoolTimes[0]){
                    m_StopShotSmallPLookBulletTimeCount = 0;
                    m_IsShotSmallPLookBullet = false;
                }

                if(m_ShotBasicCirBulletToTotalDirectionTimeCount >= m_ParamSet.ShotParams[6].Interval){
                    m_ShotBasicCirBulletToTotalDirectionTimeCount = 0;
                    OnShot(m_ParamSet.ShotParams[6], m_ParamSet.ShotOffSets[6], 37, 7);
                    AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot02Se);
                }

                break;
            case E_PHASE.WAIT_ON_NEAR_PLAYER:
                break;
            case E_PHASE.MOVE_TO_ABOVE:
                
                if(m_Shot2WayLargeBulletTimeCount >= m_ParamSet.ShotParams[2].Interval){
                    m_Shot2WayLargeBulletTimeCount = 0;
                    OnShot(m_ParamSet.ShotParams[2], m_ParamSet.ShotOffSets[3], 33, 3);
                    AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot02Se);
                }

                if(!m_IsShotPLookSmallRectBullet && m_ShotPLookSmallRectBulletTimeCount >= m_ParamSet.ShotCoolTimes[1]){
                    m_ShotPLookSmallRectBulletTimeCount = 0;
                    m_IsShotPLookSmallRectBullet = true;
                }else if(m_ShotRapidPLookSmallRectBulletTimeCount >= m_ParamSet.ShotParams[3].Interval && m_NumberOfShotRapidPLookSmallRectBullet < m_ParamSet.NumberOfShotRapidPLookSmallRectBullet){
                    m_ShotRapidPLookSmallRectBulletTimeCount = 0;
                    m_NumberOfShotRapidPLookSmallRectBullet++;
                    if(m_ShotEdge == E_SHOT_EDGE.LEFT){
                        OnShot(m_ParamSet.ShotParams[3], m_ParamSet.ShotOffSets[4], 23, 0);
                    }else{
                        OnShot(m_ParamSet.ShotParams[3], m_ParamSet.ShotOffSets[5], 23, 0);
                    }
                    AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot02Se);
                }
                else if(m_NumberOfShotRapidPLookSmallRectBullet >= m_ParamSet.NumberOfShotRapidPLookSmallRectBullet){
                    if(m_ShotEdge == E_SHOT_EDGE.LEFT){
                        m_ShotEdge = E_SHOT_EDGE.RIGHT;
                    }else{
                        m_ShotEdge = E_SHOT_EDGE.LEFT;
                    }
                    m_NumberOfShotRapidPLookSmallRectBullet = 0;
                    m_ShotRapidPLookSmallRectBulletTimeCount = 0;
                    m_IsShotPLookSmallRectBullet = false;
                }

                break;
            case E_PHASE.WAIT_ON_ABOVE:
                break;
        }
    }
}

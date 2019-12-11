using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Phase3 : BattleRealBossBehavior
{
    public enum E_PHASE{
        START,
        WAIT,
        MOVE_TO_LEFT_RIGHT,
    }

    public enum E_SHOT_EDGE{
        RIGHT,
        LEFT,
    }

    private InfC761Phase3ParamSet m_ParamSet;
    private E_PHASE m_Phase;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;

    private float m_Duration;
    private float m_TimeCount;

    private float m_ShotBulletsToTotalDirectionTimeCount;
    
    private bool m_IsShotLargeBulletToUnder;
    private float m_ShotLargeBulletToUnderTimeCount;
    private float m_ShotRapidLargeBulletToUnderTimeCount;
    private int m_NumberOfShotRapidLargeBulletToUnder;
    private int m_NumberOfMove;

    public InfC761Phase3(BattleRealEnemyController enemy, BattleRealBossBehaviorParamSet paramSet) : base(enemy, paramSet){
        m_ParamSet = paramSet as InfC761Phase3ParamSet;
    }

    public override void OnStart(){
        base.OnStart();

        var initPos = Enemy.transform.position;

        m_Phase = E_PHASE.START;
        m_MoveStartPos = initPos;
        m_MoveEndPos = m_ParamSet.BasePos;
        
        m_TimeCount = 0;
        m_Duration = m_ParamSet.StartDuration;

        m_ShotBulletsToTotalDirectionTimeCount = 0;

        m_IsShotLargeBulletToUnder = false;
        m_ShotLargeBulletToUnderTimeCount = 0;
        m_ShotRapidLargeBulletToUnderTimeCount = 0;
        m_NumberOfShotRapidLargeBulletToUnder = 0;
        m_NumberOfMove = 0;
    }

    public override void OnUpdate(){
        base.OnUpdate();
        OnMove();
        OnShot();
    }

    public override void OnFixedUpdate(){
        base.OnFixedUpdate();

        m_TimeCount += Time.fixedDeltaTime;

        if(m_Phase == E_PHASE.WAIT || m_Phase == E_PHASE.MOVE_TO_LEFT_RIGHT){
            m_ShotBulletsToTotalDirectionTimeCount += Time.deltaTime;
        }

        if(m_Phase == E_PHASE.WAIT){
            if(m_IsShotLargeBulletToUnder){
                m_ShotRapidLargeBulletToUnderTimeCount += Time.fixedDeltaTime;
            }else{
                m_ShotLargeBulletToUnderTimeCount += Time.fixedDeltaTime;
            }
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

    private void OnMove(){
        switch (m_Phase)
        {
            case E_PHASE.START:
                SetPosition(GetMovePosition(0));
                if(m_TimeCount >= m_Duration){
                    m_Phase = E_PHASE.WAIT;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.NextMoveWaitTimes[0];
                }
                break;
            case E_PHASE.WAIT:
                if(m_TimeCount >= m_Duration){
                    m_Phase = E_PHASE.MOVE_TO_LEFT_RIGHT;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.MoveDurations[0];
                    m_MoveStartPos = Enemy.transform.position;
                    var player = BattleRealPlayerManager.Instance.Player.transform.position;
                    if(player.x < m_MoveStartPos.x && m_MoveStartPos.x - m_ParamSet.Amplitudes[0] >= -0.8f){
                        m_MoveEndPos = m_MoveStartPos + m_ParamSet.Amplitudes[0] * Vector3.left;
                        m_MoveEndPos.z = m_ParamSet.BasePos.z + Random.Range(-1.0f * m_ParamSet.Amplitudes[1], m_ParamSet.Amplitudes[1]);
                    }else if(m_MoveStartPos.x + m_ParamSet.Amplitudes[0] <= 0.8f){
                        m_MoveEndPos = m_MoveStartPos + m_ParamSet.Amplitudes[0] * Vector3.right;
                        m_MoveEndPos.z = m_ParamSet.BasePos.z + Random.Range(-1.0f * m_ParamSet.Amplitudes[1], m_ParamSet.Amplitudes[1]);
                    }
                }
                break;
            case E_PHASE.MOVE_TO_LEFT_RIGHT:
                SetPosition(GetMovePosition(0));
                if(m_TimeCount >= m_Duration){
                    m_Phase = E_PHASE.WAIT;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.NextMoveWaitTimes[0];
                    m_NumberOfMove++;
                    m_IsShotLargeBulletToUnder = true;
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
            case E_PHASE.WAIT:
                if(m_ShotBulletsToTotalDirectionTimeCount >= m_ParamSet.ShotCoolTimes[0]){
                    m_ShotBulletsToTotalDirectionTimeCount = 0;
                    OnShot(m_ParamSet.ShotParams[0], m_ParamSet.ShotOffSets[0], 41, 2);
                    OnShot(m_ParamSet.ShotParams[1], m_ParamSet.ShotOffSets[0], 20, 2);
                    //AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Shot02");
                }
                if(m_IsShotLargeBulletToUnder && m_NumberOfMove % 2 == 1){
                    if(m_ShotRapidLargeBulletToUnderTimeCount >= m_ParamSet.ShotParams[2].Interval && m_NumberOfShotRapidLargeBulletToUnder < m_ParamSet.NumbersOfRapidShot[0]){
                        m_ShotRapidLargeBulletToUnderTimeCount = 0;
                        m_NumberOfShotRapidLargeBulletToUnder++;
                        OnShot(m_ParamSet.ShotParams[2], m_ParamSet.ShotOffSets[1], 33, 4);
                        //AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Shot02");
                    }else if(m_NumberOfShotRapidLargeBulletToUnder >= m_ParamSet.NumbersOfRapidShot[0]){
                        m_ShotRapidLargeBulletToUnderTimeCount = 0;
                        m_NumberOfShotRapidLargeBulletToUnder = 0;
                        m_IsShotLargeBulletToUnder = false;
                    }
                }
                break;
            case E_PHASE.MOVE_TO_LEFT_RIGHT:
                if(m_ShotBulletsToTotalDirectionTimeCount >= m_ParamSet.ShotCoolTimes[0]){
                    m_ShotBulletsToTotalDirectionTimeCount = 0;
                    OnShot(m_ParamSet.ShotParams[0], m_ParamSet.ShotOffSets[0], 41, 2);
                    OnShot(m_ParamSet.ShotParams[1], m_ParamSet.ShotOffSets[0], 20, 2);
                    //AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Shot02");
                }
                break;
        }
    }
}

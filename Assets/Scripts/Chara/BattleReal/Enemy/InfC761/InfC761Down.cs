using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Down : BattleRealBossBehavior
{
    public enum E_PHASE
    {
        WAIT,
        MOVE_TO_RIGHT,
        MOVE_TO_LEFT,
    }

    private InfC761DownParamSet m_ParamSet;
    private E_PHASE m_Phase;
    private E_PHASE m_CurrentPhase;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;

    private float m_Duration;
    private float m_TimeCount;

    private float m_ShotTimeCount;

    public InfC761Down(BattleRealEnemyBase enemy, BattleRealBossBehaviorUnitParamSet paramSet) : base(enemy, paramSet)
    {
        m_ParamSet = paramSet as InfC761DownParamSet;
    }

    public override void OnStart(){
        base.OnStart();
        m_TimeCount = 0;
        m_Phase = E_PHASE.WAIT;
        m_CurrentPhase = E_PHASE.MOVE_TO_LEFT;
        m_Duration = m_ParamSet.NextMoveWaitTime;
        m_MoveStartPos = Enemy.transform.position;
        m_MoveEndPos = m_MoveStartPos;
    }

    public override void OnUpdate(){
        base.OnUpdate();

        OnMove();
    }

    public override void OnFixedUpdate(){
        base.OnFixedUpdate();
        m_TimeCount += Time.fixedDeltaTime;
    }

    private Vector3 GetMovePosition()
    {
        var rate = m_ParamSet.NormalizedRate;
        var duration = rate.keys[rate.keys.Length - 1].time;
        var t = rate.Evaluate(m_TimeCount * duration / m_Duration);
        return Vector3.Lerp(m_MoveStartPos, m_MoveEndPos, t);
    }

    private void OnMove(){
        switch (m_Phase)
        {
            case E_PHASE.WAIT:
                if(m_TimeCount >= m_Duration){
                    if(m_CurrentPhase == E_PHASE.MOVE_TO_RIGHT){
                        m_Phase = E_PHASE.MOVE_TO_LEFT;
                        m_CurrentPhase = E_PHASE.MOVE_TO_LEFT;
                        m_TimeCount = 0;
                        m_Duration = m_ParamSet.MoveDuration;
                    }else{
                        m_Phase = E_PHASE.MOVE_TO_RIGHT;
                        m_CurrentPhase = E_PHASE.MOVE_TO_RIGHT;
                        m_TimeCount = 0;
                        m_Duration = m_ParamSet.MoveDuration;
                    }
                }
                break;
            case E_PHASE.MOVE_TO_LEFT:
                SetPosition(GetMovePosition());
                if(m_TimeCount >= m_Duration){
                    m_Phase = E_PHASE.WAIT;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.NextMoveWaitTime;
                    m_MoveStartPos = Enemy.transform.position;
                    m_MoveEndPos = m_MoveStartPos + Vector3.left * m_ParamSet.Amplitude;
                    m_MoveEndPos.z += Random.Range(-m_ParamSet.Amplitude/2, m_ParamSet.Amplitude/2);
                }
                break;
            case E_PHASE.MOVE_TO_RIGHT:
                SetPosition(GetMovePosition());
                if(m_TimeCount >= m_Duration){
                    m_Phase = E_PHASE.WAIT;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.NextMoveWaitTime;
                    m_MoveStartPos = Enemy.transform.position;
                    m_MoveEndPos = m_MoveStartPos + Vector3.right * m_ParamSet.Amplitude;
                    m_MoveEndPos.z += Random.Range(-m_ParamSet.Amplitude/2, m_ParamSet.Amplitude/2);
                }
                break;
        }
    }
}

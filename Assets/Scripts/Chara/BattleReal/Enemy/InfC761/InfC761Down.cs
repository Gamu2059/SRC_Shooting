using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Down : BattleRealBossBehavior
{
    public enum E_PHASE
    {
        START,
        DOWN,
    }

    private InfC761DownParamSet m_ParamSet;
    private E_PHASE m_Phase;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;

    private float m_Duration;
    private float m_TimeCount;

    private float m_ShotTimeCount;

    public InfC761Down(BattleRealEnemyController enemy, BattleRealBossBehaviorParamSet paramSet) : base(enemy, paramSet)
    {
        m_ParamSet = paramSet as InfC761DownParamSet;
    }

    public override void OnStart(){
        base.OnStart();
        m_TimeCount = 0;
    }

    public override void OnUpdate(){
        base.OnUpdate();
        float x = m_ParamSet.Amplitude * Mathf.Sin(m_TimeCount);
        Vector3 pos = new Vector3(x,0f,0f);
        SetPosition(Enemy.transform.position + pos);
    }

    public override void OnFixedUpdate(){
        base.OnFixedUpdate();
        m_TimeCount += Time.fixedDeltaTime;
    }
}

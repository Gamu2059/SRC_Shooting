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
        GOTO_CENTER_FROM_LEFT,
        GOTO_CENTER_FROM_RIGHT,
        END,
    }

    private InfC761Phase1ParamSet m_ParamSet;
    private E_PHASE m_Phase;

    private float m_RadSpeed;
    private float m_Rad;

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

        m_Rad = 0;
        m_RadSpeed = Mathf.PI * 2;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var x = Mathf.Cos(m_Rad) * m_ParamSet.Amplitude / 2f;
        var pos = Enemy.transform.position;
        pos.x = x + m_ParamSet.BasePos.x;
        pos.z = m_ParamSet.BasePos.y;
        Enemy.transform.position = pos;

        m_Rad += m_RadSpeed * Time.deltaTime;
        m_Rad %= Mathf.PI * 2;
    }

    /// <summary>
    /// この行動パターンから他のパターンになった時に呼び出される
    /// </summary>
    public override void OnEnd()
    {
        base.OnEnd();
    }
}

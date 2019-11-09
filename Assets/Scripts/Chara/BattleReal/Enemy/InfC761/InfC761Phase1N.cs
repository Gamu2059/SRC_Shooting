using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Phase1N : BattleRealBossBehavior
{
    public enum E_PHASE{
        START,
    }

    public enum E_SHOT_PHASE{
        NONE,
    }

    private InfC761BehaviorParamSet m_ParamSet;
    private E_PHASE m_Phase;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;

    private float m_Duration;
    private float m_TimeCount;

    private float m_ShotTimeCount;

    private E_SHOT_PHASE m_ShotPhase;

    public InfC761Phase1N(BattleRealEnemyController enemy, BattleRealBossBehaviorParamSet paramSet) : base(enemy, paramSet)
    {
        m_ParamSet = paramSet as InfC761BehaviorParamSet;
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
        }
    }

    /// <summary>
    /// この行動パターンから他のパターンになった時に呼び出される
    /// </summary>
    public override void OnEnd()
    {
        base.OnEnd();
    }

    private Vector3 GetMovePosition(int normarizedRateIndex)
    {
        var rate = m_ParamSet.NormalizedRates[normarizedRateIndex];
        var duration = rate.keys[rate.keys.Length - 1].time;
        var t = rate.Evaluate(m_TimeCount * duration / m_Duration);
        return Vector3.Lerp(m_MoveStartPos, m_MoveEndPos, t);
    }

    private void OnMove()
    {
        switch (m_Phase)
        {
            case E_PHASE.START:
                SetPosition(GetMovePosition(0));
                if(m_TimeCount >= m_Duration){
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.MoveDurations[0];
                }
                break;
        }
    }

    protected virtual void OnShot(EnemyShotParam param, Vector3 shotPosition, int bulletIndex, int bulletParamIndex, bool isPlayerLook = false)
    {
        int num = param.Num;
        float angle = param.Angle;
        var spreadAngles = CharaController.GetBulletSpreadAngles(num, angle);
        var shotParam = new BulletShotParam();
        shotParam.Position = shotPosition + Enemy.transform.position;
        shotParam.BulletParamIndex = bulletParamIndex;
        shotParam.BulletIndex = bulletIndex;

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

    private void SetShotPhase(E_SHOT_PHASE shotPhase){
        m_ShotPhase = shotPhase;
    }

    private void OnShot()
    {
        
    }
}

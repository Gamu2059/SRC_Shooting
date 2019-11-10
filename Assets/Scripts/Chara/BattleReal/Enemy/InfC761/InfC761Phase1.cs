﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Phase1 : BattleRealBossBehavior
{
    public enum E_PHASE{
        START,
        WAIT_ON_BASEPOS,
    }

    public enum E_SHOT_PHASE{
        NONE,
        PHASE1,
    }

    public enum E_RAPID_SHOT_EDGE{
        RIGHT,
        LEFT,
    }

    private InfC761Phase1ParamSet m_ParamSet;
    private E_PHASE m_Phase;

    private bool m_IsDrift;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;

    private float m_Duration;
    private float m_TimeCount;

    private float m_LargeBulletShotTimeCount;
    private int m_NumberOfShotLargeBullet;
    private float m_SmallBulletShotTimeCount;

    private E_RAPID_SHOT_EDGE m_ShotEdge;
    private float m_RectShotBulletTimeCount;
    private bool m_IsShotRectBullet;
    private float m_RectRapidShotBulletTimeCount;
    private int m_NumberOfRapidShotRectBullet;

    private E_SHOT_PHASE m_ShotPhase;

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
        m_LargeBulletShotTimeCount = 0f;
        m_NumberOfShotLargeBullet = 0;
        m_SmallBulletShotTimeCount = 0f;
        m_ShotEdge = E_RAPID_SHOT_EDGE.RIGHT;
        m_RectShotBulletTimeCount = 0;
        m_IsShotRectBullet = false;
        m_RectRapidShotBulletTimeCount = 0f;
        m_NumberOfRapidShotRectBullet = 0;
        m_IsDrift = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(m_IsDrift){
            Drift();
        }
        OnMove();
        OnShot();      
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        m_TimeCount += Time.fixedDeltaTime;

        switch(m_ShotPhase){
            case E_SHOT_PHASE.NONE:
                
                break;
            case E_SHOT_PHASE.PHASE1:
                m_LargeBulletShotTimeCount += Time.fixedDeltaTime;
                m_SmallBulletShotTimeCount += Time.fixedDeltaTime;
                if(m_IsShotRectBullet){
                    m_RectRapidShotBulletTimeCount += Time.fixedDeltaTime;
                }else{
                    m_RectShotBulletTimeCount += Time.fixedDeltaTime;
                }
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

    private void StartDrift(){
        if(!m_IsDrift){
            m_IsDrift = true;
        }
    }

    private void Drift(){
        float x = m_ParamSet.Amplitudes[1] * Mathf.Sin(m_TimeCount);
        Vector3 pos = new Vector3(x,0f,0f);
        SetPosition(Enemy.transform.position + pos);
    }

    private void OnMove()
    {
        switch (m_Phase)
        {
            case E_PHASE.START:
                SetPosition(GetMovePosition(0));
                if(m_TimeCount >= m_Duration){
                    m_Phase = E_PHASE.WAIT_ON_BASEPOS; 
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.NextMoveWaitTimes[0];
                    m_MoveStartPos = Enemy.transform.position;
                }
                break;
            case E_PHASE.WAIT_ON_BASEPOS:
                StartDrift();
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

    private void OnShot()
    {
        switch (m_Phase)
        {
            case E_PHASE.START:
            break;
            case E_PHASE.WAIT_ON_BASEPOS:

            if(m_ShotPhase != E_SHOT_PHASE.PHASE1){
                m_ShotPhase = E_SHOT_PHASE.PHASE1;
            }

            if(m_LargeBulletShotTimeCount >= m_ParamSet.ShotParams[0].Interval){
                if(m_NumberOfShotLargeBullet >= m_ParamSet.NumberOfChangeBullet){
                    m_LargeBulletShotTimeCount = 0;
                    m_NumberOfShotLargeBullet = 0;
                    OnShot(m_ParamSet.ShotParams[2], m_ParamSet.ShotOffSets[5], 6, 0);
                    OnShot(m_ParamSet.ShotParams[2], m_ParamSet.ShotOffSets[6], 6, 0);
                    OnShot(m_ParamSet.ShotParams[2], m_ParamSet.ShotOffSets[7], 6, 0);
                    OnShot(m_ParamSet.ShotParams[2], m_ParamSet.ShotOffSets[8], 6, 0);
                    OnShot(m_ParamSet.ShotParams[2], m_ParamSet.ShotOffSets[9], 6, 0);
                    AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Shot02");
                }else{
                    m_LargeBulletShotTimeCount = 0;
                    OnShot(m_ParamSet.ShotParams[0], m_ParamSet.ShotOffSets[0], 6, 0);
                    AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Shot02");
                    m_NumberOfShotLargeBullet++;
                }
            }

            if(m_SmallBulletShotTimeCount >= m_ParamSet.ShotParams[1].Interval){
                m_SmallBulletShotTimeCount = 0;
                OnShot(m_ParamSet.ShotParams[1], m_ParamSet.ShotOffSets[1], 3, 1);
                OnShot(m_ParamSet.ShotParams[1], m_ParamSet.ShotOffSets[2], 3, 1);
                OnShot(m_ParamSet.ShotParams[1], m_ParamSet.ShotOffSets[3], 3, 1);
                OnShot(m_ParamSet.ShotParams[1], m_ParamSet.ShotOffSets[4], 3, 1);
                AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Shot02");
            }

            if(!m_IsShotRectBullet && m_RectShotBulletTimeCount >= m_ParamSet.GenericDurations[0]){
                m_RectShotBulletTimeCount = 0;
                m_IsShotRectBullet = true;
                var player = BattleRealPlayerManager.Instance.Player;
                if(player.transform.position.x >= Enemy.transform.position.x){
                    m_ShotEdge = E_RAPID_SHOT_EDGE.RIGHT;
                }else{
                    m_ShotEdge = E_RAPID_SHOT_EDGE.LEFT;
                }
            }else if(m_RectRapidShotBulletTimeCount >= m_ParamSet.ShotParams[3].Interval && m_NumberOfRapidShotRectBullet < m_ParamSet.NumberOfRapidShot){
                m_RectRapidShotBulletTimeCount = 0;
                m_NumberOfRapidShotRectBullet++;
                if(m_ShotEdge == E_RAPID_SHOT_EDGE.RIGHT){
                    OnShot(m_ParamSet.ShotParams[3], m_ParamSet.ShotOffSets[10], 5, 1);
                }else{
                    OnShot(m_ParamSet.ShotParams[3], m_ParamSet.ShotOffSets[11], 5, 1);
                }
                AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Shot02");
            }else if(m_NumberOfRapidShotRectBullet >= m_ParamSet.NumberOfRapidShot){
                m_NumberOfRapidShotRectBullet = 0;
                m_RectRapidShotBulletTimeCount = 0;
                m_IsShotRectBullet = false;
            }

            break;
        }
    }
}

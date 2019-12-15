using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Phase3OLD : BattleRealBossBehavior
{
    public enum E_PHASE{
        START,
        TRANSITION_TO_CIRCLE_MOVE,
        CIRCLE_MOVE,
        MOVE_TO_BASE_POS,
        BASE_POS,
    }

    public enum E_SHOT_PHASE{
        NONE,
        PLOOK,
        LINE,
    }

    public enum E_SHOT_START_EDGE{
        LEFT,
        RIGHT,
    }

    private InfC761Phase3ParamSetOld m_ParamSet;

    private E_PHASE m_Phase;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;
    private float m_MoveStartAngle;
    private float m_MoveEndAngle;

    private float m_Duration;
    private float m_TimeCount;

    private Vector3 m_CircleMoveCenter;
    private float m_CircleMoveRadius;

    private E_SHOT_PHASE m_ShotPhase;
    private E_SHOT_START_EDGE m_ShotStartEdge;
    private float m_ShotTimeCount;

    private float m_PlookShotTimeCount;
    private int m_PlookShotTime;

    //private float m_DirShotTimeCount;

    private float m_LineShotTimeCount;
    private int m_LineShotTime;

    public InfC761Phase3OLD(BattleRealEnemyController enemy, BattleRealBossBehaviorUnitParamSet paramSet) : base(enemy, paramSet)
    {
        m_ParamSet = paramSet as InfC761Phase3ParamSetOld;
    }

    public override void OnStart(){
        base.OnStart();

        var initPos = Enemy.transform.position;

        m_Phase = E_PHASE.START;
        m_MoveStartPos = initPos;
        m_MoveEndPos = GetArcPosition(0);
        m_TimeCount = 0;
        m_Duration = m_ParamSet.StartDuration;
        m_ShotPhase = E_SHOT_PHASE.NONE;
        m_CircleMoveCenter = m_ParamSet.BasePos;
        m_CircleMoveRadius = 0.2f;
        m_ShotTimeCount = 0;
        m_PlookShotTimeCount = 0;
        //m_DirShotTimeCount = 0;
        m_ShotStartEdge = E_SHOT_START_EDGE.LEFT;
        m_PlookShotTime = 0;
        m_LineShotTime = 0;
        m_LineShotTimeCount = 0;
    }

    public override void OnUpdate(){
        base.OnUpdate();
        OnMove();
        OnShot();
    }

    private void UpdateShotTimeCount(){
        switch(m_ShotPhase){
            case E_SHOT_PHASE.NONE:
                m_ShotTimeCount += Time.fixedDeltaTime;
                break;
            case E_SHOT_PHASE.PLOOK:
                m_PlookShotTimeCount += Time.fixedDeltaTime;
                //m_DirShotTimeCount += Time.fixedDeltaTime;
                break;
            case E_SHOT_PHASE.LINE:
                m_LineShotTimeCount += Time.fixedDeltaTime;
                break;
        }
    }

    public override void OnFixedUpdate(){
        m_TimeCount += Time.fixedDeltaTime;
        UpdateShotTimeCount();
    }

    public override void OnEnd(){
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

    private void OnMove(){
        switch (m_Phase)
        {
            case E_PHASE.START:
                SetPosition(GetMovePosition());
                if(m_TimeCount >= m_Duration){
                    m_Phase = E_PHASE.TRANSITION_TO_CIRCLE_MOVE;
                    m_MoveStartAngle = 0;
                    m_MoveEndAngle = 0;
                    m_MoveStartPos = Enemy.transform.position;
                    m_MoveEndPos = m_CircleMoveCenter + new Vector3(m_CircleMoveRadius, 0f, 0f);
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.NextMoveWaitTime;
                    
                }
                break;
            
            case E_PHASE.TRANSITION_TO_CIRCLE_MOVE:
                SetPosition(GetMovePosition());
                if(m_TimeCount >= m_Duration){
                    m_Phase = E_PHASE.CIRCLE_MOVE;
                    m_MoveStartAngle = 0;
                    m_MoveEndAngle = 0;
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.MoveDuration;
                }
                break;

            case E_PHASE.CIRCLE_MOVE:
                float x = m_CircleMoveRadius * Mathf.Cos(m_TimeCount);
                float z = m_CircleMoveRadius * Mathf.Sin(m_TimeCount);
                Vector3 pos = new Vector3(x, 0f, z);
                SetPosition(m_CircleMoveCenter + pos);
                if(m_TimeCount >= m_Duration){
                    m_Phase = E_PHASE.MOVE_TO_BASE_POS;
                    m_MoveStartPos = Enemy.transform.position;
                    m_MoveEndPos = m_ParamSet.BasePos;
                    m_Duration = m_ParamSet.NextMoveWaitTime;
                    m_TimeCount = 0;
                }
                break;

            case E_PHASE.MOVE_TO_BASE_POS:
                SetPosition(GetMovePosition());
                if(m_TimeCount >= m_Duration){
                    m_Phase = E_PHASE.BASE_POS;
                    m_Duration = m_ParamSet.MoveDuration / 2;
                    m_TimeCount = 0;
                }
                break;
            
            case E_PHASE.BASE_POS:
                if(m_TimeCount >= m_Duration){
                    m_Phase = E_PHASE.TRANSITION_TO_CIRCLE_MOVE;
                    m_MoveStartAngle = 0;
                    m_MoveEndAngle = 0;
                    m_MoveStartPos = Enemy.transform.position;
                    m_MoveEndPos = m_CircleMoveCenter + new Vector3(m_CircleMoveRadius, 0f, 0f);
                    m_TimeCount = 0;
                    m_Duration = m_ParamSet.NextMoveWaitTime;
                }
                break;
        }
    }

    protected virtual void OnShot(EnemyShotParam param, Vector3 shotPosition, int bulletIndex, int bulletParamIndex, bool isPlayerLook = false){
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

    // private void DirctionShot(){
    //     if(m_DirShotTimeCount >= m_ParamSet.ShotParams[4].Interval){
    //         m_DirShotTimeCount = 0;
    //     }else{
    //         OnShot(m_ParamSet.ShotParams[4], m_ParamSet.CenterShotOffset, 2);
    //     }
    // }

    private void PLookShot(){
        if(m_PlookShotTimeCount >= m_ParamSet.ShotParams[1].Interval){
            m_PlookShotTimeCount = 0;

            if(m_PlookShotTime >= m_ParamSet.NShotsPresets[0].NShotsNum){
                m_PlookShotTime = 0;
                m_ShotPhase = E_SHOT_PHASE.NONE;

                if(m_ShotStartEdge == E_SHOT_START_EDGE.LEFT){
                    m_ShotStartEdge = E_SHOT_START_EDGE.RIGHT;
                }else{
                    m_ShotStartEdge = E_SHOT_START_EDGE.LEFT;
                }

            }else{

                switch (m_ShotStartEdge)
                {
                    case E_SHOT_START_EDGE.LEFT:
                        OnShot(m_ParamSet.ShotParams[1], m_ParamSet.LeftShotOffset, 0, 7, true);
                        //AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Shot02");
                        m_PlookShotTime++;
                        break;
                    case E_SHOT_START_EDGE.RIGHT:
                        OnShot(m_ParamSet.ShotParams[1], m_ParamSet.RigthShotOffset, 0, 7, true);
                        //AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Shot02");
                        m_PlookShotTime++;
                        break;
                }

            }

        }
    }

    private void LineShot(){
        if(m_LineShotTimeCount >= m_ParamSet.ShotParams[2].Interval){
            m_LineShotTimeCount = 0;
            if(m_LineShotTime >= m_ParamSet.NShotsPresets[1].NShotsNum){
                m_LineShotTime = 0;

                if(m_ShotStartEdge == E_SHOT_START_EDGE.LEFT){
                    m_ShotStartEdge = E_SHOT_START_EDGE.RIGHT;
                }else{
                    m_ShotStartEdge = E_SHOT_START_EDGE.LEFT;
                }

                m_ShotPhase = E_SHOT_PHASE.NONE;
            }else{
                float r = 1.0f * m_LineShotTime / m_ParamSet.NShotsPresets[1].NShotsNum;
                m_LineShotTime++;
                float x,z;
                switch (m_ShotStartEdge)
                {
                    case E_SHOT_START_EDGE.LEFT:
                        x = Mathf.Lerp(m_ParamSet.LineShotStart.x, m_ParamSet.LineShotEnd.x, r);
                        z = Mathf.Lerp(m_ParamSet.LineShotStart.z, m_ParamSet.LineShotEnd.z, r);
                        OnShot(m_ParamSet.ShotParams[2], new Vector3(x, Enemy.transform.position.y, z), 2, 6, true);
                        //AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Shot01");
                        break;
                    case E_SHOT_START_EDGE.RIGHT:
                        x = Mathf.Lerp(m_ParamSet.LineShotEnd.x, m_ParamSet.LineShotStart.x, r);
                        z = Mathf.Lerp(m_ParamSet.LineShotEnd.z, m_ParamSet.LineShotStart.z, r);
                        OnShot(m_ParamSet.ShotParams[2], new Vector3(x, Enemy.transform.position.y, z), 2, 6, true);
                        //AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Shot01");
                        break;
                }
                
                
            }
        }
    }

    private void DoShot(){
        switch(m_ShotPhase){
            case E_SHOT_PHASE.NONE:
                if(m_Phase == E_PHASE.BASE_POS){
                    if(m_ShotTimeCount >= m_ParamSet.ShotParams[3].Interval){
                        m_ShotTimeCount = 0;
                        m_ShotPhase = E_SHOT_PHASE.LINE;
                    }
                }else{
                    if(m_ShotTimeCount >= m_ParamSet.ShotParams[5].Interval){
                        m_ShotTimeCount = 0;
                        m_ShotPhase = E_SHOT_PHASE.PLOOK;
                    }
                }
                break;
            case E_SHOT_PHASE.PLOOK:
                PLookShot();
                //DirctionShot();
                break;
            case E_SHOT_PHASE.LINE:
                LineShot();
                break;
        }
    }

    private void OnShot(){
        switch (m_Phase)
        {
            case E_PHASE.START:
                break;
            case E_PHASE.TRANSITION_TO_CIRCLE_MOVE:
                break;
            case E_PHASE.CIRCLE_MOVE:
                DoShot();
                break;
            case E_PHASE.MOVE_TO_BASE_POS:
                break;
            case E_PHASE.BASE_POS:
                DoShot();
                break;
        }
    }
}

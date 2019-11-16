using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Hacker1Phase1 : InfC761Hacker1Phase
{

    #region Field

    private InfC761Hacker1Phase1ParamSet m_ParamSet;

    #endregion

    public InfC761Hacker1Phase1(BattleHackingEnemyController enemy, BattleHackingBossBehaviorParamSet paramSet) : base(enemy, paramSet)
    {
        m_ParamSet = paramSet as InfC761Hacker1Phase1ParamSet;
    }


    public override BattleHackingBossBehaviorParamSet GetParamSet()
    {
        return m_ParamSet;
    }
}




//SetPosition(new Vector3(0.4f * Mathf.Sin(0.5f * m_Time),0,0.5f));


//// 敵本体の位置を更新する
//private void BezierPositionMoving()
//{
//    // 時刻の進行度(0～1の値)
//    float normalizedTime = m_MoveTime / m_Bezier3Points[m_NowPhase].m_Time;

//    if (m_NowPhase == 0)
//    {
//        Enemy.transform.position = BezierMoving(m_InitPos,
//                        m_Bezier3Points[0].m_ControlPoint1, m_Bezier3Points[0].m_ControlPoint2, m_Bezier3Points[0].m_EndPoint, normalizedTime);
//    }
//    else
//    {
//        Enemy.transform.position = BezierMoving(m_Bezier3Points[m_NowPhase - 1].m_EndPoint,
//            m_Bezier3Points[m_NowPhase].m_ControlPoint1, m_Bezier3Points[m_NowPhase].m_ControlPoint2, m_Bezier3Points[m_NowPhase].m_EndPoint, normalizedTime);
//    }
//}


//m_Bezier3Points[m_LoopBeginPhase - 1].m_AnchorPosition = m_Bezier3Points[m_NowPhase - 1].m_AnchorPosition;
//m_Bezier3Points[m_LoopBeginPhase - 1].m_AnchorVelocity = m_Bezier3Points[m_NowPhase - 1].m_AnchorVelocity;


//private Vector3 GetMovePosition()
//{
//    var rate = m_ParamSet.NormalizedRate;
//    var duration = rate.keys[rate.keys.Length - 1].time;
//    var t = rate.Evaluate(m_TimeCount * duration / m_Duration);
//    return Vector3.Lerp(m_MoveStartPos, m_MoveEndPos, t);
//}

//private void OnMove()
//{
//    switch (m_Phase)
//    {
//        case E_PHASE.START:
//            SetPosition(GetMovePosition());
//            if (m_TimeCount >= m_Duration)
//            {
//                m_Phase = E_PHASE.MOVE_TO_RIGHT;
//                m_MoveStartPos = m_ParamSet.BasePos;
//                m_MoveEndPos = m_ParamSet.BasePos + Vector3.right * m_ParamSet.Amplitude / 2f;
//                m_TimeCount = 0;
//                m_Duration = m_ParamSet.MoveDuration;
//            }
//            break;

//        case E_PHASE.MOVE_TO_LEFT:
//            SetPosition(GetMovePosition());
//            if (m_TimeCount >= m_Duration)
//            {
//                m_Phase = E_PHASE.WAIT_ON_LEFT;
//                m_Duration = m_ParamSet.NextMoveWaitTime;
//                m_TimeCount = 0;
//            }
//            break;

//        case E_PHASE.WAIT_ON_LEFT:
//            if (m_TimeCount >= m_Duration)
//            {
//                m_Phase = E_PHASE.MOVE_TO_RIGHT;
//                m_MoveStartPos = m_ParamSet.BasePos - Vector3.right * m_ParamSet.Amplitude / 2f;
//                m_MoveEndPos = m_ParamSet.BasePos + Vector3.right * m_ParamSet.Amplitude / 2f;
//                m_TimeCount = 0;
//                m_Duration = m_ParamSet.MoveDuration;
//            }
//            break;

//        case E_PHASE.MOVE_TO_RIGHT:
//            SetPosition(GetMovePosition());
//            if (m_TimeCount >= m_Duration)
//            {
//                m_Phase = E_PHASE.WAIT_ON_RIGHT;
//                m_Duration = m_ParamSet.NextMoveWaitTime;
//                m_TimeCount = 0;
//            }
//            break;

//        case E_PHASE.WAIT_ON_RIGHT:
//            if (m_TimeCount >= m_Duration)
//            {
//                m_Phase = E_PHASE.MOVE_TO_LEFT;
//                m_MoveStartPos = m_ParamSet.BasePos + Vector3.right * m_ParamSet.Amplitude / 2f;
//                m_MoveEndPos = m_ParamSet.BasePos - Vector3.right * m_ParamSet.Amplitude / 2f;
//                m_TimeCount = 0;
//                m_Duration = m_ParamSet.MoveDuration;
//            }
//            break;
//    }
//}
//protected virtual void OnShot(EnemyShotParam param, Vector3 shotPosition, int index, bool isPlayerLook = false)
//{
//    int num = param.Num;
//    float angle = param.Angle;
//    var spreadAngles = CharaController.GetBulletSpreadAngles(num, angle);
//    var shotParam = new CommandBulletShotParam();
//    shotParam.BulletIndex = index;
//    shotParam.Position = shotPosition + Enemy.transform.position;

//    var correctAngle = 0f;
//    if (isPlayerLook)
//    {
//        var player = BattleHackingPlayerManager.Instance.Player;
//        var delta = player.transform.position - ( shotPosition + Enemy.transform.position);
//        correctAngle = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg;
//    }

//    for (int i = 0; i < num; i++)
//    {
//        var bullet = Shot(shotParam);
//        bullet.SetRotation(new Vector3(0, spreadAngles[i] + correctAngle, 0));
//    }
//}

//private void OnShot()
//{
//    var centerParam = m_ParamSet.CenterShotParam;
//    var sideParam = m_ParamSet.SideShotParam;
//    switch (m_Phase)
//    {
//        case E_PHASE.START:
//            break;

//        case E_PHASE.WAIT_ON_LEFT:
//        case E_PHASE.WAIT_ON_RIGHT:
//            break;

//        case E_PHASE.MOVE_TO_LEFT:
//        case E_PHASE.MOVE_TO_RIGHT:
//            if (m_CenterShotTimeCount >= centerParam.Interval)
//            {
//                AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Hack_Shot01");
//                m_CenterShotTimeCount = 0;
//                if (m_GraphicsController != null)
//                {
//                    var pos = m_GraphicsController.Eye.position - Enemy.transform.position;
//                    OnShot(centerParam, pos, 0, true);
//                }
//                else
//                {
//                    OnShot(centerParam, m_ParamSet.AlternativeCenterPos, 0, true);
//                }
//            }
//            if (m_SideShotTimeCount >= sideParam.Interval)
//            {
//                AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.ENEMY, "SE_Enemy_Hack_Shot02");
//                m_SideShotTimeCount = 0;
//                OnShot(sideParam, m_ParamSet.LeftShotOffset, 1);
//                OnShot(sideParam, m_ParamSet.RigthShotOffset, 1);
//            }
//            break;
//    }
//}


//var initPos = Enemy.transform.position;

//m_Phase = E_PHASE.START;
//m_MoveStartPos = initPos;
//m_MoveEndPos = m_ParamSet.BasePos;
//m_TimeCount = 0;
//m_Duration = m_ParamSet.StartDuration;


//OnMove();
//OnShot();


//m_TimeCount += Time.fixedDeltaTime;
//m_CenterShotTimeCount += Time.fixedDeltaTime;
//m_SideShotTimeCount += Time.fixedDeltaTime;


//public enum E_PHASE
//{
//    START,
//    MOVE_TO_LEFT,
//    MOVE_TO_RIGHT,
//    WAIT_ON_LEFT,
//    WAIT_ON_RIGHT,
//}

//private E_PHASE m_Phase;

//private Vector3 m_MoveStartPos;
//private Vector3 m_MoveEndPos;

//private float m_Duration;
//private float m_TimeCount;

//private float m_CenterShotTimeCount;
//private float m_SideShotTimeCount;
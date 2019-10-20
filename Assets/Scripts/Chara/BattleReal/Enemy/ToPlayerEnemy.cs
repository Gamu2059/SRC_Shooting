using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーに目掛けて動く敵のコントローラ。
/// </summary>
public class ToPlayerEnemy : UTurnEnemy
{
    private enum E_TO_PLAYER_PHASE
    {
        BEGIN_STRAIGHT,
        WAIT,
        TO_PLAYER,
        STRAIGHT,
    }

    private E_TO_PLAYER_PHASE m_ToPlayerPhase;

    /// <summary>
    /// 最初の直線移動時の補間係数
    /// </summary>
    private float m_StraightLerp;

    /// <summary>
    /// 最初の直線移動に掛ける時間
    /// </summary>
    private float m_StraightDuration;

    // WAITステートの時の待ち時間
    private float m_WaitTime;

    private float m_StraightTimeCount;


    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();

        if (BehaviorParamSet is BattleRealEnemyToPlayerParamSet paramSet)
        {
            m_StraightLerp = paramSet.StraightLerp;
            m_StraightDuration = paramSet.StraightDuration;
            m_WaitTime = paramSet.WaitTime;
        }
        else
        {
            Debug.LogError("BehaviorParamSetが不適切です。");
        }
    }

    public override void OnStart()
    {
        base.OnStart();
        m_ToPlayerPhase = E_TO_PLAYER_PHASE.BEGIN_STRAIGHT;
        m_StraightTimeCount = 0;
    }

    protected override void Move()
    {
        float deltaTime = Time.deltaTime;

        switch (m_ToPlayerPhase)
        {
            case E_TO_PLAYER_PHASE.BEGIN_STRAIGHT:
                //var pos = Vector2.Lerp(transform.localPosition, m_FactStraightMoveEndPosition, m_StraightLerp);
                //transform.localPosition = pos;

                //m_StraightTimeCount += deltaTime;
                //if (m_StraightTimeCount >= m_StraightDuration)
                //{
                //    m_ToPlayerPhase = E_TO_PLAYER_PHASE.WAIT;
                //    var waitTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, m_WaitTime, CalcCircleMoveParam);
                //    RegistTimer("Wait", waitTimer);
                //}
                Vector3 beforeDir = m_FactStraightMoveEndPosition - transform.localPosition;
                Vector3 beginDeltaMove = m_StraightMoveDirection * m_StraightMoveSpeed * deltaTime;
                transform.localPosition += beginDeltaMove;

                if (Vector2.Dot(beforeDir.ToVector2XZ(), beginDeltaMove.ToVector2XZ()) <= 0)
                {
                    m_IsLookMoveDir = false;
                    m_ToPlayerPhase = E_TO_PLAYER_PHASE.WAIT;
                    transform.localPosition = m_FactStraightMoveEndPosition;

                    var waitTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, m_WaitTime, ()=>
                    {
                        CalcCircleMoveParam();
                        m_IsLookMoveDir = true;
                    });
                    RegistTimer("Wait", waitTimer);
                }

                break;

            case E_TO_PLAYER_PHASE.WAIT:
                break;

            case E_TO_PLAYER_PHASE.STRAIGHT:
                var endMovePos = m_StraightMoveDirection * m_StraightMoveSpeed * deltaTime;
                transform.localPosition += endMovePos;
                break;

            case E_TO_PLAYER_PHASE.TO_PLAYER:
                m_NowCircleMoveAngle += m_CircleMoveSpeed * deltaTime;
                SetPositionOnCircle(m_NowCircleMoveAngle);

                if (Mathf.Abs(m_NowCircleMoveAngle - m_BeginCircleMoveAngle) >= m_CircleMoveAngle)
                {
                    m_ToPlayerPhase = E_TO_PLAYER_PHASE.STRAIGHT;
                    float endAngle = m_BeginCircleMoveAngle + Mathf.Sign(m_CircleMoveSpeed) * m_CircleMoveAngle;
                    SetPositionOnCircle(endAngle);
                }

                break;
        }
    }

    protected void CalcCircleMoveParam()
    {
        var selfPos = transform.position.ToVector2XZ();

        var player = BattleRealPlayerManager.Instance.Player;
        var playerPos = player.transform.position.ToVector2XZ();

        var selfForward = transform.forward.ToVector2XZ();
        var deltaPos = playerPos - selfPos;
        var cross = Collision.Cross(selfForward, deltaPos);

        if (cross == 0)
        {
            // 完全に正面にいる
            m_ToPlayerPhase = E_TO_PLAYER_PHASE.STRAIGHT;
            m_StraightMoveDirection = transform.forward;
        }
        else
        {
            m_ToPlayerPhase = E_TO_PLAYER_PHASE.TO_PLAYER;
            var selfPosCircleCenterDir = Vector2.zero;
            var midPosCircleCenterDir = Vector2.zero;

            if (cross > 0)
            {
                // 正面から左側にプレイヤーがいる
                selfPosCircleCenterDir = new Vector2(-selfForward.y, selfForward.x);
                midPosCircleCenterDir = new Vector2(-deltaPos.y, deltaPos.x).normalized;
            }
            else if (cross < 0)
            {
                // 正面から右側にプレイヤーがいる
                selfPosCircleCenterDir = new Vector2(selfForward.y, -selfForward.x);
                midPosCircleCenterDir = new Vector2(deltaPos.y, -deltaPos.x).normalized;
            }

            var col1 = new CollisionLine();
            col1.p = selfPos;
            col1.v = selfPosCircleCenterDir * 1000f;

            var col2 = new CollisionLine();
            col2.p = selfPos + deltaPos / 2f;
            col2.v = midPosCircleCenterDir * 1000f;

            List<Vector2> result = null;
            var isCross = Collision.IsCollideLineToLine(col1, col2, out result);
            if (!isCross)
            {
                // 交差していないので直線移動にする
                m_ToPlayerPhase = E_TO_PLAYER_PHASE.STRAIGHT;
                m_StraightMoveDirection = transform.forward;
            }
            else
            {
                // 回転する円の相対的な中心点が求まる
                m_RelativeCircleCenterPosition = result[0];

                var r2 = (result[0]).sqrMagnitude;
                var a2 = deltaPos.sqrMagnitude;

                var cosA = (2 * r2 - a2) / (2 * r2);
                m_CircleMoveAngle = Mathf.Abs(Mathf.Acos(cosA) * Mathf.Rad2Deg);

                // 正面からどちらにプレイヤーがいるかで回転角度の符号を変える
                m_CircleMoveSpeed = Mathf.Abs(m_CircleMoveSpeed) * (cross > 0 ? 1 : -1);

                // 実際の円の中心座標を求める
                m_FactCircleCenterPosition = m_FactStraightMoveEndPosition + m_RelativeCircleCenterPosition;

                // 円の中心座標から初期角度を求める
                m_BeginCircleMoveAngle = Mathf.Atan2(m_RelativeCircleCenterPosition.z, m_RelativeCircleCenterPosition.x) * Mathf.Rad2Deg + 180;
                m_NowCircleMoveAngle = m_BeginCircleMoveAngle;
                // 円の半径を求める
                m_CircleRadius = m_RelativeCircleCenterPosition.magnitude;

                // 回転移動後の直線移動方向を反射ベクトルとして定義する
                m_StraightMoveDirection = 2 * Vector2.Dot(-selfForward, midPosCircleCenterDir) * midPosCircleCenterDir + selfForward;
            }
        }
    }
}

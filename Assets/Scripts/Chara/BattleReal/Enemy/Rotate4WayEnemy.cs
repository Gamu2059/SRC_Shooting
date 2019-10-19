using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 5砲塔持つ敵のコントローラ
/// </summary>
public class Rotate4WayEnemy : BattleRealEnemyController
{
    protected enum E_PHASE
    {
        APPEAR,
        WAIT_WITHDRAWAL,
        WITHDRAWAL,
    }

    protected float m_GunStartAngle;
    protected float m_GunRadius;

    // 直進距離
    protected float m_StraightMoveDistance;
    // 直進方向
    protected Vector3 m_StraightMoveDirection;
    // 直進移動時のラープ
    protected float m_StraightMoveLerp = 0.001f;
    // 直進移動の終了と看做すしきい値
    protected float m_LerpThreshold;
    // 直進後の座標から相対座標で撤退座標を定める
    protected Vector3 m_RelativeWithdrawalMoveEndPosition;
    // 撤退時のラープ
    protected float m_WithdrawalMoveLerp;
    // 撤退までの待機時間
    protected float m_WaitWithdrawalTime;

    // 発射パラメータ
    protected EnemyShotParam m_ShotParam;
    // 射出の回転速度
    protected float m_RotateSpeed;

    protected E_PHASE m_Phase;

    // 直進時の実際に行きつく先の終了座標
    protected Vector3 m_FactStraightMoveEndPosition;

    protected Vector3 m_FactWithdrawalMoveEndPosition;

    protected float m_ShotTimeCount;
    protected float m_NowRotateAngle;

    public override void SetArguments(string param)
    {
        base.SetArguments(param);

        m_StraightMoveDistance = m_ParamSet.FloatParam["SMD"];
        m_StraightMoveLerp = m_ParamSet.FloatParam["SML"];
        m_LerpThreshold = m_ParamSet.FloatParam["LT"];
        m_RelativeWithdrawalMoveEndPosition = m_ParamSet.V3Param["RWMEP"];
        m_WithdrawalMoveLerp = m_ParamSet.FloatParam["WML"];
        m_WaitWithdrawalTime = m_ParamSet.FloatParam["WWT"];

        m_ShotParam.Num = m_ParamSet.IntParam["BN"];
        m_ShotParam.Angle = m_ParamSet.FloatParam["BA"];

        m_RotateSpeed = m_ParamSet.FloatParam["RSPD"];

        m_NowRotateAngle = 0;
    }

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();
    }

    public override void OnStart()
    {
        base.OnStart();

        Vector3 startPos = transform.localPosition;

        // 直進時の行先を求める
        m_FactStraightMoveEndPosition = m_StraightMoveDirection * m_StraightMoveDistance + startPos;

        // 撤退時の行先を求める
        m_FactWithdrawalMoveEndPosition = m_FactStraightMoveEndPosition + m_RelativeWithdrawalMoveEndPosition;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Move();
        Shot();

        m_NowRotateAngle += m_RotateSpeed * Time.deltaTime;
        m_NowRotateAngle = m_NowRotateAngle % 360f;
    }

    protected virtual void Move()
    {
        switch (m_Phase)
        {
            case E_PHASE.APPEAR:
                Vector3 beforePos = transform.localPosition;
                Vector3 straightPos = Vector3.Lerp(transform.localPosition, m_FactStraightMoveEndPosition, m_StraightMoveLerp);
                transform.localPosition = straightPos;

                if ((m_FactStraightMoveEndPosition - straightPos).sqrMagnitude <= m_LerpThreshold)
                {
                    m_Phase = E_PHASE.WAIT_WITHDRAWAL;

                    transform.localPosition = m_FactStraightMoveEndPosition;

                    var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, m_WaitWithdrawalTime);
                    timer.SetTimeoutCallBack(() =>
                    {
                        timer = null;
                        m_Phase = E_PHASE.WITHDRAWAL;
                    });
                    RegistTimer("Wait Withdrawal", timer);

                    m_ShotTimeCount = m_ShotParam.Interval;
                }
                break;

            case E_PHASE.WAIT_WITHDRAWAL:
                break;

            case E_PHASE.WITHDRAWAL:
                Vector3 withdrawalPos = Vector3.Lerp(transform.localPosition, m_FactWithdrawalMoveEndPosition, m_WithdrawalMoveLerp);
                transform.localPosition = withdrawalPos;
                break;
        }
    }

    protected virtual void Shot()
    {
        m_ShotTimeCount += Time.deltaTime;
        if (m_ShotTimeCount < m_ShotParam.Interval)
        {
            return;
        }

        m_ShotTimeCount = 0;

        int num = m_ShotParam.Num;
        float angle = m_ShotParam.Angle;
        var spreadAngles = GetBulletSpreadAngles(num, angle);
        var shotParam = new BulletShotParam(this);
        shotParam.OrbitalIndex = -1;

        for (int i = 0; i < 4; i++)
        {
            var baseAngle = 90 * i + m_GunStartAngle;

            for (int j = 0; j < num; j++)
            {
                var bullet = BulletController.ShotBullet(shotParam);
                bullet.SetRotation(new Vector3(0, spreadAngles[i] + baseAngle, 0), E_RELATIVE.RELATIVE);

                var x = m_GunRadius * Mathf.Cos(-baseAngle);
                var z = m_GunRadius * Mathf.Sin(-baseAngle);
                bullet.SetPosition(new Vector3(x, 0, z));
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 出現してすぐに離脱する敵のコントローラ。
/// </summary>
public class FastWithdrawalEnemy : EnemyController
{
	protected enum E_PHASE
	{
		APPEAR,
		WAIT_WITHDRAWAL,
		WITHDRAWAL,
	}

	[Header( "Move Param" )]

	// 出現時、プレイヤーに近づくかどうか
	[SerializeField]
	protected bool m_ApproachPlayer;

	// 直進距離
	[SerializeField]
	protected float m_StraightMoveDistance;

	// 直進方向
	[SerializeField]
	protected Vector3 m_StraightMoveDirection;

	// 直進移動時のラープ
	[SerializeField]
	protected float m_StraightMoveLerp = 0.001f;

	// 直進移動の終了と看做すしきい値
	[SerializeField]
	protected float m_LerpThreshold;

	// 直進後の座標から相対座標で撤退座標を定める
	[SerializeField]
	protected Vector3 m_RelativeWithdrawalMoveEndPosition;

	// 撤退時のラープ
	[SerializeField]
	protected float m_WithdrawalMoveLerp;

	// 撤退までの待機時間
	[SerializeField]
	protected float m_WaitWithdrawalTime;

	[Header( "Shot Param" )]

	[SerializeField]
	protected Transform m_ShotPosition;

	// 発射パラメータ
	[SerializeField]
	protected EnemyShotParam m_ShotParam;



	// 処理分けを行うためのデータ
	protected E_PHASE m_Phase;

	// 直進時の実際に行きつく先の終了座標
	protected Vector3 m_FactStraightMoveEndPosition;

	protected Vector3 m_FactWithdrawalMoveEndPosition;

	protected Timer m_WithdrawalTimer;

    protected float m_Interval;



	public override void SetArguments( string param )
	{
		base.SetArguments( param );

		m_ApproachPlayer = m_ParamSet.BoolParam["AP"];
		m_StraightMoveDistance = m_ParamSet.FloatParam["SMD"];
		m_StraightMoveLerp = m_ParamSet.FloatParam["SML"];
		m_LerpThreshold = m_ParamSet.FloatParam["LT"];
		m_RelativeWithdrawalMoveEndPosition = m_ParamSet.V3Param["RWMEP"];
		m_WithdrawalMoveLerp = m_ParamSet.FloatParam["WML"];
		m_WaitWithdrawalTime = m_ParamSet.FloatParam["WWT"];

		m_ShotParam.Num = m_ParamSet.IntParam["BN"];
		m_ShotParam.Angle = m_ParamSet.FloatParam["BA"];
        m_ShotParam.Interval = m_ParamSet.FloatParam["BI"];
	}

	public override void OnStart()
	{
		base.OnStart();

		Vector3 startPos = transform.localPosition;

		if( m_ApproachPlayer )
		{
			var targetPos = PlayerCharaManager.Instance.GetCurrentController().transform.localPosition;
			m_StraightMoveDirection = ( targetPos - startPos ).normalized;
		}

		// 直進時の行先を求める
		m_FactStraightMoveEndPosition = m_StraightMoveDirection * m_StraightMoveDistance + startPos;

		// 撤退時の行先を求める
		m_FactWithdrawalMoveEndPosition = m_FactStraightMoveEndPosition + m_RelativeWithdrawalMoveEndPosition;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		switch( m_Phase )
		{
			case E_PHASE.APPEAR:
				Vector3 beforePos = transform.localPosition;
				Vector3 straightPos = Vector3.Lerp( transform.localPosition, m_FactStraightMoveEndPosition, m_StraightMoveLerp );
				transform.localPosition = straightPos;

				if( ( m_FactStraightMoveEndPosition - straightPos ).sqrMagnitude <= m_LerpThreshold )
				{
					m_Phase = E_PHASE.WAIT_WITHDRAWAL;

					transform.localPosition = m_FactStraightMoveEndPosition;

					m_WithdrawalTimer = Timer.CreateTimeoutTimer( E_TIMER_TYPE.SCALED_TIMER, m_WaitWithdrawalTime, () =>
					{
						m_Phase = E_PHASE.WITHDRAWAL;
					} );
					BattleMainTimerManager.Instance.RegistTimer( m_WithdrawalTimer );

					Shot();
                    m_Interval = 0;
				}

				break;

			case E_PHASE.WAIT_WITHDRAWAL:

                m_Interval += Time.deltaTime;
                if (m_Interval >= m_ShotParam.Interval)
                {
                    m_Interval = 0;
                    Shot();
                }

				break;

			case E_PHASE.WITHDRAWAL:
				Vector3 withdrawalPos = Vector3.Lerp( transform.localPosition, m_FactWithdrawalMoveEndPosition, m_WithdrawalMoveLerp );
				transform.localPosition = withdrawalPos;

				break;
		}

		var target = PlayerCharaManager.Instance.GetCurrentController().transform;
		transform.LookAt( target );
	}

	protected virtual void Shot()
	{
		int num = m_ShotParam.Num;
		float angle = m_ShotParam.Angle;
		var spreadAngles = GetBulletSpreadAngles( num, angle );
		var shotParam = new BulletShotParam( this );
		shotParam.Position = m_ShotPosition.position - transform.parent.position;

		for( int i = 0; i < num; i++ )
		{
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_RELATIVE.RELATIVE );
		}
	}

	public override void Dead()
	{
		base.Dead();

		if( m_WithdrawalTimer != null )
		{
			m_WithdrawalTimer.StopTimer();
			m_WithdrawalTimer = null;
		}
	}
}

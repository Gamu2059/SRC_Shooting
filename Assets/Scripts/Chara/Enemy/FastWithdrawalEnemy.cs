using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 出現してすぐに離脱する敵のコントローラ。
/// </summary>
public class FastWithdrawalEnemy : EnemyController
{
	private enum E_PHASE
	{
		APPEAR,
		WAIT_WITHDRAWAL,
		WITHDRAWAL,
	}

	[Header( "Move Param" )]

	// 出現時、プレイヤーに近づくかどうか
	[SerializeField]
	private bool m_ApproachPlayer;

	// 直進距離
	[SerializeField]
	private float m_StraightMoveDistance;

	// 直進方向
	[SerializeField]
	private Vector3 m_StraightMoveDirection;

	// 直進移動時のラープ
	[SerializeField]
	private float m_StraightMoveLerp;

	// 直進後の座標から相対座標で撤退座標を定める
	[SerializeField]
	private Vector3 m_RelativeWithdrawalMoveEndPosition;

	// 撤退時のラープ
	[SerializeField]
	private float m_WithdrawalMoveLerp;

	// 撤退までの待機時間
	[SerializeField]
	private float m_WaitWithdrawalTime;

	[Header( "Shot Param" )]

	[SerializeField]
	private Transform m_ShotPosition;

	// 発射パラメータ
	[SerializeField]
	private EnemyShotParam m_ShotParam;



	// 処理分けを行うためのデータ
	private E_PHASE m_Phase;

	// 直進時の実際に行きつく先の終了座標
	private Vector3 m_FactStraightMoveEndPosition;

	private Vector3 m_FactWithdrawalMoveEndPosition;



	public override void SetStringParam( string param )
	{
		base.SetStringParam( param );

		m_ApproachPlayer = m_ParamSet.BoolParam["AP"];
		m_StraightMoveDistance = m_ParamSet.FloatParam["SMD"];
		m_StraightMoveLerp = m_ParamSet.FloatParam["SML"];
		m_RelativeWithdrawalMoveEndPosition = m_ParamSet.V3Param["RWMEP"];
		m_WithdrawalMoveLerp = m_ParamSet.FloatParam["WML"];
		m_WaitWithdrawalTime = m_ParamSet.FloatParam["WWT"];

		m_ShotParam.Num = m_ParamSet.IntParam["BN"];
		m_ShotParam.Angle = m_ParamSet.FloatParam["BA"];
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

				if( ( m_FactStraightMoveEndPosition - straightPos ).sqrMagnitude <= 1f )
				{
					m_Phase = E_PHASE.WAIT_WITHDRAWAL;

					transform.localPosition = m_FactStraightMoveEndPosition;

					var timer = Timer.CreateTimeoutTimer( E_TIMER_TYPE.SCALED_TIMER, m_WaitWithdrawalTime, () =>
					{
						m_Phase = E_PHASE.WITHDRAWAL;
					} );
					BattleMainTimerManager.Instance.RegistTimer( timer );

					Shot();
				}

				break;

			case E_PHASE.WAIT_WITHDRAWAL:
				break;

			case E_PHASE.WITHDRAWAL:
				Vector3 withdrawalPos = Vector3.Lerp( transform.localPosition, m_FactWithdrawalMoveEndPosition, m_WithdrawalMoveLerp );
				transform.localPosition = withdrawalPos;

				break;
		}

		var target = PlayerCharaManager.Instance.GetCurrentController().transform;
		transform.LookAt( target );
	}

	private void Shot()
	{
		int num = m_ShotParam.Num;
		float angle = m_ShotParam.Angle;
		var spreadAngles = GetBulletSpreadAngles( num, angle );
		var shotParam = new BulletShotParam( this );
		shotParam.Position = transform.localPosition + m_ShotPosition.localPosition;

		for( int i = 0; i < num; i++ )
		{
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );
		}
	}

}

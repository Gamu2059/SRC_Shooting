using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 5砲塔持つ敵のコントローラ
/// ベースはFastWithdrawalEnemy
/// </summary>
public class Rotate5WayEnemy : EnemyController
{
	protected enum E_PHASE
	{
		APPEAR,
		WAIT_WITHDRAWAL,
		WITHDRAWAL,
	}

	[Header( "Gan Param" )]

	[SerializeField]
	protected Transform[] m_GanArray;

	[SerializeField]
	protected Transform[] m_ShotPositionArray;

	[SerializeField]
	protected float m_GanRadius;

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

	// 回転射出の射出間隔
	[SerializeField]
	protected float m_RotateShotInterval;

	// 射出の回転速度
	[SerializeField]
	protected float m_RotateSpeed;



	// 処理分けを行うためのデータ
	protected E_PHASE m_Phase;

	// 直進時の実際に行きつく先の終了座標
	protected Vector3 m_FactStraightMoveEndPosition;

	protected Vector3 m_FactWithdrawalMoveEndPosition;

	protected float m_NowRotateAngle;

	protected Timer m_WithdrawalTimer;


	public override void SetStringParam( string param )
	{
		base.SetStringParam( param );

		m_ApproachPlayer = m_ParamSet.BoolParam["AP"];
		m_StraightMoveDistance = m_ParamSet.FloatParam["SMD"];
		m_StraightMoveLerp = m_ParamSet.FloatParam["SML"];
		m_LerpThreshold = m_ParamSet.FloatParam["LT"];
		m_RelativeWithdrawalMoveEndPosition = m_ParamSet.V3Param["RWMEP"];
		m_WithdrawalMoveLerp = m_ParamSet.FloatParam["WML"];
		m_WaitWithdrawalTime = m_ParamSet.FloatParam["WWT"];

		m_ShotParam.Num = m_ParamSet.IntParam["BN"];
		m_ShotParam.Angle = m_ParamSet.FloatParam["BA"];

		m_RotateShotInterval = m_ParamSet.FloatParam["RSI"];
		m_RotateSpeed = m_ParamSet.FloatParam["RSPD"];

		m_NowRotateAngle = 0;
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

		RotateGan();

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

					m_WithdrawalTimer = Timer.CreateTimer(
					                        E_TIMER_TYPE.SCALED_TIMER,
					                        m_RotateShotInterval,
					                        m_WaitWithdrawalTime,
					                        null,
					                        () => ShotRotatingGan(),
					                        () => m_Phase = E_PHASE.WITHDRAWAL );
					BattleMainTimerManager.Instance.RegistTimer( m_WithdrawalTimer );

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

	protected virtual void Shot()
	{
		int num = m_ShotParam.Num;
		float angle = m_ShotParam.Angle;
		var spreadAngles = GetBulletSpreadAngles( num, angle );
		var shotParam = new BulletShotParam( this );
		shotParam.OrbitalIndex = -1;
		shotParam.Position = m_ShotPosition.position - transform.parent.position;

		for( int i = 0; i < num; i++ )
		{
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );
		}

		shotParam.OrbitalIndex = 1;

		for( int i = 0; i < num; i++ )
		{
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );
		}

		shotParam.OrbitalIndex = 2;

		for( int i = 0; i < num; i++ )
		{
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );
		}
	}

	protected virtual void RotateGan()
	{
		if( m_GanArray.Length < 1 )
		{
			return;
		}

		float radSpeed = m_RotateSpeed * Time.deltaTime * Mathf.Deg2Rad;
		m_NowRotateAngle += radSpeed;
		m_NowRotateAngle %= Mathf.PI * 2;

		int count = m_GanArray.Length;
		float deltaAngle = Mathf.PI * 2 / count;

		for( int i = 0; i < count; i++ )
		{
			float angle = m_NowRotateAngle + i * deltaAngle;
			var dir = new Vector3( Mathf.Cos( angle ), 0f, Mathf.Sin( angle ) );
			Vector3 pos = dir * m_GanRadius;

			m_GanArray[i].localPosition = pos;
			m_GanArray[i].forward = m_GanArray[i].position - transform.position;
		}
	}

	protected virtual void ShotRotatingGan()
	{
		if( m_GanArray.Length < 1 )
		{
			return;
		}

		var shotParam = new BulletShotParam( this );
		shotParam.BulletIndex = 1;
		shotParam.OrbitalIndex = 0;

		for( int i = 0; i < m_GanArray.Length; i++ )
		{
			shotParam.Position = m_ShotPositionArray[i].position - transform.parent.position;
			shotParam.Rotation = m_GanArray[i].eulerAngles - transform.parent.eulerAngles;
			BulletController.ShotBullet( shotParam );
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

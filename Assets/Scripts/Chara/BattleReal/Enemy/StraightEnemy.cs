using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 直線移動する敵のコントローラ。
/// </summary>
public class StraightEnemy : BattleRealEnemyController
{
	[Header( "Move Param" )]

	// 移動方向
	[SerializeField]
	protected Vector3 m_MoveDirection;

	// 移動速度
	[SerializeField]
	protected float m_MoveSpeed;

	[Header( "Shot Param" )]

	[SerializeField]
	protected Transform m_ShotPosition;

	[SerializeField]
	protected float m_VisibleOffsetShotTime;

	[SerializeField]
	protected EnemyShotParam m_ShotParam;



	protected Vector3 m_NormalizedMoveDirection;

	protected float m_ShotTimeCount;

	protected Timer m_StartShotTimer;


	public override void SetArguments( string param )
	{
		base.SetArguments( param );

		m_MoveDirection = m_ParamSet.V3Param["MD"];
		m_MoveSpeed = m_ParamSet.FloatParam["MS"];
		m_ShotParam.Interval = m_ParamSet.FloatParam["BI"];
		m_ShotParam.Num = m_ParamSet.IntParam["BN"];
		m_ShotParam.Angle = m_ParamSet.FloatParam["BA"];

		m_VisibleOffsetShotTime = m_ParamSet.FloatParam["VOST"];
	}

	public override void OnStart()
	{
		base.OnStart();

		// 直進の方向を求める
		m_NormalizedMoveDirection = m_MoveDirection.normalized;

		m_ShotTimeCount = 0f;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		Vector3 deltaPos = m_NormalizedMoveDirection * m_MoveSpeed * Time.deltaTime;
		transform.localPosition += deltaPos;

		Shot();

		//var target = BattleRealPlayerManager.Instance.GetCurrentController().transform;
		//transform.LookAt( target );
	}

	protected virtual void Shot()
	{
		m_ShotTimeCount += Time.deltaTime;

		if( m_ShotTimeCount >= m_ShotParam.Interval )
		{
			m_ShotTimeCount = 0;
			OnShot( m_ShotParam );
		}
	}

	protected virtual void OnShot( EnemyShotParam param )
	{
		if( m_ShotPosition == null )
		{
			return;
		}

		int num = param.Num;
		float angle = param.Angle;
		var spreadAngles = GetBulletSpreadAngles( num, angle );
		var shotParam = new BulletShotParam( this );
		shotParam.Position = m_ShotPosition.position - transform.parent.position;

		for( int i = 0; i < num; i++ )
		{
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_RELATIVE.RELATIVE );
		}
	}

	protected override void OnBecameVisible()
	{
		base.OnBecameVisible();

		m_StartShotTimer = Timer.CreateTimeoutTimer( E_TIMER_TYPE.SCALED_TIMER, m_VisibleOffsetShotTime, () =>
		{
			OnShot( m_ShotParam );
		} );

		//BattleRealTimerManager.Instance.RegistTimer( m_StartShotTimer );
	}

	public override void Dead()
	{
		base.Dead();

		if( m_StartShotTimer != null )
		{
			m_StartShotTimer.StopTimer();
			m_StartShotTimer = null;
		}
	}
}

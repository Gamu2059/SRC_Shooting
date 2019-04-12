using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 出現してすぐに離脱する敵のコントローラ。
/// </summary>
public class FastWithdrawalDoubleShotEnemy : FastWithdrawalEnemy
{
	[Header( "Double Shot Param" )]

	// 二発目を発射するまでの間隔
	[SerializeField]
	private float m_DoubleShotInterval;

	public override void SetStringParam( string param )
	{
		base.SetStringParam( param );

		m_DoubleShotInterval = m_ParamSet.FloatParam["DSI"];
	}

	protected override void Shot()
	{
		// 最初
		int num = m_ShotParam.Num;
		float angle = m_ShotParam.Angle;
		var spreadAngles = GetBulletSpreadAngles( num, angle );
		var shotParam = new BulletShotParam( this );
		shotParam.Position = m_ShotPosition.position - transform.parent.position;

		for( int i = 0; i < num; i++ )
		{
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );
		}

		var timer = Timer.CreateTimer( E_TIMER_TYPE.SCALED_TIMER, 0, m_DoubleShotInterval );
		timer.SetTimeoutCallBack( () =>
		{
			// 二回目
			num = m_ShotParam.Num;
			angle = m_ShotParam.Angle;
			spreadAngles = GetBulletSpreadAngles( num, angle );
			shotParam = new BulletShotParam( this );
			shotParam.Position = m_ShotPosition.position - transform.parent.position;
			shotParam.OrbitalIndex = 0;

			for( int i = 0; i < num; i++ )
			{
				var bullet = BulletController.ShotBullet( shotParam );
				bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );
			}
		} );

		BattleMainTimerManager.Instance.RegistTimer( timer );
	}
}

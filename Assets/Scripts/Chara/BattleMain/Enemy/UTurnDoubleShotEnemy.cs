using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ある円周を軌道として動く敵のコントローラ。
/// </summary>
public class UTurnDoubleShotEnemy : UTurnEnemy
{
	[Header( "Double Shot Param" )]

	// 二発目を発射するまでの間隔
	[SerializeField]
	private float m_DoubleShotInterval;

	public override void SetArguments( string param )
	{
		base.SetArguments( param );

		m_DoubleShotInterval = m_ParamSet.FloatParam["DSI"];
	}

	protected override void OnShot( EnemyShotParam param )
	{
		// 最初
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

		var timer = Timer.CreateTimer( E_TIMER_TYPE.SCALED_TIMER, 0, m_DoubleShotInterval );
		timer.SetTimeoutCallBack( () =>
		{
			// 二回目
			num = param.Num;
			angle = param.Angle;
			spreadAngles = GetBulletSpreadAngles( num, angle );
			shotParam = new BulletShotParam( this );
			shotParam.Position = m_ShotPosition.position - transform.parent.position;
			shotParam.OrbitalIndex = 0;

			for( int i = 0; i < num; i++ )
			{
				var bullet = BulletController.ShotBullet( shotParam );
				bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_RELATIVE.RELATIVE );
			}
		} );

		//BattleRealTimerManager.Instance.RegistTimer( timer );
	}
}

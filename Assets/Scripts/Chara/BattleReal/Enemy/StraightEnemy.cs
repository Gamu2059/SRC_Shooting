using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 直線移動する敵のコントローラ。
/// </summary>
public class StraightEnemy : BattleRealEnemyController
{
	protected Vector3 m_MoveDirection;
	protected float m_MoveSpeed;
	protected EnemyShotParam m_ShotParam;
	protected Vector3 m_NormalizedMoveDirection;
	protected float m_ShotTimeCount;

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();

        if (BehaviorParamSet is BattleRealEnemyStraightParamSet paramSet)
        {
            m_MoveDirection = paramSet.MoveDirection;
            m_MoveSpeed = paramSet.MoveSpeed;
            m_ShotParam = paramSet.ShotParam;
        }
    }

    public override void OnStart()
	{
		base.OnStart();

		// 直進の方向を求める
		m_NormalizedMoveDirection = m_MoveDirection.normalized;

        m_ShotTimeCount = m_ShotParam.Interval;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		Vector3 deltaPos = m_NormalizedMoveDirection * m_MoveSpeed * Time.deltaTime;
		transform.localPosition += deltaPos;

		Shot();
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

	protected virtual void OnShot( EnemyShotParam param, bool isLookPlayer = true )
	{
		int num = param.Num;
		float angle = param.Angle;
		var spreadAngles = GetBulletSpreadAngles( num, angle );
		var shotParam = new BulletShotParam( this );

        var correctAngle = 0f;
        if (isLookPlayer)
        {
            var player = BattleRealPlayerManager.Instance.Player;
            var delta = player.transform.position - transform.position;
            correctAngle = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg + 180;
        }

        for ( int i = 0; i < num; i++ )
		{
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, spreadAngles[i] + correctAngle, 0 ), E_RELATIVE.RELATIVE );
		}
	}
}

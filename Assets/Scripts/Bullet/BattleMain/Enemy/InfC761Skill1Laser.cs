using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// INF-C-761のスキル1のレーザー
/// </summary>
public class InfC761Skill1Laser : BulletController
{
	private enum E_PHASE
	{
		SHOT,
		THROW,
		MOVE_BULLETS,
	}

	/// <summary>
	/// INF-C-761のスキル1のレーザーパラメータ
	/// </summary>
	[System.Serializable]
	public struct ConstParam
	{
		// SHOT時のレーザーの最大長
		public float m_MaxLength;

		// SHOTからTHROWになるまでの待機時間
		public float m_ThrowWaitTime;

		// THROWからMOVE_BULLETになるまでの待機時間
		public float m_MoveBulletWaitTime;

		// 振りかざし速度
		public float m_ThrowSpeed;

		// 弾を生成するタイミングの個数
		public int m_BulletCreateTimingNum;

		// 弾を生成する個数
		public int m_BulletCreateNum;

		// 弾を生成する単位の距離間隔
		public float m_BulletCreateUnitDistance;

		// 弾が飛んでいくばらつき度合いの角度
		public float m_SpreadBulletAngle;

		// 弾が飛んでいく加速度
		public float m_SpreadBulletAccel;

		// 拡散する弾のインデックス
		public int m_BulletIndex;

		// 拡散する弾の弾パラメータインデックス
		public int m_BulletParamIndex;

		// 拡散する弾の軌道パラメータのインデックス
		public int m_OrbitalIndex;
	}



	// このレーザーのパラメータ
	private ConstParam m_Param;

	// レーザーのフェーズ
	private E_PHASE m_Phase;

	// タイムカウント
	private float m_TimeCount;

	// 弾リスト
	private List<BulletController> m_Bullets;



	public void SetParam( ConstParam param )
	{
		m_Param = param;
	}

	public override void OnStart()
	{
		base.OnStart();

		m_Phase = E_PHASE.SHOT;
		m_TimeCount = 0;
		m_Bullets = new List<BulletController>();
	}

	public override void OnUpdate()
	{
		m_TimeCount += Time.deltaTime;

		switch( m_Phase )
		{
			case E_PHASE.SHOT:
				OnShotUpdate();

				if( m_TimeCount >= m_Param.m_ThrowWaitTime )
				{
					m_Phase = E_PHASE.THROW;
					m_TimeCount = 0;

					if( m_Param.m_BulletCreateTimingNum > 0 )
					{
						float interval = m_Param.m_MoveBulletWaitTime / m_Param.m_BulletCreateTimingNum;
						var createBulletTimer = Timer.CreateIntervalTimer( E_TIMER_TYPE.SCALED_TIMER, interval, () => CreateBullets() );
						RegistTimer( "CreateBulletTimer", createBulletTimer );
					}
				}

				break;

			case E_PHASE.THROW:
				OnThrowUpdate();

				if( m_TimeCount >= m_Param.m_MoveBulletWaitTime )
				{
					m_Phase = E_PHASE.MOVE_BULLETS;
					m_TimeCount = 0;
				}

				break;

			case E_PHASE.MOVE_BULLETS:
				OnMoveBulletsUpdate();
				break;
		}
	}

	private void OnShotUpdate()
	{
		if( GetScale().z < m_Param.m_MaxLength )
		{
			SetScale( GetNowDeltaScale() * Time.deltaTime, E_ATTACK_PARAM_RELATIVE.RELATIVE );
		}
	}

	private void OnThrowUpdate()
	{
		SetRotation( new Vector3( 0, m_Param.m_ThrowSpeed * Time.deltaTime, 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );

		float nextAngle = GetRotation().y;

		float progressRatio = 0;

		if( m_Param.m_MoveBulletWaitTime != 0 )
		{
			progressRatio = m_TimeCount / m_Param.m_MoveBulletWaitTime;
		}

		// 0,1でクランプするとレーザーの発色がおかしくなる
		progressRatio = Mathf.Clamp( progressRatio, 0, 0.99f );
		var scale = GetScale();
		scale.x = 1 - progressRatio;
		SetScale( scale );
	}

	private void OnMoveBulletsUpdate()
	{
		float randomRangeAngle = m_Param.m_SpreadBulletAngle / 2f;

		foreach( var bullet in m_Bullets )
		{
			float randomAngle = Random.Range( -randomRangeAngle, randomRangeAngle );
			bullet.SetRotation( new Vector3( 0, randomAngle, 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );
			bullet.SetNowAccel( m_Param.m_SpreadBulletAccel );
		}

		DestroyBullet();
	}

	private void CreateBullets()
	{
		Vector3 basePos = GetPosition();
		Vector3 dir = transform.forward;

		var shotParam = new BulletShotParam( GetBulletOwner() );
		float shotAngle = GetRotation().y;

		for( int j = 0; j < m_Param.m_BulletCreateNum; j++ )
		{
			shotParam.Position = dir * m_Param.m_BulletCreateUnitDistance * ( j + 1 ) + basePos;
			shotParam.Rotation = new Vector3( 0, shotAngle + Mathf.Sign( m_Param.m_ThrowSpeed ), 0 );
			shotParam.BulletIndex = m_Param.m_BulletIndex;
			shotParam.BulletParamIndex = m_Param.m_BulletParamIndex;
			shotParam.OrbitalIndex = m_Param.m_OrbitalIndex;

			var bullet = ShotBullet( shotParam );
			m_Bullets.Add( bullet );
		}
	}
}

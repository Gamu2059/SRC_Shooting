using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ1のボスINF-C-761のコントローラ
/// </summary>
public class InfC761_ : BattleRealEnemyController
{
	private enum E_PHASE
	{
		NORMAL1,
		NORMAL2,
		NORMAL3,
		SKILL1,
		SKILL2,
		SKILL3,
	}

	private enum E_SKILL1_PHASE
	{
		WAIT_RIGHT_LASER,
		RIGHT_LASER_SHOT,
		RIGHT_LASER_THROW,
		WAIT_LEFT_LASER,
		LEFT_LASER_SHOT,
		LEFT_LASER_THROW,
	}

	private enum E_SKILL2_PHASE
	{
		SHOT_BOMB,
		WAIT_FIRE_BOMB,
		FIRE_BOMB,
		WAIT_SHOT_BOMB,
	}

	private enum E_SKILL3_PHASE
	{
		LASER_SHOT,
		WAIT_CENTER_LASER_SHOT,
		CENTER_LASER_SHOT,
		SPREAD_BULLETS,
	}



	/// <summary>
	/// 通常時のランダムな移動座標を定めるための構造体
	/// </summary>
	[System.Serializable]
	private struct NormalMoveTargetPosition
	{
		public Vector3[] Pos;
	}

	[System.Serializable]
	private struct ShotIndex
	{
		public int BulletIndex;
		public int BulletParamIndex;
		public int OrbitalParamIndex;
	}

	private static E_PHASE[] PHASES = new[]
	{
		E_PHASE.NORMAL1,
		E_PHASE.SKILL1,
		E_PHASE.NORMAL2,
		E_PHASE.SKILL2,
		E_PHASE.NORMAL3,
		E_PHASE.SKILL3,
	};


	#region Field Inspector

	[SerializeField]
	private float m_ChangePhaseTime;

	[Header( "Shot Position" )]

	// このキャラから見て右側のレールガンの射出点
	[SerializeField]
	private Transform m_RightRailgunShotPosition;

	// このキャラから見て左側のレールガンの射出点
	[SerializeField]
	private Transform m_LeftRailgunShotPosition;

	[Header( "Normal Move Param" )]

	// 通常移動の目標地点
	[SerializeField]
	private NormalMoveTargetPosition[] m_NormalMoveTargetPositions;

	// 目標地点を切り替える最小時間
	[SerializeField]
	private float m_NormalMinMoveInterval;

	// 目標地点を切り替える最大時間
	[SerializeField]
	private float m_NormalMaxMoveInterval;

	// 目標地点に行く時の線形補完係数
	[SerializeField]
	private float m_NormalMoveLerp;

	// 目標地点に到達したと判断する線形補完の閾値
	[SerializeField]
	private float m_StayThreshold;

	[Header( "Skill Move Param" )]

	[SerializeField]
	private float m_SkillMinMoveInterval;

	[SerializeField]
	private float m_SkillMaxMoveInterval;

	[SerializeField]
	private float m_SkillLookAtLerp;

	#region Field Inspector Normal Param

	[Space()]

	[Header( "Normal1 Param" )]

	// 通常1のショットパラメータ
	[SerializeField]
	private EnemyShotParam m_Normal1ShotParam;

	// プレイヤーを基準にした時の相対角度
	[SerializeField]
	private float m_Normal1RelativeShotRotation;

	[SerializeField]
	private ShotIndex m_Normal1ShotIndex;

	[Header( "Normal2 Param" )]

	// 通常2のショットパラメータ
	[SerializeField]
	private EnemyShotParam m_Normal2ShotParam;

	[SerializeField]
	private ShotIndex m_Normal2ShotIndex;

	[Header( "Normal3 Param" )]

	// 通常3のショットパラメータ
	[SerializeField]
	private EnemyShotParam m_Normal3ShotParam;

	[SerializeField]
	private ShotIndex m_Normal3ShotIndex;

	#endregion

	[Space()]

	#region Field Inspector Skill1 Param

	[Header( "Skill1 Param" )]

	[SerializeField]
	private ShotIndex m_Skill1NormalLaserIndex;

	// レーザー射出までの待機時間
	[SerializeField]
	private float m_Skill1WaitShotLaserTime;

	// レーザーに関するパラメータ
	[SerializeField]
	private InfC761Skill1Laser.ConstParam m_Skill1LaserConstParam;

	// レーザーを振りかざす角度
	[SerializeField]
	private float m_Skill1ThrowSpeed;

	// 次にくるレーザーの動作開始までの待機時間
	[SerializeField]
	private float m_Skill1WaitNextLaserTime;

	#endregion

	#region Field Inspector Skill2 Param

	[Header( "Skill2 Param" )]

	[SerializeField]
	private ShotIndex m_Skill2FireBulletShotIndex;

	[SerializeField]
	private int m_Skill2SpreadOrbitalBaseIndex;

	// 起爆弾に関するパラメータ
	[SerializeField]
	private InfC761Skill2FireBullet.ConstParam m_Skill2FireBulletConstParam;

	// 次の起爆弾を撃つまでの待機時間
	[SerializeField]
	private float m_Skill2WaitNextShotFireBulletTime;

	// 撃ってから起爆を開始するまでの待機時間
	[SerializeField]
	private float m_Skill2WaitFireTime;

	// 次の起爆弾を起爆するまでの待機時間
	[SerializeField]
	private float m_Skill2WaitNextFireTime;

	// 次のスキル2の一通りの動作を開始するまでの待機時間
	[SerializeField]
	private float m_Skill2WaitNextSkill2Time;

	#endregion

	#region Field Inspector Skill3 Param

	[Header( "Skill3 Param" )]

	// スキル3を発動する時の開始座標
	[SerializeField]
	private Vector2 m_Skill3EndPosition;

	// スキル3通常レーザーインデックス
	[SerializeField]
	private ShotIndex m_Skill3NormalLaserIndex;

	// スキル3巨大レーザーインデックス
	[SerializeField]
	private ShotIndex m_Skill3CenterLaserIndex;

	// スキル3の通常レーザーに関するパラメータ
	[SerializeField]
	private InfC761Skill3NormalLaser.ConstParam m_Skill3NormalLaserParam;

	// スキル3の巨大レーザーに関するパラメータ
	[SerializeField]
	private InfC761Skill3CenterLaser.ConstParam m_Skill3CenterLaserParam;

	// 通常レーザーの射撃の角度間隔
	[SerializeField]
	private float m_Skill3NormalLaserUnitAngle;

	// 通常レーザーの射撃回数
	[SerializeField]
	private int m_Skill3NormalLaserShotNum;

	// 次の通常レーザーを撃つまでの待機時間
	[SerializeField]
	private float m_Skill3WaitNextNormalLaserTime;

	// 巨大レーザーのチャージまでの待機時間
	[SerializeField]
	private float m_Skill3WaitCenterLaserChargeTime;

	// 巨大レーザーのチャージ時間
	[SerializeField]
	private float m_Skill3CenterLaserChargeTime;

	// 巨大レーザーを照射し続ける時間
	[SerializeField]
	private float m_Skill3CenterLaserShotTime;

	#endregion

	#endregion



	[Space()]

	[SerializeField]
	private E_PHASE m_Phase;

	[SerializeField]
	private E_SKILL1_PHASE m_Skill1Phase;

	[SerializeField]
	private E_SKILL2_PHASE m_Skill2Phase;

	[SerializeField]
	private E_SKILL3_PHASE m_Skill3Phase;


	private int m_PhaseIndex;

	// Normal系

	private float m_NormalMoveTimeCount;
	private float m_NormalMoveInterval;
	private int m_NormalMoveCurrentPosIndex;
	private int m_NormalMovePosDir;
	private Vector3 m_FactNormalMoveTargetPos;
	private float m_NormalShotInterval;

	private bool m_IsStay;

	// Skill系

	private float m_SkillMoveTimeCount;
	private float m_SkillMoveInterval;
	private float m_SkillShotTimeCount;

	// Skill1系
	private InfC761Skill1Laser m_Skill1RightLaser;
	private InfC761Skill1Laser m_Skill1LeftLaser;

	// Skill2系
	private bool m_Skill2IsShotRight;
	private int m_Skill2FireBulletIndex;
	private InfC761Skill2FireBullet[] m_Skill2FireBullets;

	// Skill3系
	private bool m_Skill3IsShotRight;
	private int m_Skill3NormalLaserAngleIndex;
	private List<BulletController> m_Skill3NormalLaserFireBullets;
	private InfC761Skill3CenterLaser m_Skill3RightCenterLaser;
	private InfC761Skill3CenterLaser m_Skill3LeftCenterLaser;





	public override void SetArguments( string param )
	{
		base.SetArguments( param );
	}

	public override void OnStart()
	{
		base.OnStart();

		ResetNormalTimeCount();
		// 最初の行先の決定
		DecideStartNormalTargetPos();
		m_IsStay = false;

		m_Phase = E_PHASE.NORMAL1;
		m_PhaseIndex = 0;
		m_Skill2FireBullets = new InfC761Skill2FireBullet[3];

		m_Phase = E_PHASE.NORMAL1;
		var timer = Timer.CreateIntervalTimer( E_TIMER_TYPE.SCALED_TIMER, m_ChangePhaseTime, () =>
		{
			ChangePhase();
		} );
		RegistTimer( "aa", timer );
	}

	private void ChangePhase()
	{
		m_PhaseIndex = ( m_PhaseIndex + 1 ) % PHASES.Length;
		m_Phase = PHASES[m_PhaseIndex];

		switch( m_Phase )
		{
			case E_PHASE.NORMAL1:
			case E_PHASE.NORMAL2:
			case E_PHASE.NORMAL3:
				ResetNormalTimeCount();
				DecideNormalTargetPos();
				m_IsStay = false;
				break;

			case E_PHASE.SKILL1:
				m_Skill1Phase = E_SKILL1_PHASE.WAIT_RIGHT_LASER;
				ResetSkillTimeCount();
				DecideNormalTargetPos();
				m_IsStay = false;
				break;

			case E_PHASE.SKILL2:
				m_Skill2Phase = E_SKILL2_PHASE.SHOT_BOMB;
				ResetSkillTimeCount();
				DecideNormalTargetPos();
				m_IsStay = false;
				break;

			case E_PHASE.SKILL3:
				ChangeToSkill3();
				break;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		Move();
		Shot();
	}

	private void ResetNormalTimeCount()
	{
		m_NormalMoveTimeCount = 0;
		m_NormalMoveInterval = Random.Range( m_NormalMinMoveInterval, m_NormalMaxMoveInterval );
	}

	private void ResetSkillTimeCount()
	{
		m_SkillMoveTimeCount = 0;
		m_SkillMoveInterval = Random.Range( m_SkillMinMoveInterval, m_SkillMaxMoveInterval );
	}

	private void DecideStartNormalTargetPos()
	{
		int groupNum = m_NormalMoveTargetPositions.Length;
		m_NormalMoveCurrentPosIndex = Random.Range( 0, groupNum );
		var targetPositions = m_NormalMoveTargetPositions[m_NormalMoveCurrentPosIndex];
		int posNum = targetPositions.Pos.Length;
		var viewPos = targetPositions.Pos[Random.Range( 0, posNum )];

		m_FactNormalMoveTargetPos = BattleRealCameraManager.Instance.GetViewportWorldPoint( viewPos.x, viewPos.z ) - transform.parent.position;

		if( m_NormalMoveCurrentPosIndex < 1 )
		{
			m_NormalMovePosDir = 1;
		}
		else if( m_NormalMoveCurrentPosIndex >= groupNum - 1 )
		{
			m_NormalMovePosDir = -1;
		}
		else
		{
			m_NormalMovePosDir = Random.Range( 0, 2 ) * 2 - 1;
		}
	}

	private void DecideNormalTargetPos()
	{
		int groupNum = m_NormalMoveTargetPositions.Length;
		m_NormalMoveCurrentPosIndex += m_NormalMovePosDir;
		var targetPositions = m_NormalMoveTargetPositions[m_NormalMoveCurrentPosIndex];
		int posNum = targetPositions.Pos.Length;
		var viewPos = targetPositions.Pos[Random.Range( 0, posNum )];

		m_FactNormalMoveTargetPos = BattleRealCameraManager.Instance.GetViewportWorldPoint( viewPos.x, viewPos.z ) - transform.parent.position;

		if( m_NormalMoveCurrentPosIndex < 1 )
		{
			m_NormalMovePosDir = 1;
		}
		else if( m_NormalMoveCurrentPosIndex >= groupNum - 1 )
		{
			m_NormalMovePosDir = -1;
		}
	}

	private void DeceideSkill3TargetPos()
	{
		m_FactNormalMoveTargetPos = BattleRealCameraManager.Instance.GetViewportWorldPoint( m_Skill3EndPosition.x, m_Skill3EndPosition.y ) - transform.parent.position;
	}

	#region Move Method

	private void Move()
	{
		switch( m_Phase )
		{
			case E_PHASE.NORMAL1:
			case E_PHASE.NORMAL2:
			case E_PHASE.NORMAL3:
				MoveNormal();

				break;

			case E_PHASE.SKILL1:
				MoveSkill1();
				break;

			case E_PHASE.SKILL2:
				MoveSkill2();
				break;

			case E_PHASE.SKILL3:
				MoveSkill3();
				break;
		}
	}

	private void MoveNormal()
	{
		if( m_IsStay )
		{
			m_NormalMoveTimeCount += Time.deltaTime;

			if( m_NormalMoveTimeCount >= m_NormalMoveInterval )
			{
				ResetNormalTimeCount();
				DecideNormalTargetPos();
				m_IsStay = false;
			}
		}

		Vector3 pos = Vector3.Lerp( transform.localPosition, m_FactNormalMoveTargetPos, m_NormalMoveLerp );
		transform.localPosition = pos;

		if( !m_IsStay && ( m_FactNormalMoveTargetPos - pos ).sqrMagnitude <= m_StayThreshold )
		{
			m_IsStay = true;
			m_NormalShotInterval = float.MaxValue;
		}

		//var target = BattleRealPlayerManager.Instance.GetCurrentController().transform;
		//transform.LookAt( target );
	}

	private void MoveSkill1()
	{
		Vector3 pos = Vector3.Lerp( transform.localPosition, m_FactNormalMoveTargetPos, m_NormalMoveLerp );
		transform.localPosition = pos;

		if( !m_IsStay && ( m_FactNormalMoveTargetPos - pos ).sqrMagnitude <= m_StayThreshold )
		{
			m_IsStay = true;
		}


		switch( m_Skill1Phase )
		{
			case E_SKILL1_PHASE.WAIT_LEFT_LASER:
			case E_SKILL1_PHASE.WAIT_RIGHT_LASER:
				//var target = BattleRealPlayerManager.Instance.GetCurrentController().transform;
				//var forward = Vector3.Lerp( transform.forward, ( target.position - transform.position ), m_SkillLookAtLerp );
				//transform.forward = forward;
				break;
		}
	}

	private void MoveSkill2()
	{
		Vector3 pos = Vector3.Lerp( transform.localPosition, m_FactNormalMoveTargetPos, m_NormalMoveLerp );
		transform.localPosition = pos;

		if( !m_IsStay && ( m_FactNormalMoveTargetPos - pos ).sqrMagnitude <= m_StayThreshold )
		{
			m_IsStay = true;
		}

		//var target = BattleRealPlayerManager.Instance.GetCurrentController().transform;
		//transform.LookAt( target );
	}

	private void MoveSkill3()
	{
		Vector3 pos = Vector3.Lerp( transform.localPosition, m_FactNormalMoveTargetPos, m_NormalMoveLerp );
		transform.localPosition = pos;

		if( !m_IsStay && ( m_FactNormalMoveTargetPos - pos ).sqrMagnitude <= m_StayThreshold )
		{
			m_IsStay = true;
		}

		Vector3 forward = Vector3.Lerp( transform.forward, Vector3.back, m_SkillLookAtLerp );
		transform.forward = forward;
	}

	#endregion

	#region Shot Method

	private void Shot()
	{
		switch( m_Phase )
		{
			case E_PHASE.NORMAL1:
				if( m_IsStay )
				{
					ShotNormal1();
				}

				break;

			case E_PHASE.NORMAL2:
				if( m_IsStay )
				{
					ShotNormal2();
				}

				break;

			case E_PHASE.NORMAL3:
				if( m_IsStay )
				{
					ShotNormal3();
				}

				break;

			case E_PHASE.SKILL1:
				if( m_IsStay )
				{
					ShotSkill1();
				}

				break;

			case E_PHASE.SKILL2:
				if( m_IsStay )
				{
					ShotSkill2();
				}

				break;

			case E_PHASE.SKILL3:
				if( m_IsStay )
				{
					ShotSkill3();
				}

				break;
		}
	}



	private void ShotNormal1()
	{
		m_NormalShotInterval += Time.deltaTime;

		if( m_NormalShotInterval >= m_Normal1ShotParam.Interval )
		{
			m_NormalShotInterval = 0;
			OnShotNormal1( m_LeftRailgunShotPosition, true );
			OnShotNormal1( m_RightRailgunShotPosition, false );
		}
	}

	private void OnShotNormal1( Transform shotPosition, bool isLeft )
	{
		var shotParam = new BulletShotParam( this );
		shotParam.BulletIndex = m_Normal1ShotIndex.BulletIndex;
		shotParam.BulletParamIndex = m_Normal1ShotIndex.BulletParamIndex;
		shotParam.Position = shotPosition.position - transform.parent.position;

		float deltaRotation = ( isLeft ? -1 : 1 ) * m_Normal1RelativeShotRotation;

		for( int i = 0; i < 3; i++ )
		{
			shotParam.OrbitalIndex = i;
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, deltaRotation, 0 ), E_RELATIVE.RELATIVE );
		}
	}



	private void ShotNormal2()
	{
		m_NormalShotInterval += Time.deltaTime;

		if( m_NormalShotInterval >= m_Normal2ShotParam.Interval )
		{
			m_NormalShotInterval = 0;

			var shotParam = new BulletShotParam( this );
			shotParam.BulletIndex = m_Normal2ShotIndex.BulletIndex;
			shotParam.BulletParamIndex = m_Normal2ShotIndex.BulletParamIndex;
			shotParam.OrbitalIndex = m_Normal2ShotIndex.OrbitalParamIndex;

			// 後にパラメータ化
			int num = m_Normal2ShotParam.Num;
			float deltaAngle = Mathf.PI * 2 / num;

			for( int i = 0; i < num; i++ )
			{
				float angle = deltaAngle * i;

				// z軸が0度とするために、パラメータの使い方が数学的に逆
				var dir = new Vector3( Mathf.Sin( angle ), 0, Mathf.Cos( angle ) );
				var bullet = BulletController.ShotBullet( shotParam );

				bullet.SetRotation( new Vector3( 0, angle * Mathf.Rad2Deg, 0 ), E_RELATIVE.RELATIVE );
				bullet.SetPosition( dir * 5f, E_RELATIVE.RELATIVE );
			}
		}
	}



	private void ShotNormal3()
	{
		m_NormalShotInterval += Time.deltaTime;

		if( m_NormalShotInterval >= m_Normal3ShotParam.Interval )
		{
			m_NormalShotInterval = 0;
			OnShotNormal3( m_LeftRailgunShotPosition );
			OnShotNormal3( m_RightRailgunShotPosition );
		}
	}

	private void OnShotNormal3( Transform shotPosition )
	{
		int num = m_Normal3ShotParam.Num;
		float angle = m_Normal3ShotParam.Angle;
		var spreadAngles = GetBulletSpreadAngles( num, angle );

		var shotParam = new BulletShotParam( this );
		shotParam.BulletIndex = m_Normal3ShotIndex.BulletIndex;
		shotParam.BulletParamIndex = m_Normal3ShotIndex.BulletParamIndex;
		shotParam.Position = shotPosition.position - transform.parent.position;

		for( int i = 0; i < 3; i++ )
		{
			shotParam.OrbitalIndex = i;

			for( int j = 0; j < num; j++ )
			{
				var bullet = BulletController.ShotBullet( shotParam );
				bullet.SetRotation( new Vector3( 0, spreadAngles[j], 0 ), E_RELATIVE.RELATIVE );
			}
		}
	}



	private void ShotSkill1()
	{
		m_SkillShotTimeCount += Time.deltaTime;

		switch( m_Skill1Phase )
		{
			case E_SKILL1_PHASE.WAIT_RIGHT_LASER:
				if( m_SkillShotTimeCount >= m_Skill1WaitShotLaserTime )
				{
					m_SkillShotTimeCount = 0;
					m_Skill1Phase = E_SKILL1_PHASE.RIGHT_LASER_SHOT;

					ShotSkill1Laser( m_RightRailgunShotPosition, true );
				}

				break;

			case E_SKILL1_PHASE.RIGHT_LASER_SHOT:
				if( m_SkillShotTimeCount >= m_Skill1LaserConstParam.m_ThrowWaitTime )
				{
					m_SkillShotTimeCount = 0;
					m_Skill1Phase = E_SKILL1_PHASE.RIGHT_LASER_THROW;
				}

				break;

			case E_SKILL1_PHASE.RIGHT_LASER_THROW:
				if( m_SkillShotTimeCount >= m_Skill1LaserConstParam.m_MoveBulletWaitTime + m_Skill1WaitNextLaserTime )
				{
					m_SkillShotTimeCount = 0;
					m_Skill1Phase = E_SKILL1_PHASE.WAIT_LEFT_LASER;
				}

				break;

			case E_SKILL1_PHASE.WAIT_LEFT_LASER:
				if( m_SkillShotTimeCount >= m_Skill1WaitShotLaserTime )
				{
					m_SkillShotTimeCount = 0;
					m_Skill1Phase = E_SKILL1_PHASE.LEFT_LASER_SHOT;

					ShotSkill1Laser( m_LeftRailgunShotPosition, false );
				}

				break;

			case E_SKILL1_PHASE.LEFT_LASER_SHOT:
				if( m_SkillShotTimeCount >= m_Skill1LaserConstParam.m_ThrowWaitTime )
				{
					m_SkillShotTimeCount = 0;
					m_Skill1Phase = E_SKILL1_PHASE.LEFT_LASER_THROW;
				}

				break;

			case E_SKILL1_PHASE.LEFT_LASER_THROW:
				if( m_SkillShotTimeCount >= m_Skill1LaserConstParam.m_MoveBulletWaitTime + m_Skill1WaitNextLaserTime )
				{
					m_SkillShotTimeCount = 0;
					m_Skill1Phase = E_SKILL1_PHASE.WAIT_RIGHT_LASER;

					ResetSkillTimeCount();
					DecideNormalTargetPos();
					m_IsStay = false;
				}

				break;
		}
	}

	private void ShotSkill1Laser( Transform shotPosition, bool isRight )
	{
		var shotParam = new BulletShotParam( this );
		shotParam.BulletIndex = m_Skill1NormalLaserIndex.BulletIndex;
		shotParam.BulletParamIndex = m_Skill1NormalLaserIndex.BulletParamIndex;
		shotParam.OrbitalIndex = m_Skill1NormalLaserIndex.OrbitalParamIndex;
		shotParam.Position = shotPosition.position - transform.parent.position;
		shotParam.Rotation = shotPosition.eulerAngles - transform.parent.eulerAngles;

		var laser = BulletController.ShotBullet( shotParam );
		m_Skill1RightLaser = laser as InfC761Skill1Laser;
		m_Skill1LaserConstParam.m_ThrowSpeed = m_Skill1ThrowSpeed * ( isRight ? 1 : -1 );
		m_Skill1RightLaser.SetParam( m_Skill1LaserConstParam );
	}



	private void ShotSkill2()
	{
		m_SkillShotTimeCount += Time.deltaTime;

		switch( m_Skill2Phase )
		{
			case E_SKILL2_PHASE.SHOT_BOMB:
				if( m_SkillShotTimeCount >= m_Skill2WaitNextShotFireBulletTime )
				{
					m_SkillShotTimeCount = 0;

					var shotParam = new BulletShotParam( this );
					shotParam.BulletIndex = m_Skill2FireBulletShotIndex.BulletIndex;
					shotParam.BulletParamIndex = m_Skill2FireBulletShotIndex.BulletParamIndex;

					if( m_Skill2IsShotRight )
					{
						shotParam.Position = m_RightRailgunShotPosition.position - transform.parent.position;
					}
					else
					{
						shotParam.Position = m_LeftRailgunShotPosition.position - transform.parent.position;
					}

					shotParam.OrbitalIndex = m_Skill2FireBulletIndex;
					var bullet = BulletController.ShotBullet( shotParam );
					var fireBullet = bullet as InfC761Skill2FireBullet;

					m_Skill2FireBulletConstParam.m_OrbitalIndex = m_Skill2SpreadOrbitalBaseIndex + m_Skill2FireBulletIndex;
					fireBullet.SetParam( m_Skill2FireBulletConstParam );
					m_Skill2FireBullets[m_Skill2FireBulletIndex] = fireBullet;
					m_Skill2FireBulletIndex++;

					if( m_Skill2FireBulletIndex >= 3 )
					{
						m_Skill2FireBulletIndex = 0;
						m_Skill2Phase = E_SKILL2_PHASE.WAIT_FIRE_BOMB;
					}
				}

				break;

			case E_SKILL2_PHASE.WAIT_FIRE_BOMB:
				if( m_SkillShotTimeCount >= m_Skill2WaitFireTime )
				{
					m_SkillShotTimeCount = 0;
					m_Skill2Phase = E_SKILL2_PHASE.FIRE_BOMB;
				}

				break;

			case E_SKILL2_PHASE.FIRE_BOMB:
				if( m_SkillShotTimeCount >= m_Skill2WaitNextFireTime )
				{
					m_SkillShotTimeCount = 0;

					m_Skill2FireBullets[m_Skill2FireBulletIndex].FireSpreadBullet();
					m_Skill2FireBulletIndex++;

					if( m_Skill2FireBulletIndex >= 3 )
					{
						m_Skill2FireBulletIndex = 0;
						m_Skill2Phase = E_SKILL2_PHASE.WAIT_SHOT_BOMB;
					}
				}

				break;

			case E_SKILL2_PHASE.WAIT_SHOT_BOMB:
				if( m_SkillShotTimeCount >= m_Skill2WaitNextSkill2Time )
				{
					m_Skill2Phase = E_SKILL2_PHASE.SHOT_BOMB;
					m_Skill2IsShotRight = !m_Skill2IsShotRight;

					ResetSkillTimeCount();
					DecideNormalTargetPos();
					m_IsStay = false;
				}

				break;
		}
	}



	private void ShotSkill3()
	{
		m_SkillShotTimeCount += Time.deltaTime;

		switch( m_Skill3Phase )
		{
			case E_SKILL3_PHASE.LASER_SHOT:
				if( m_SkillShotTimeCount >= m_Skill3WaitNextNormalLaserTime )
				{
					m_SkillShotTimeCount = 0;

					var shotParam = new BulletShotParam( this );
					shotParam.BulletIndex = m_Skill3NormalLaserIndex.BulletIndex;
					shotParam.BulletParamIndex = m_Skill3NormalLaserIndex.BulletParamIndex;
					shotParam.OrbitalIndex = m_Skill3NormalLaserIndex.OrbitalParamIndex;

					if( m_Skill3IsShotRight )
					{
						shotParam.Position = m_RightRailgunShotPosition.position - transform.parent.position;
					}
					else
					{
						shotParam.Position = m_LeftRailgunShotPosition.position - transform.parent.position;
					}

					float deltaAngle = m_Skill3NormalLaserAngleIndex * m_Skill3NormalLaserUnitAngle;
					shotParam.Rotation = new Vector3( 0, deltaAngle, 0 ) + transform.eulerAngles - transform.parent.eulerAngles;

					var bullet = BulletController.ShotBullet( shotParam );
					var normalLaser = bullet as InfC761Skill3NormalLaser;
					normalLaser.SetParam( m_Skill3NormalLaserParam );

					m_Skill3IsShotRight = !m_Skill3IsShotRight;
					m_Skill3NormalLaserAngleIndex *= -1;

					if( m_Skill3IsShotRight )
					{
						m_Skill3NormalLaserAngleIndex--;

						if( m_Skill3NormalLaserAngleIndex <= 0 )
						{
							m_Skill3Phase = E_SKILL3_PHASE.WAIT_CENTER_LASER_SHOT;
						}
					}
				}

				break;

			case E_SKILL3_PHASE.WAIT_CENTER_LASER_SHOT:
				if( m_SkillShotTimeCount >= m_Skill3WaitCenterLaserChargeTime )
				{
					m_SkillShotTimeCount = 0;
					m_Skill3Phase = E_SKILL3_PHASE.CENTER_LASER_SHOT;
					ShotSkill3CenterLaser();
					SpreadSkill3NormalLaserFireBullets();
				}

				break;

			case E_SKILL3_PHASE.CENTER_LASER_SHOT:
				if( m_SkillShotTimeCount >= m_Skill3CenterLaserShotTime )
				{
					m_SkillShotTimeCount = 0;
					m_Skill3Phase = E_SKILL3_PHASE.SPREAD_BULLETS;
				}

				break;

			case E_SKILL3_PHASE.SPREAD_BULLETS:
				if( m_SkillShotTimeCount >= m_Skill3WaitNextNormalLaserTime )
				{
					m_Skill3Phase = E_SKILL3_PHASE.LASER_SHOT;
					m_SkillShotTimeCount = 0;
					ChangeToSkill3();
				}

				break;
		}
	}

	private void ShotSkill3CenterLaser()
	{
		m_Skill3RightCenterLaser = OnShotSkill3CenterLaser( m_RightRailgunShotPosition );
		m_Skill3LeftCenterLaser = OnShotSkill3CenterLaser( m_LeftRailgunShotPosition );
		m_Skill3RightCenterLaser.SetParam( m_Skill3CenterLaserParam );
		m_Skill3LeftCenterLaser.SetParam( m_Skill3CenterLaserParam );
	}

	private InfC761Skill3CenterLaser OnShotSkill3CenterLaser( Transform shotPosition )
	{
		var shotParam = new BulletShotParam( this );
		shotParam.BulletIndex = m_Skill3CenterLaserIndex.BulletIndex;
		shotParam.BulletParamIndex = m_Skill3CenterLaserIndex.BulletParamIndex;
		shotParam.OrbitalIndex = m_Skill3CenterLaserIndex.OrbitalParamIndex;
		shotParam.Position = shotPosition.position - transform.parent.position;
		var laser = BulletController.ShotBullet( shotParam );
		return laser as InfC761Skill3CenterLaser;
	}

	public void AddSkill3NormalLaserFireBullets( List<BulletController> bullets )
	{
		if( bullets == null || bullets.Count < 1 )
		{
			return;
		}

		if( m_Skill3NormalLaserFireBullets == null )
		{
			m_Skill3NormalLaserFireBullets = new List<BulletController>();
		}

		m_Skill3NormalLaserFireBullets.AddRange( bullets );
	}

	private void SpreadSkill3NormalLaserFireBullets()
	{
		foreach( var bullet in m_Skill3NormalLaserFireBullets )
		{
			Vector3 dir = ( bullet.transform.position - transform.position ).normalized;
			bullet.transform.forward = dir;
			bullet.SetNowAccel( 20 );
		}
	}

	#endregion

	#region Change Phase Method

	private void ChangeToSkill3()
	{
		m_Skill3NormalLaserAngleIndex = m_Skill3NormalLaserShotNum;
		m_Skill3Phase = E_SKILL3_PHASE.LASER_SHOT;
		m_Skill3IsShotRight = true;

		ResetSkillTimeCount();
		DeceideSkill3TargetPos();
		m_IsStay = false;
	}

	#endregion

	public override void Dead()
	{
		base.Dead();

		BattleManager.Instance.GameClear();
	}
}

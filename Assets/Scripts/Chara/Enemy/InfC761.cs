using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ1のボスINF-C-761のコントローラ
/// </summary>
public class InfC761 : EnemyController
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

	[System.Serializable]
	private struct NormalMoveTargetPosition
	{
		public Vector3[] Pos;
	}

	[Header( "Normal Move Param" )]

	[SerializeField]
	private NormalMoveTargetPosition[] m_NormalMoveTargetPositions;

	[SerializeField]
	private float m_MinMoveInterval;

	[SerializeField]
	private float m_MaxMoveInterval;

	[SerializeField]
	private float m_NormalMoveLerp;

	[SerializeField]
	private float m_StayThreshold;

	[Header( "Normal1 Param" )]

	[SerializeField]
	private EnemyShotParam m_Normal1ShotParam;

	[SerializeField]
	private float m_DeltaRotation;

	[Header( "Normal2 Param" )]

	[SerializeField]
	private EnemyShotParam m_Normal2ShotParam;

	[Header( "Normal3 Param" )]

	[SerializeField]
	private EnemyShotParam m_Normal3ShotParam;

	[Header( "Shot Position" )]

	[SerializeField]
	private Transform m_RightRailgunShotPosition;

	[SerializeField]
	private Transform m_LeftRailgunShotPosition;

	[Space()]

	[SerializeField]
	private E_PHASE m_Phase;

	private float m_NormalMoveTimeCount;
	private float m_NormalMoveInterval;
	private int m_NormalMoveCurrentPosIndex;
	private int m_NormalMovePosDir;

	[SerializeField]
	private Vector3 m_FactNormalMoveTargetPos;

	private bool m_IsStay;
	private float m_NormalShotInterval;

	public override void SetStringParam( string param )
	{
		base.SetStringParam( param );
	}

	public override void OnStart()
	{
		base.OnStart();

		ResetNormalTimeCount();
		// 最初の行先の決定
		DecideStartNormalTargetPos();
		m_IsStay = false;
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
		m_NormalMoveInterval = Random.Range( m_MinMoveInterval, m_MaxMoveInterval );
	}

	private void DecideStartNormalTargetPos()
	{
		int groupNum = m_NormalMoveTargetPositions.Length;
		m_NormalMoveCurrentPosIndex = Random.Range( 0, groupNum );
		var targetPositions = m_NormalMoveTargetPositions[m_NormalMoveCurrentPosIndex];
		int posNum = targetPositions.Pos.Length;
		var viewPos = targetPositions.Pos[Random.Range( 0, posNum )];

		m_FactNormalMoveTargetPos = CameraManager.Instance.GetViewportWorldPoint( viewPos.x, viewPos.z ) - transform.parent.position;

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

		m_FactNormalMoveTargetPos = CameraManager.Instance.GetViewportWorldPoint( viewPos.x, viewPos.z ) - transform.parent.position;

		if( m_NormalMoveCurrentPosIndex < 1 )
		{
			m_NormalMovePosDir = 1;
		}
		else if( m_NormalMoveCurrentPosIndex >= groupNum - 1 )
		{
			m_NormalMovePosDir = -1;
		}
	}

	private void Move()
	{
		switch( m_Phase )
		{
			case E_PHASE.NORMAL1:
			case E_PHASE.NORMAL2:
			case E_PHASE.NORMAL3:

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

				var target = PlayerCharaManager.Instance.GetCurrentController().transform;
				transform.LookAt( target );
				break;
		}
	}

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
		shotParam.BulletIndex = 0;
		shotParam.BulletParamIndex = 0;
		shotParam.Position = shotPosition.position - transform.parent.position;

		float deltaRotation = ( isLeft ? -1 : 1 ) * m_DeltaRotation;

		for( int i = 0; i < 3; i++ )
		{
			shotParam.OrbitalIndex = i;
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, deltaRotation, 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );
		}
	}



	private void ShotNormal2()
	{
		m_NormalShotInterval += Time.deltaTime;

		if( m_NormalShotInterval >= m_Normal2ShotParam.Interval )
		{
			m_NormalShotInterval = 0;

			var shotParam = new BulletShotParam( this );
			shotParam.BulletIndex = 1;
			shotParam.BulletParamIndex = 1;
			shotParam.OrbitalIndex = -1;

			// 後にパラメータ化
			int num = m_Normal2ShotParam.Num;
			float deltaAngle = Mathf.PI * 2 / num;

			for( int i = 0; i < num; i++ )
			{
				float angle = deltaAngle * i;

				// z軸が0度とするために、パラメータの使い方が数学的に逆
				var dir = new Vector3( Mathf.Sin( angle ), 0, Mathf.Cos( angle ) );
				var bullet = BulletController.ShotBullet( shotParam );

				bullet.SetRotation( new Vector3( 0, angle * Mathf.Rad2Deg, 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );
				bullet.SetPosition( dir * 5f, E_ATTACK_PARAM_RELATIVE.RELATIVE );
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
		shotParam.BulletIndex = 2;
		shotParam.BulletParamIndex = 2;
		shotParam.Position = shotPosition.position - transform.parent.position;

		for( int i = 0; i < 3; i++ )
		{
			shotParam.OrbitalIndex = i;

			for( int j = 0; j < num; j++ )
			{
				var bullet = BulletController.ShotBullet( shotParam );
				bullet.SetRotation( new Vector3( 0, spreadAngles[j], 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );
			}
		}
	}



	public override void Dead()
	{
		base.Dead();

		BattleManager.Instance.GameClear();
	}
}

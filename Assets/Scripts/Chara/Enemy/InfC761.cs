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

	[Header( "Normal1 Param" )]

	[SerializeField]
	private EnemyShotParam m_Normal1ShotParam;

	[Header( "Normal2 Param" )]

	[SerializeField]
	private EnemyShotParam m_Normal2ShotParam;

	[Header( "Normal3 Param" )]

	[SerializeField]
	private EnemyShotParam m_Normal3ShotParam;




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
				m_NormalMoveTimeCount += Time.deltaTime;

				if( m_NormalMoveTimeCount >= m_NormalMoveInterval )
				{
					ResetNormalTimeCount();
					DecideNormalTargetPos();
				}


				Vector3 pos = Vector3.Lerp( transform.localPosition, m_FactNormalMoveTargetPos, m_NormalMoveLerp );
				transform.localPosition = pos;

				m_IsStay = ( m_FactNormalMoveTargetPos - pos ).sqrMagnitude <= 0.01f;
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

				}

				break;

			case E_PHASE.NORMAL2:
				if( m_IsStay )
				{

				}

				break;

			case E_PHASE.NORMAL3:
				if( m_IsStay )
				{

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
		}
	}

	private void ShotNormal2()
	{
		m_NormalShotInterval += Time.deltaTime;

		if( m_NormalShotInterval >= m_Normal2ShotParam.Interval )
		{
			m_NormalShotInterval = 0;
		}
	}

	private void ShotNormal3()
	{
		m_NormalShotInterval += Time.deltaTime;

		if( m_NormalShotInterval >= m_Normal3ShotParam.Interval )
		{
			m_NormalShotInterval = 0;
		}
	}

	public override void Dead()
	{
		base.Dead();

		BattleManager.Instance.GameClear();
	}
}

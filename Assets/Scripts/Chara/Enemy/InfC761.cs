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

	[Header( "Normal Move Param" )]

	[SerializeField]
	private Vector3[][] m_NormalMoveTargetPositions;

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
	private EnemyShotParam m_;




	private E_PHASE m_Phase;

	private float m_NormalMoveTimeCount;
	private float m_NormalMoveInterval;
	private int m_NormalMoveCurrentPosIndex;
	private int m_NormalMovePosDir;
	private Vector3 m_FactNormalMoveTargetPos;


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
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
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
		int posNum = targetPositions.Length;
		m_FactNormalMoveTargetPos = targetPositions[Random.Range( 0, posNum )];

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
		int posNum = targetPositions.Length;
		m_FactNormalMoveTargetPos = targetPositions[Random.Range( 0, posNum )];

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
				break;
		}
	}
}

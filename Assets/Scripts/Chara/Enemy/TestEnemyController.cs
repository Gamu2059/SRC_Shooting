using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 画面上部から下がって来て数秒後に離脱する基本的な敵のコントローラ。
/// </summary>
public class TestEnemyController : EnemyController
{
	[Space()]
	[Header( "TestEnemyController Parameter" )]

	[SerializeField]
	private Transform m_ShotPosition;

	/// <summary>
	/// 待機座標
	/// 出現初期位置に対する相対座標
	/// </summary>
	[SerializeField]
	private Vector3 m_MoveTargetLocalPosition;

	/// <summary>
	/// 待機座標に行くまでに掛かる時間
	/// </summary>
	[SerializeField]
	private float m_MoveTargetDuration;

	/// <summary>
	/// 離脱までの待機時間
	/// </summary>
	[SerializeField]
	private float m_WaitDuration;

	/// <summary>
	/// 離脱時のWorldX軸速度
	/// </summary>
	[SerializeField]
	private AnimationCurve m_WithdrawalXSpeed;

	/// <summary>
	/// 離脱時のWorldZ軸速度
	/// </summary>
	[SerializeField]
	private AnimationCurve m_WithdrawalZSpeed;

	/// <summary>
	/// 離脱時のLocalY軸回転
	/// </summary>
	[SerializeField]
	private AnimationCurve m_WithdrawalYRotation;

	/// <summary>
	/// 離脱時のLocalZ軸回転
	/// </summary>
	[SerializeField]
	private AnimationCurve m_WithdrawalZRotation;

	[SerializeField]
	private float m_WithdrawalDuration;

	/// <summary>
	/// 待機時の弾を撃つ間隔
	/// </summary>
	[SerializeField]
	private float m_BulletShotInterval;

	/// <summary>
	/// 同時に撃つ弾の数
	/// </summary>
	[SerializeField]
	private int m_BulletShotNum;

	/// <summary>
	/// 弾の拡散角度(degree)
	/// </summary>
	[SerializeField]
	private float m_BulletSpreadAngle;

	/// <summary>
	/// 弾の基準方向をプレイヤーの方向に向けるか
	/// </summary>
	[SerializeField]
	private bool m_IsSearchBullet;

	[SerializeField]
	private int m_MoveStatus;
	private Vector3 m_TargetLocalPosition;
	private float m_WithdrawalCounter;

	protected override void OnAwake()
	{
		base.OnAwake();

		m_MoveStatus = 0;
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
	}

	public override void OnFinalize()
	{
		base.OnFinalize();
	}

	public override void OnStart()
	{
		base.OnStart();
	}

	public override void OnUpdate()
	{
		if( m_MoveStatus == 0 )
		{
			// 行先の計算
			var pos = new Vector3( m_MoveTargetLocalPosition.x * transform.forward.x, 0, m_MoveTargetLocalPosition.z * transform.forward.z );
			m_TargetLocalPosition = pos + transform.localPosition;
			m_MoveStatus = 1;
		}
		else if( m_MoveStatus == 1 )
		{
			// 行先へ移動
			m_MoveStatus = 2;
			transform.DOLocalMove( m_TargetLocalPosition, m_MoveTargetDuration ).OnComplete( () => m_MoveStatus = 3 );
		}
		else if( m_MoveStatus == 3 )
		{
			m_MoveStatus = 4;
			var timer = Timer.CreateTimer( Timer.E_TIMER_TYPE.SCALED_TIMER, m_BulletShotInterval, m_WaitDuration );
			timer.SetIntervalCallBack( () =>
			{
				ShotBullet();
			} ).SetTimeoutCallBack( () =>
			{
				m_MoveStatus = 5;
			} ).SetStopCallBack( () =>
			{
				m_MoveStatus = 5;
			} );
			TimerManager.Instance.RegistTimter( timer );
		}
		else if( m_MoveStatus == 5 )
		{
			m_WithdrawalCounter = 0;
			m_MoveStatus = 6;
		}
		else if( m_MoveStatus == 6 )
		{
			var xPos = m_WithdrawalXSpeed.Evaluate( m_WithdrawalCounter );
			var zPos = m_WithdrawalZSpeed.Evaluate( m_WithdrawalCounter );
			var yRot = m_WithdrawalYRotation.Evaluate( m_WithdrawalCounter );
			var zRot = m_WithdrawalZRotation.Evaluate( m_WithdrawalCounter );

			transform.position += new Vector3( xPos, 0, zPos );
			transform.localEulerAngles += new Vector3( 0, yRot, zRot );
			m_WithdrawalCounter += Time.deltaTime;

			if( m_WithdrawalCounter >= m_WithdrawalDuration )
			{
				m_MoveStatus = 7;
			}
		}
		else if( m_MoveStatus == 7 )
		{
			EnemyCharaManager.Instance.DestroyEnemy( this );
		}
	}

	private void ShotBullet()
	{
		if( m_ShotPosition == null )
		{
			return;
		}

		var spreadAngles = GetSpreadAngles( m_BulletShotNum, m_BulletSpreadAngle );
		var shotParam = new BulletShotParam( this );
		shotParam.Position = m_ShotPosition.GetMoveObjectHolderBasePosition();

		for( int i = 0; i < m_BulletShotNum; i++ )
		{
			var bullet = BulletController.ShotBullet( shotParam );

			if( m_IsSearchBullet )
			{
				bullet.LookAtTarget();
			}

			bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_ATTACK_PARAM_RELATIVE.RELATIVE );
		}
	}

	private List<float> GetSpreadAngles( int bulletNum, float spreadAngle )
	{
		List<float> spreadAngles = new List<float>();

		if( bulletNum % 2 == 1 )
		{
			spreadAngles.Add( 0f );

			for( int i = 0; i < ( bulletNum - 1 ) / 2; i++ )
			{
				spreadAngles.Add( spreadAngle * ( i + 1f ) );
				spreadAngles.Add( spreadAngle * -( i + 1f ) );
			}
		}
		else
		{
			for( int i = 0; i < bulletNum / 2; i++ )
			{
				spreadAngles.Add( spreadAngle * ( i + 0.5f ) );
				spreadAngles.Add( spreadAngle * -( i + 0.5f ) );
			}
		}

		return spreadAngles;
	}

	public override void OnSuffer( BulletController bullet, ColliderData colliderData )
	{
		base.OnSuffer( bullet, colliderData );
		EnemyCharaManager.Instance.DestroyEnemy( this );
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Skill3CenterLaser : BulletController
{
	private enum E_PHASE
	{
		BIGGER,
		SMALLER,
	}

	/// <summary>
	/// INF-C-761のスキル3の中心レーザーパラメータ
	/// </summary>
	[System.Serializable]
	public struct ConstParam
	{
		// 弾を生成する個数
		public int m_BulletCreateNum;

		// 弾の拡散加速度
		public float m_BulletAccel;

		// 拡散する弾のインデックス
		public int m_BulletIndex;

		// 拡散する弾の弾パラメータインデックス
		public int m_BulletParamIndex;

		// 拡散する弾の軌道パラメータのインデックス
		public int m_OrbitalIndex;
	}



	private ConstParam m_Param;

	private E_PHASE m_Phase;

	private List<BulletController> m_Bullets;


	public void SetParam( ConstParam param )
	{
		m_Param = param;
	}

	public override void OnStart()
	{
		base.OnStart();

		m_Phase = E_PHASE.BIGGER;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		switch( m_Phase )
		{
			case E_PHASE.BIGGER:
				OnBiggerUpdate();
				break;

			case E_PHASE.SMALLER:
				OnSmallerUpdate();
				break;
		}
	}

	private void OnBiggerUpdate()
	{
		var scale = GetScale();
		scale.x += 3 * Time.deltaTime;

		SetScale( scale );

		if( scale.x >= 2f )
		{
			CreateBullet();
			m_Phase = E_PHASE.SMALLER;
		}
	}

	private void OnSmallerUpdate()
	{
		var scale = GetScale();
		scale.x -= 3 * Time.deltaTime;

		if( scale.x < 0f )
		{
			scale.x = 0f;
		}

		SetScale( scale );

		if( scale.x <= 0 )
		{
			SpreadBullet();
			DestroyBullet();
		}
	}

	private void CreateBullet()
	{
		var infC761 = GetBulletOwner() as InfC761;

		if( infC761 == null )
		{
			return;
		}

		var shotParam = new BulletShotParam( GetBulletOwner(), m_Param.m_BulletIndex, m_Param.m_BulletParamIndex, m_Param.m_OrbitalIndex );

		Vector3 right = transform.right;
		right.y = 0;

		Vector3 forward = transform.forward;
		forward.y = 0;

		Vector3 scale = GetScale();

		m_Bullets = new List<BulletController>();

		for( int i = 0; i < m_Param.m_BulletCreateNum; i++ )
		{
			float xPos = Random.Range( -scale.x / 2f, scale.x / 2f );
			float zPos = Random.Range( 0, scale.z );

			shotParam.Position = xPos * right + zPos * forward + GetPosition();
			shotParam.Rotation = new Vector3( 0, Random.Range( 0f, 360f ), 0 );
			m_Bullets.Add( ShotBullet( shotParam ) );
		}
	}

	private void SpreadBullet()
	{
		foreach( var bullet in m_Bullets )
		{
			bullet.SetNowAccel( m_Param.m_BulletAccel );
		}
	}
}

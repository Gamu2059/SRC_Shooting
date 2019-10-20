using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Skill3NormalLaser : BulletController
{
	private enum E_PHASE
	{
		BIGGER,
		SMALLER,
	}

	/// <summary>
	/// INF-C-761のスキル3の通常レーザーパラメータ
	/// </summary>
	[System.Serializable]
	public struct ConstParam
	{

		// 弾を生成する個数
		public int m_BulletCreateNum;

		// 弾を生成する単位の距離間隔
		public float m_BulletCreateUnitDistance;

		// 拡散する弾のインデックス
		public int m_BulletIndex;

		// 拡散する弾の弾パラメータインデックス
		public int m_BulletParamIndex;

		// 拡散する弾の軌道パラメータのインデックス
		public int m_OrbitalIndex;
	}



	private ConstParam m_Param;

	private E_PHASE m_Phase;



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
		scale.x += 5 * Time.deltaTime;

		SetScale( scale );

		if( scale.x >= 1f )
		{
			CreateBullet();
			m_Phase = E_PHASE.SMALLER;
		}
	}

	private void OnSmallerUpdate()
	{
		var scale = GetScale();
		scale.x -= 5 * Time.deltaTime;

		if( scale.x < 0.01f )
		{
			scale.x = 0.01f;
		}

		SetScale( scale );

		if( scale.x <= 0.01f )
		{
			DestroyBullet();
		}
	}

	private void CreateBullet()
	{
		var infC761 = GetBulletOwner() as InfC761_;

		if( infC761 == null )
		{
			return;
		}

		Debug.Log( 111 );
		var shotParam = new BulletShotParam( GetBulletOwner(), m_Param.m_BulletIndex, m_Param.m_BulletParamIndex, m_Param.m_OrbitalIndex );
		shotParam.Rotation = GetRotation();

		Vector3 dir = transform.forward;
		dir.y = 0;

		List<BulletController> bullets = new List<BulletController>();

		Debug.Log( 222 );

		for( int i = 0; i < m_Param.m_BulletCreateNum; i++ )
		{
			Debug.Log( i );
			shotParam.Position = ( i + 1 ) * m_Param.m_BulletCreateUnitDistance * dir + GetPosition();
			bullets.Add( ShotBullet( shotParam ) );
		}

		infC761.AddSkill3NormalLaserFireBullets( bullets );
	}
}

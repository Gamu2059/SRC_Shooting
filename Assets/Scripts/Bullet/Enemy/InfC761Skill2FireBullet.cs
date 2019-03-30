using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Skill2FireBullet : BulletController
{
	/// <summary>
	/// INF-C-761のスキル2の起爆弾パラメータ
	/// </summary>
	[System.Serializable]
	public struct ConstParam
	{

		// 拡散する弾のインデックス
		public int m_BulletIndex;

		// 拡散する弾の弾パラメータインデックス
		public int m_BulletParamIndex;

		// 拡散する弾の軌道パラメータのインデックス
		public int m_OrbitalIndex;

		// 拡散弾の生成個数
		public int m_BulletNum;
	}



	// この弾のパラメータ
	private ConstParam m_Param;



	public void SetParam( ConstParam param )
	{
		m_Param = param;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if( GetNowSpeed() < 0 )
		{
			SetNowSpeed( 0 );
		}
	}

	/// <summary>
	/// 拡散弾を発生させる。
	/// </summary>
	public void FireSpreadBullet()
	{
		float unitAngle = 360f / m_Param.m_BulletNum;

		var shotParam = new BulletShotParam( GetBulletOwner(), m_Param.m_BulletIndex, m_Param.m_BulletParamIndex, m_Param.m_OrbitalIndex );
		shotParam.Position = GetPosition();

		for( int i = 0; i < m_Param.m_BulletNum; i++ )
		{
			shotParam.Rotation = GetRotation() + new Vector3( 0, unitAngle * i, 0 );
			ShotBullet( shotParam );
		}

		DestroyBullet();
	}

	protected override void OnBecameInvisible()
	{
		// 通常は場外に行ったら破棄するが、この弾は場外に行っても残るようにするため何もしない
	}
}

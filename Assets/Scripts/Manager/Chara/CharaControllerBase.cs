using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーキャラの制御を行うコンポーネント。
/// </summary>
public class CharaControllerBase : BehaviorBase
{

	[Header( "" )]

	[SerializeField]
	private BehaviorBase m_BulletController;

	[SerializeField]
	private float m_MoveSpeed = 5f;

	[Header( "プロテクタのパラメータ" )]

	[SerializeField]
	private Transform[] m_Protectors;

	[SerializeField]
	private float m_ProtectorRadius;

	[SerializeField]
	private float m_ProtectorSpeed;



	private float m_Rad;



	public override void OnUpdate()
	{
		m_Rad += m_ProtectorSpeed * Time.deltaTime;
		m_Rad %= Mathf.PI * 2;
		float unitAngle = Mathf.PI * 2 / m_Protectors.Length;

		for( int i = 0; i < m_Protectors.Length; i++ )
		{
			float angle = unitAngle * i + m_Rad;
			float x = m_ProtectorRadius * Mathf.Cos( angle );
			float z = m_ProtectorRadius * Mathf.Sin( angle );
			m_Protectors[i].localPosition = new Vector3( x, 0, z );
			m_Protectors[i].LookAt( transform );
		}
	}

	/// <summary>
	/// キャラを移動させる。
	/// 移動速度はキャラに現在設定されているものとなる。
	/// </summary>
	/// <param name="moveDirection"> 移動方向 </param>
	public void Move( Vector3 moveDirection )
	{
		Vector3 move = moveDirection.normalized * m_MoveSpeed * Time.deltaTime;
		transform.Translate( move );
	}

	public void Rotate()
	{

	}

	/// <summary>
	/// 通常弾を発射する。
	/// </summary>
	public virtual void ShotBullet()
	{

	}

	/// <summary>
	/// ボムを使用する。
	/// </summary>
	public virtual void ShotBomb()
	{

	}
}

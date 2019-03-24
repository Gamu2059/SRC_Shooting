using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharaController
{
	/// <summary>
	/// プレイヤーキャラのライフサイクル
	/// </summary>
	[System.Serializable]
	public enum E_PLAYER_LIFE_CYCLE
	{
		/// <summary>
		/// 戦闘画面には出ていない
		/// </summary>
		AHEAD,

		/// <summary>
		/// 現在戦闘中
		/// </summary>
		SORTIE,

		/// <summary>
		/// 死亡により戦闘画面から退場
		/// </summary>
		DEAD,
	}

	#region Field Inspector

	[Space()]
	[Header( "プレイヤーキャラ専用 基本ステータス" )]

	[SerializeField, Tooltip( "キャラのレベル" )]
	private int m_Lv;

	[SerializeField, Tooltip( "レベル上昇に必要な経験値" )]
	private int m_Exp;

	[SerializeField, Tooltip( "キャラの移動速度" )]
	private float m_MoveSpeed = 5f;

	[SerializeField, Tooltip( "キャラのライフサイクル" )]
	private E_PLAYER_LIFE_CYCLE m_LifeCycle;

	[Space()]
	[Header( "プレイヤーキャラ専用 プロテクタのパラメータ" )]

	[SerializeField, Tooltip( "プロテクタのトランスフォーム配列" )]
	protected Transform[] m_Protectors;

	[SerializeField, Tooltip( "プロテクタの回転半径" )]
	protected float m_ProtectorRadius;

	[SerializeField, Tooltip( "プロテクタの回転速度" )]
	protected float m_ProtectorSpeed;

	#endregion

	#region Field

	/// <summary>
	/// プロテクタの回転の基準角度
	/// </summary>
	protected float m_ProtectorRad;

	#endregion

	#region Getter & Setter

	public int GetLv()
	{
		return m_Lv;
	}

	public void SetLv( int lv )
	{
		m_Lv = lv;
	}

	public void AddLv( int lv )
	{
		m_Lv += lv;
	}

	public int GetExp()
	{
		return m_Exp;
	}

	public void SetExp( int exp )
	{
		m_Exp = exp;
	}

	public void AddExp( int exp )
	{
		m_Exp += exp;
	}

	public float GetMoveSpeed()
	{
		return m_MoveSpeed;
	}

	public void SetMoveSpeed( float moveSpeed )
	{
		m_MoveSpeed = moveSpeed;
	}

	public E_PLAYER_LIFE_CYCLE GetLifeCycle()
	{
		return m_LifeCycle;
	}

	public void SetLifeCycle( E_PLAYER_LIFE_CYCLE lifeCycle )
	{
		m_LifeCycle = lifeCycle;
	}

	#endregion

	private void Start()
	{
		// 開発時専用で、自動的にマネージャにキャラを追加するためにUnityのStartを用いています
		PlayerCharaManager.Instance.RegistChara( this );
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
	}

	public override void OnStart()
	{
		base.OnStart();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		UpdateProtector();
	}

	public override void OnLateUpdate()
	{
		base.OnLateUpdate();
	}

	/// <summary>
	/// 通常弾を発射する。
	/// このメソッドをオーバーロードしてそれぞれのキャラ固有の処理を記述して下さい。
	/// </summary>
	public virtual void ShotBullet()
	{
		// 何もオーバーロードしない場合は適当に弾を飛ばす
		BulletController.ShotBullet( this );
	}

	/// <summary>
	/// ボムを使用する。
	/// </summary>
	public virtual void ShotBomb()
	{

	}



	/// <summary>
	/// キャラを移動させる。
	/// 移動速度はキャラに現在設定されているものとなる。
	/// </summary>
	/// <param name="moveDirection"> 移動方向 </param>
	public virtual void Move( Vector3 moveDirection )
	{
		Vector3 move = moveDirection.normalized * m_MoveSpeed * Time.deltaTime;
		transform.Translate( move, Space.World );
	}


	public override void OnSuffer( BulletController bullet, ColliderData colliderData )
	{
		base.OnSuffer( bullet, colliderData );
	}

	public int GetLevel()
	{
		return m_Lv;
	}

	protected virtual void UpdateProtector()
	{
		m_ProtectorRad += m_ProtectorSpeed * Time.deltaTime;
		m_ProtectorRad %= Mathf.PI * 2;
		float unitAngle = Mathf.PI * 2 / m_Protectors.Length;

		for( int i = 0; i < m_Protectors.Length; i++ )
		{
			float angle = unitAngle * i + m_ProtectorRad;
			float x = m_ProtectorRadius * Mathf.Cos( angle );
			float z = m_ProtectorRadius * Mathf.Sin( angle );
			m_Protectors[i].localPosition = new Vector3( x, 0, z );
			m_Protectors[i].LookAt( transform );
		}
	}

	public override void Dead()
	{
		base.Dead();

		gameObject.SetActive( false );
		BattleManager.Instance.GameOver();
	}
}

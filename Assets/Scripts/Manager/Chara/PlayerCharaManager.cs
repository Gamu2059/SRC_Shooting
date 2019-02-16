using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーキャラの動作を制御するマネージャ。
/// とりあえずで作ってます。
/// </summary>
public class PlayerCharaManager : GlobalSingletonMonoBehavior<PlayerCharaManager>
{

	#region Inspector

	[Header( "Key config" )]

	[SerializeField]
	private KeyCode[] m_ForwardMove;

	[SerializeField]
	private KeyCode[] m_BackMove;

	[SerializeField]
	private KeyCode[] m_RightMove;

	[SerializeField]
	private KeyCode[] m_LeftMove;

	[SerializeField]
	private KeyCode[] m_ShotBullet;

	[SerializeField]
	private KeyCode[] m_ShotBomb;

	[SerializeField]
	private KeyCode[] m_1stCharaChange;
	[SerializeField]
	private KeyCode[] m_2ndCharaChange;
	[SerializeField]
	private KeyCode[] m_3rdCharaChange;


	[SerializeField]
	private int m_Smasher;

	[SerializeField]
	private int m_Echo;

	[SerializeField]
	private int m_Hacker;

	[SerializeField]
	private CharaControllerBase m_CharaController;

	#endregion



	// 現在出撃中のキャラのインデックス
	private int m_CharaIndex = 0;



	public override void Init()
	{
		base.Init();
	}

	protected override void OnAwake()
	{
		base.OnAwake();

		m_CharaController.OnAwake();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		m_CharaController.OnDestroyed();
	}

	private void Start()
	{
		m_CharaController.OnStart();
	}

	private void Update()
	{
		if( m_CharaController == null )
		{
			return;
		}

		Vector3 moveDir = Vector3.zero;

		// 移動関係の入力
		if( IsGetKey( m_ForwardMove ) )
		{
			moveDir.z += 1;
		}

		if( IsGetKey( m_BackMove ) )
		{
			moveDir.z -= 1;
		}

		if( IsGetKey( m_RightMove ) )
		{
			moveDir.x += 1;
		}

		if( IsGetKey( m_LeftMove ) )
		{
			moveDir.x -= 1;
		}

		m_CharaController.Move( moveDir );

		// 通常弾
		if( IsGetKey( m_ShotBullet, Input.GetMouseButton( 0 ) ) )
		{
			m_CharaController.ShotBullet();
		}

		// ボム
		if( IsGetKeyDown( m_ShotBomb, Input.GetMouseButtonDown( 1 ) ) )
		{
			m_CharaController.ShotBomb();
		}

		// 上にスクロールするとプラス、下でマイナス
		float wheel = Input.GetAxis( "Mouse ScrollWheel" );

		// キャラ交代の入力
		if( IsGetKey( m_1stCharaChange, m_CharaIndex > 0 && wheel > 0 ) )
		{
			ChangeChara( 0 );
		}
		else if( IsGetKey( m_2ndCharaChange, m_CharaIndex < 1 && wheel < 0 || m_CharaIndex > 1 && wheel > 0 ) )
		{
			ChangeChara( 1 );
		}
		else if( IsGetKey( m_3rdCharaChange, m_CharaIndex < 2 && wheel < 0 ) )
		{
			ChangeChara( 2 );
		}

		m_CharaController.OnUpdate();
	}

	private void LateUpdate()
	{
		m_CharaController.OnLateUpdate();
	}

	private bool IsGetKeyDown( KeyCode[] targetKeys, bool additionalCondition = false )
	{
		foreach( var key in targetKeys )
		{
			if( Input.GetKeyDown( key ) )
			{
				return true;
			}
		}

		return additionalCondition;
	}

	private bool IsGetKey( KeyCode[] targetKeys, bool additionalCondition = false )
	{
		foreach( var key in targetKeys )
		{
			if( Input.GetKey( key ) )
			{
				return true;
			}
		}

		return additionalCondition;
	}

	private void ChangeChara( int index )
	{
		if( m_CharaIndex == index )
		{
			return;
		}

		m_CharaIndex = index;

		Debug.Log( "Change Chara : " + m_CharaIndex );
	}
}

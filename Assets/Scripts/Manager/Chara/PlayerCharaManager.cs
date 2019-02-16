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
	private CharaControllerBase[] m_CharaControllers;

	[SerializeField]
	private CharaControllerBase m_CurrentCharaController;

	#endregion



	// 現在出撃中のキャラのインデックス
	private int m_CharaIndex = 0;

	private float m_WaitChangeTime = 0;


	public override void Init()
	{
		base.Init();
	}

	protected override void OnAwake()
	{
		base.OnAwake();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void Start()
	{
		foreach( var chara in m_CharaControllers )
		{
			chara.gameObject.SetActive( false );
		}

		ChangeChara( 0 );
	}

	private void Update()
	{
		if( m_CurrentCharaController == null )
		{
			return;
		}

		if( m_WaitChangeTime > 0f )
		{
			m_WaitChangeTime -= Time.deltaTime;
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

		m_CurrentCharaController.Move( moveDir );

		// 通常弾
		if( IsGetKey( m_ShotBullet, Input.GetMouseButton( 0 ) ) )
		{
			m_CurrentCharaController.ShotBullet();
		}

		// ボム
		if( IsGetKeyDown( m_ShotBomb, Input.GetMouseButtonDown( 1 ) ) )
		{
			m_CurrentCharaController.ShotBomb();
		}

		// 上にスクロールするとプラス、下でマイナス
		float wheel = Input.GetAxis( "Mouse ScrollWheel" );

		// キャラ交代の入力
		int charaNum = m_CharaControllers.Length;

		if( wheel > 0 )
		{
			ChangeChara( ( m_CharaIndex - 1 + charaNum ) % charaNum );
		}
		else if( wheel < 0 )
		{
			ChangeChara( ( m_CharaIndex + 1 + charaNum ) % charaNum );
		}

		if( IsGetKey( m_1stCharaChange ) )
		{
			ChangeChara( 0 );
		}
		else if( IsGetKey( m_2ndCharaChange ) )
		{
			ChangeChara( 1 );
		}
		else if( IsGetKey( m_3rdCharaChange ) )
		{
			ChangeChara( 2 );
		}

		m_CurrentCharaController.OnUpdate();
	}

	private void LateUpdate()
	{
		m_CurrentCharaController.OnLateUpdate();
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
		if( m_CharaIndex == index && m_CurrentCharaController != null || m_WaitChangeTime > 0f )
		{
			return;
		}

		m_CharaIndex = index;

		if( m_CurrentCharaController != null )
		{
			m_CurrentCharaController.gameObject.SetActive( false );
		}

		m_CurrentCharaController = m_CharaControllers[m_CharaIndex];
		m_CurrentCharaController.gameObject.SetActive( true );
		m_WaitChangeTime = 1f;
	}
}

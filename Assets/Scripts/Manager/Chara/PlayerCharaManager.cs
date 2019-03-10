using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーキャラの動作を制御するマネージャ。
/// とりあえずで作ってます。
/// </summary>
public class PlayerCharaManager : SingletonMonoBehavior<PlayerCharaManager>
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
	private List<PlayerController> m_Controllers;

	[SerializeField]
	private PlayerController m_CurrentController;

	#endregion



	// 現在出撃中のキャラのインデックス
	private int m_CharaIndex = 0;

	private float m_WaitChangeTime = 0;


	#region Get Set

	public List<PlayerController> GetControllers()
	{
		return m_Controllers;
	}

	public PlayerController GetCurrentController()
	{
		return m_CurrentController;
	}

	#endregion



	protected override void OnAwake()
	{
		base.OnAwake();
		m_Controllers = new List<PlayerController>();
	}

	protected override void OnDestroyed()
	{
		base.OnDestroyed();
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
	}

	public override void OnFinalize()
	{
		base.OnFinalize();
		m_Controllers.Clear();
	}

	//private void Start()
	//{
	//	foreach( var chara in m_Controllers )
	//	{
	//		chara.gameObject.SetActive( false );
	//	}

	//	ChangeChara( 0 );
	//}

	public override void OnUpdate()
	{
		if( m_CurrentController == null )
		{
			ChangeChara( 0 );
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

		m_CurrentController.Move( moveDir );

		// 通常弾
		if( IsGetKey( m_ShotBullet, Input.GetMouseButton( 0 ) ) )
		{
			m_CurrentController.ShotBullet();
		}

		// ボム
		if( IsGetKeyDown( m_ShotBomb, Input.GetMouseButtonDown( 1 ) ) )
		{
			m_CurrentController.ShotBomb();
		}

		// 上にスクロールするとプラス、下でマイナス
		float wheel = Input.GetAxis( "Mouse ScrollWheel" );

		// キャラ交代の入力
		int charaNum = m_Controllers.Count;

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

		m_CurrentController.OnUpdate();
	}

	public override void OnLateUpdate()
	{
		if( m_CurrentController == null )
		{
			return;
		}

		m_CurrentController.OnLateUpdate();
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
		if( m_CharaIndex == index && m_CurrentController != null || m_WaitChangeTime > 0f )
		{
			return;
		}

		if( index < 0 || index >= m_Controllers.Count )
		{
			return;
		}

		m_CharaIndex = index;

		if( m_CurrentController != null )
		{
			m_CurrentController.gameObject.SetActive( false );
		}

		m_CurrentController = m_Controllers[m_CharaIndex];
		m_CurrentController.gameObject.SetActive( true );
		m_WaitChangeTime = 1f;
	}

	public void RegistChara( PlayerController controller )
	{
		if( controller == null || m_Controllers.Contains( controller ) )
		{
			return;
		}

		m_Controllers.Add( controller );

		// 最初のキャラだけONにする
		if( m_Controllers.Count > 1 )
		{
			controller.gameObject.SetActive( false );
		}
	}
}

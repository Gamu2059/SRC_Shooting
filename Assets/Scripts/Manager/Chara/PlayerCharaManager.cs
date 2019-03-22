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

	[Header( "Holder" )]

	[SerializeField]
	private Transform m_PlayerCharaHolder;

	[Header( "Chara" )]

	[SerializeField]
	private PlayerController[] m_CharaPrefabs;

	[SerializeField]
	private Vector2 m_InitAppearViewportPosition;

	[Header( "Key config" )]

	[SerializeField]
	private InputParam m_InputParam;

	[Header( "Restrict Filed" )]

	[SerializeField]
	private Vector2 m_MinViewportRestrict;

	[SerializeField]
	private Vector2 m_MaxViewportRestrict;

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

	public override void OnStart()
	{
		base.OnStart();

		if( StageManager.Instance != null && StageManager.Instance.GetPlayerCharaHolder() != null )
		{
			m_PlayerCharaHolder = StageManager.Instance.GetPlayerCharaHolder().transform;
		}
		else if( m_PlayerCharaHolder == null )
		{
			var obj = new GameObject( "[PlayerCharaHolder]" );
			obj.transform.position = Vector3.zero;
			m_PlayerCharaHolder = obj.transform;
		}

		foreach( var charaPrefab in m_CharaPrefabs )
		{
			var chara = Instantiate( charaPrefab );
			RegistChara( chara );
			var pos = CameraManager.Instance.GetViewportWorldPoint( m_InitAppearViewportPosition.x, m_InitAppearViewportPosition.y );
			chara.transform.position = pos;
		}
	}

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
		if( IsGetKey( m_InputParam.GetForwardMove() ) )
		{
			moveDir.z += 1;
		}

		if( IsGetKey( m_InputParam.GetBackMove() ) )
		{
			moveDir.z -= 1;
		}

		if( IsGetKey( m_InputParam.GetRightMove() ) )
		{
			moveDir.x += 1;
		}

		if( IsGetKey( m_InputParam.GetLeftMove() ) )
		{
			moveDir.x -= 1;
		}

		m_CurrentController.Move( moveDir );

		// 通常弾
		if( IsGetKey( m_InputParam.GetShotBullet(), Input.GetMouseButton( 0 ) ) )
		{
			m_CurrentController.ShotBullet();
		}

		// ボム
		if( IsGetKeyDown( m_InputParam.GetShotBomb(), Input.GetMouseButtonDown( 1 ) ) )
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

		if( IsGetKey( m_InputParam.Get1stCharaChange() ) )
		{
			ChangeChara( 0 );
		}
		else if( IsGetKey( m_InputParam.Get2ndCharaChange() ) )
		{
			ChangeChara( 1 );
		}
		else if( IsGetKey( m_InputParam.Get3rdCharaChange() ) )
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

		RestrictCharaPosition();
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
		var nextController = m_Controllers[m_CharaIndex];

		if( m_CurrentController != null )
		{
			m_CurrentController.gameObject.SetActive( false );
			nextController.transform.localPosition = m_CurrentController.transform.localPosition;
		}

		m_CurrentController = nextController;
		m_CurrentController.gameObject.SetActive( true );
		m_WaitChangeTime = 1f;
	}

	public void RegistChara( PlayerController controller )
	{
		if( controller == null || m_Controllers.Contains( controller ) )
		{
			return;
		}

		controller.transform.SetParent( m_PlayerCharaHolder );
		m_Controllers.Add( controller );
		controller.OnInitialize();

		// 最初のキャラだけONにする
		if( m_Controllers.Count > 1 )
		{
			controller.gameObject.SetActive( false );
		}
	}

	public void RestrictCharaPosition()
	{
		var chara = GetCurrentController();

		if( chara == null )
		{
			return;
		}

		Vector3 viewPos = CameraManager.Instance.WorldToViewportPoint( chara.transform.position );
		float x = Mathf.Clamp( viewPos.x, m_MinViewportRestrict.x, m_MaxViewportRestrict.x );
		float y = Mathf.Clamp( viewPos.y, m_MinViewportRestrict.y, m_MaxViewportRestrict.y );

		if( viewPos.x == x && viewPos.y == y )
		{
			return;
		}

		var pos = CameraManager.Instance.GetViewportWorldPoint( x, y );
		chara.transform.position = pos;
	}
}

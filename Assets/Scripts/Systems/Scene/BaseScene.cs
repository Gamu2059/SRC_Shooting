using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

/// <summary>
/// シーンごとのマネージャ等を管理するコンポーネント。
/// </summary>
public class BaseScene : MonoBehaviour, IControllableGameCycle
{
	/// <summary>
	/// このシーンが何のシーンなのか。
	/// </summary>
	[SerializeField]
	private BaseSceneManager.E_SCENE m_Scene;

	/// <summary>
	/// このシーンのサイクル。
	/// </summary>
	[SerializeField]
	private BaseSceneManager.E_SCENE_CYCLE m_SceneCycle;

	/// <summary>
	/// このシーンに固有で紐づいているマネージャのリスト。
	/// </summary>
	[SerializeField]
	private List<IControllableGameCycle> m_ManagerList;



	#region Getter & Setter

	/// <summary>
	/// このシーンの値を取得する。
	/// </summary>
	public BaseSceneManager.E_SCENE GetScene()
	{
		return m_Scene;
	}

	/// <summary>
	/// このシーンのサイクルを取得する。
	/// </summary>
	public BaseSceneManager.E_SCENE_CYCLE GetSceneCycle()
	{
		return m_SceneCycle;
	}

	/// <summary>
	/// このシーンのサイクルを設定する。
	/// </summary>
	public void SetSceneCycle( BaseSceneManager.E_SCENE_CYCLE value )
	{
		m_SceneCycle = value;
	}

	/// <summary>
	/// このシーンに固有で紐づいているマネージャのリストを取得する。。
	/// </summary>
	public List<IControllableGameCycle> GetManagerList()
	{
		return m_ManagerList;
	}

	#endregion

	/// <summary>
	/// Unityで制御される生成直後に呼び出される処理。
	/// GameManagerが初期化されていない場合、最初にあるシーンに強制的に遷移する。
	/// </summary>
	private void Awake()
	{
		if( !GameManager.CheckExistInstance() )
		{
			BaseSceneManager.SetBeginScene( GetScene() );
			SceneManager.LoadScene( 0 );
			return;
		}

		BaseSceneManager.Instance.RegisterScene( this );
		OnAwake();
	}

	/// <summary>
	/// Unityで制御される破棄直前に呼び出される処理。
	/// </summary>
	private void OnDestroy()
	{
		OnDestroyed();
	}

	/// <summary>
	/// インスタンス生成直後に呼び出される処理。
	/// </summary>
	protected virtual void OnAwake()
	{
		m_ManagerList = new List<IControllableGameCycle>();
	}

	/// <summary>
	/// インスタンス破棄直前に呼び出される処理。
	/// </summary>
	protected virtual void OnDestroyed()
	{
		m_ManagerList.Clear();
		m_ManagerList = null;
	}

	public virtual void OnInitialize()
	{
		m_ManagerList.ForEach( ( m ) => m.OnInitialize() );
	}

	public virtual void OnFinalize()
	{
		m_ManagerList.ForEach( ( m ) => m.OnFinalize() );
	}

	public virtual void OnStart()
	{
		m_ManagerList.ForEach( ( m ) => m.OnStart() );
	}

	public virtual void OnUpdate()
	{
		m_ManagerList.ForEach( ( m ) => m.OnUpdate() );
	}

	public virtual void OnLateUpdate()
	{
		m_ManagerList.ForEach( ( m ) => m.OnLateUpdate() );
	}

	public virtual void OnFixedUpdate()
	{
		m_ManagerList.ForEach( ( m ) => m.OnFixedUpdate() );
	}

	/// <summary>
	/// シーン遷移前の演出が入る直前に呼び出される処理。
	/// </summary>
	public virtual void OnBeforeHide( Action onComplete )
	{
		EventUtility.SafeInvokeAction( onComplete );
	}

	/// <summary>
	/// シーン遷移前の演出が入る直前に呼び出される処理。
	/// </summary>
	public virtual void OnAfterHide( Action onComplete )
	{
		EventUtility.SafeInvokeAction( onComplete );
	}

	/// <summary>
	/// シーン遷移前の演出が入る直前に呼び出される処理。
	/// </summary>
	public virtual void OnBeforeShow( Action onComplete )
	{
		EventUtility.SafeInvokeAction( onComplete );
	}

	/// <summary>
	/// シーン遷移前の演出が入る直前に呼び出される処理。
	/// </summary>
	public virtual void OnAfterShow( Action onComplete )
	{
		EventUtility.SafeInvokeAction( onComplete );
	}
}
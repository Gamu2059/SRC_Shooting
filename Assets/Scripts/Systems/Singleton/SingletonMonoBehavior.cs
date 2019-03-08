using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シングルトンパターンを実装したMonoBehaviorの基底クラス。
/// </summary>
public abstract class SingletonMonoBehavior<T> : MonoBehaviour, IControllableGameCycle where T : MonoBehaviour
{

	public static T Instance
	{
		get;
		private set;
	}

	/// <summary>
	/// Unityで制御される生成直後に呼び出される処理。
	/// </summary>
	private void Awake()
	{
		if( CheckExistInstance() )
		{
			Destroy( gameObject );
		}
		else
		{
			Instance = GetComponent<T>();
			OnAwake();
		}
	}

	/// <summary>
	/// Unityで制御される破棄直前に呼び出される処理。
	/// </summary>
	private void OnDestroy()
	{
		OnDestroyed();
		Instance = null;
	}

	/// <summary>
	/// このクラスのインスタンスが存在するかどうかを取得する。
	/// </summary>
	public static bool CheckExistInstance()
	{
		return Instance;
	}

	/// <summary>
	/// インスタンス生成直後に呼び出される処理。
	/// </summary>
	protected virtual void OnAwake()
	{
	}

	/// <summary>
	/// インスタンス破棄直前に呼び出される処理。
	/// </summary>
	protected virtual void OnDestroyed()
	{
	}

	public virtual void OnInitialize()
	{
	}

	public virtual void OnFinalize()
	{
	}

	public virtual void OnStart()
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual void OnLateUpdate()
	{
	}
}
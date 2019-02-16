using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シングルトンパターンを実装したMonoBehaviorの基底クラス。
/// ただし、これを直接継承しないで下さい。
/// </summary>
public abstract class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
{

	public static T Instance
	{
		get;
		private set;
	}

	protected virtual void Awake()
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

	public static bool CheckExistInstance()
	{
		return Instance;
	}

	protected virtual void OnAwake()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	public virtual void OnInit()
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
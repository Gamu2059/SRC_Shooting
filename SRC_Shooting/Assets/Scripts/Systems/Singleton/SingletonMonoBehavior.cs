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

	protected virtual void OnAwake()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	public static bool CheckExistInstance()
	{
		return Instance;
	}

	public virtual void Init()
	{
	}
}
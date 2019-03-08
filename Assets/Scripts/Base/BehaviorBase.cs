using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SRC_Shootingでコンポーネントを制御しやすいようにするための拡張です。
/// </summary>
public class ControllableMonoBehaviour : MonoBehaviour, IControllableGameCycle
{
	private void Awake()
	{
		OnAwake();
	}

	private void OnDestroy()
	{
		OnDestroyed();
	}

	/// <summary>
	/// Awakeで呼び出されるメソッド。
	/// </summary>
	public virtual void OnAwake() { }

	/// <summary>
	/// OnDestroyで呼び出されるメソッド。
	/// </summary>
	public virtual void OnDestroyed() { }


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

	public virtual void OnFixedUpdate()
	{
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SRC_Shootingでコンポーネントを制御しやすいようにするための拡張です。
/// </summary>
public class BehaviorBase : MonoBehaviour
{
	/// <summary>
	/// 上位の制御スクリプトのAwakeで呼び出されるメソッド。
	/// </summary>
	public virtual void OnAwake() { }

	/// <summary>
	/// 上位の制御スクリプトのStartで呼び出されるメソッド。
	/// </summary>
	public virtual void OnStart() { }

	/// <summary>
	/// 上位の制御スクリプトのUpdateで呼び出されるメソッド。
	/// </summary>
	public virtual void OnUpdate() { }

	/// <summary>
	/// 上位の制御スクリプトのLateUpdateで呼び出されるメソッド。
	/// </summary>
	public virtual void OnLateUpdate() { }

	/// <summary>
	/// 上位の制御スクリプトのOnDestroyで呼び出されるメソッド。
	/// </summary>
	public virtual void OnDestroyed() { }

}

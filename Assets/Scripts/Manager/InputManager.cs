using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの入力を管理する。
/// </summary>
public class InputManager : SingletonMonoBehavior<InputManager>
{
	/// <summary>
	/// 使用する入力パラメータ
	/// </summary>
	[SerializeField]
	private InputParam m_InputParam;

	public override void OnUpdate()
	{
		base.OnUpdate();
	}
}

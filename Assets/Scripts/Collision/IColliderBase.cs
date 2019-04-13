using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColliderBase
{
	/// <summary>
	/// 当たり判定情報を取得する。
	/// 複数あることを考慮して配列として取得する。
	/// </summary>
	ColliderData[] GetColliderData();
}

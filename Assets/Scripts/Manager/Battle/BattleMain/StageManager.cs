using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メインのバトル画面のオブジェクトを保持するためのマネージャ。
/// </summary>
public class StageManager : SingletonMonoBehavior<StageManager>
{
	/// <summary>
	/// 動物体を保持するためのホルダー
	/// </summary>
	[SerializeField]
	private GameObject m_MoveObjectHolder;

	/// <summary>
	/// 静止物体を保持するためのホルダー
	/// </summary>
	[SerializeField]
	private GameObject m_FixedObjectHolder;

	/// <summary>
	/// ステージ上の背景オブジェクトを保持するためのホルダー
	/// </summary>
	[SerializeField]
	private GameObject m_StageObjectHolder;

	/// <summary>
	/// プレイヤーキャラを保持するためのホルダー
	/// </summary>
	[SerializeField]
	private GameObject m_PlayerCharaHolder;

	/// <summary>
	/// 敵キャラを保持するためのホルダー
	/// </summary>
	[SerializeField]
	private GameObject m_EnemyCharaHolder;

	/// <summary>
	/// 弾を保持するためのホルダー
	/// </summary>
	[SerializeField]
	private GameObject m_BulletHolder;

	public override void OnInitialize()
	{
		base.OnInitialize();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
	}

	public GameObject GetMoveObjectHolder()
	{
		return m_MoveObjectHolder;
	}

	public GameObject GetFixedObjectHolder()
	{
		return m_FixedObjectHolder;
	}

	public GameObject GetStageObjectHolder()
	{
		return m_StageObjectHolder;
	}

	public GameObject GetPlayerCharaHolder()
	{
		return m_PlayerCharaHolder;
	}

	public GameObject GetEnemyCharaHolder()
	{
		return m_EnemyCharaHolder;
	}

	public GameObject GetBulletHolder()
	{
		return m_BulletHolder;
	}

	/// <summary>
	/// MoveObjectHolderのピボットを基準にした座標に変換する。
	/// </summary>
	public Vector3 GetMoveObjectHolderBasePosition( Vector3 position )
	{
		return position - m_MoveObjectHolder.transform.position;
	}

	/// <summary>
	/// MoveObjectHolderのピボットを基準にした回転に変換する。
	/// </summary>
	public Vector3 GetMoveObjectHolderBaseEulerAngles( Vector3 eulerAngles )
	{
		return eulerAngles - m_MoveObjectHolder.transform.eulerAngles;
	}
}

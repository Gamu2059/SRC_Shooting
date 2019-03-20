using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メインのバトル画面のステージを管理する。
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

	[SerializeField]
	private ControllableMonoBehaviour m_StageController;

	public override void OnInitialize()
	{
		base.OnInitialize();
		m_StageController.OnInitialize();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		m_StageController.OnUpdate();

		//if( m_MoveObjectHolder == null )
		//{
		//	return;
		//}

		//if( m_StageController != null )
		//{
		//	m_StageController.OnUpdate();
		//}

		//m_MoveObjectHolder.transform.Translate( m_Direction * m_Speed * Time.deltaTime, Space.World );

		//if( m_MoveObjectHolder.transform.position.z > 448 )
		//{
		//	var pos = m_MoveObjectHolder.transform.position;
		//	pos.z = 0;
		//	m_MoveObjectHolder.transform.position = pos;
		//}
	}

	/// <summary>
	/// 移動型オブジェクトホルダの子にする。
	/// </summary>
	public void AddMoveObjectHolder( Transform obj )
	{
		if( obj == null )
		{
			return;
		}

		obj.SetParent( m_MoveObjectHolder.transform );
	}

	/// <summary>
	/// 固定型オブジェクトホルダの子にする。
	/// </summary>
	public void AddFixedObjectHolder( Transform obj )
	{
		if( obj == null )
		{
			return;
		}

		obj.SetParent( m_FixedObjectHolder.transform );
	}

	/// <summary>
	/// ステージオブジェクトホルダの子にする。
	/// </summary>
	public void AddStageObjectHolder( Transform obj )
	{
		if( obj == null )
		{
			return;
		}

		obj.SetParent( m_StageObjectHolder.transform );
	}

	/// <summary>
	/// プレイヤーキャラホルダの子にする。
	/// </summary>
	public void AddPlayerCharaHolder( Transform obj )
	{
		if( obj == null )
		{
			return;
		}

		obj.SetParent( m_PlayerCharaHolder.transform );
	}

	/// <summary>
	/// 敵キャラホルダの子にする。
	/// </summary>
	public void AddEnemyCharaHolder( Transform obj )
	{
		if( obj == null )
		{
			return;
		}

		obj.SetParent( m_EnemyCharaHolder.transform );
	}

	/// <summary>
	/// 弾ホルダの子にする。
	/// </summary>
	public void AddBulletHolder( Transform obj )
	{
		if( obj == null )
		{
			return;
		}

		obj.SetParent( m_BulletHolder.transform );
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

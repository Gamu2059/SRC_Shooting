using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メインのバトル画面のステージを管理する。
/// </summary>
public class StageManager : SingletonMonoBehavior<StageManager>
{
	[SerializeField]
	private float m_Speed;

	[SerializeField]
	private Vector3 m_Direction;

	[SerializeField]
	private GameObject m_MoveObjectHolder;

	[SerializeField]
	private GameObject m_FixedObjectHolder;

	[SerializeField]
	private GameObject m_StageObjectHolder;

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

		if( m_MoveObjectHolder == null )
		{
			return;
		}

		if( m_StageController != null )
		{
			m_StageController.OnUpdate();
		}

		m_MoveObjectHolder.transform.Translate( m_Direction * m_Speed * Time.deltaTime, Space.World );

		if( m_MoveObjectHolder.transform.position.z > 448 )
		{
			var pos = m_MoveObjectHolder.transform.position;
			pos.z = 0;
			m_MoveObjectHolder.transform.position = pos;
		}
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
}

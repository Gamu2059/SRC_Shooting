using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テストステージの敵配置やカメラ制御などを管理する。
/// </summary>
public class TestStageController : ControllableMonoBehaviour
{
	[System.Serializable]
	internal class EnemyAppearData
	{
		public float ApperTime;
		public Vector2 ApperPosition;
		public EnemyController Enemy;
		public bool IsAppeared;
	}

	[SerializeField]
	private List<EnemyAppearData> m_EnemyAppearDataList;

	[SerializeField]
	private EnemyAppearData m_BossAppearData;

	private bool m_IsStarted;
	private float m_Count;

	public override void OnInitialize()
	{
		base.OnInitialize();
	}

	public override void OnFinalize()
	{
		base.OnFinalize();
	}

	public override void OnStart()
	{
		base.OnStart();

		m_IsStarted = true;
		m_Count = 0;

		foreach( var appearData in m_EnemyAppearDataList )
		{
			appearData.IsAppeared = false;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if( !m_IsStarted )
		{
			OnStart();
		}

		foreach( var appearData in m_EnemyAppearDataList )
		{
			if( appearData.IsAppeared )
			{
				continue;
			}

			if( appearData.ApperTime >= m_Count )
			{
				appearData.IsAppeared = true;


			}
		}

		m_Count += Time.deltaTime;
	}

	public override void OnLateUpdate()
	{
		base.OnLateUpdate();
	}

	public override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
	}
}

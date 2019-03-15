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

			if( appearData.ApperTime < m_Count )
			{
				appearData.IsAppeared = true;
				var enemy = EnemyCharaManager.Instance.CreateEnemy( appearData.Enemy );

				if( enemy == null )
				{
					continue;
				}

				var camera = CameraManager.Instance.GetTargetCamera();
				var pos = GetViewportWorldPoint( camera, new Vector3( 0, 20, 0 ), Vector3.up, 0.5f, 1 );
				Debug.Log( pos );
				enemy.transform.position = pos;
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

	private Vector3 GetViewportWorldPoint( Camera camera, Vector3 basePos, Vector3 axis, float viewX, float viewY )
	{
		Vector3 farPos = camera.ViewportToWorldPoint( new Vector3( viewX, viewY, camera.farClipPlane ) );
		Vector3 originPos = camera.transform.position;
		Vector3 dir = farPos - originPos;

		// axisは単位ベクトルなのが前提
		float h = Vector3.Dot( basePos, axis );
		return originPos + dir * ( h - Vector3.Dot( axis, originPos ) ) / ( Vector3.Dot( axis, dir ) );
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テストステージの敵配置やカメラ制御などを管理する。
/// </summary>
public class TestStageController : ControllableMonoBehaviour
{
	[SerializeField]
	private StageEnemyParam m_StageEnemyParam;

	[SerializeField]
	private XL_StageEnemyParam m_StageEnemyList;

	[SerializeField]
	private List<XL_StageEnemyParam.Param> m_StageEnemyAppearData;
	[SerializeField]
	private List<XL_StageEnemyParam.Param> m_RemovingData;
	private bool m_IsStarted;
	private float m_Count;

	public override void OnInitialize()
	{
		base.OnInitialize();

		m_StageEnemyAppearData = new List<XL_StageEnemyParam.Param>();
		m_RemovingData = new List<XL_StageEnemyParam.Param>();

		BuildEnemyAppearData();
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


	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if( !m_IsStarted )
		{
			OnStart();
		}

		var camera = CameraManager.Instance.GetCamera();

		foreach( var data in m_StageEnemyAppearData )
		{
			if( data.Time >= m_Count )
			{
				continue;
			}

			var enemy = EnemyCharaManager.Instance.CreateEnemy( m_StageEnemyParam.GetEnemyControllers()[data.EnemyMoveId] );

			if( enemy == null )
			{
				continue;
			}

			enemy.SetBulletSetParam( m_StageEnemyParam.GetBulletSets()[data.BulletSetId] );

			var pos = GetViewportWorldPoint( camera, data );
			pos.x += data.AppearOffsetX;
			pos.y += data.AppearOffsetY;
			pos.z += data.AppearOffsetZ;
			enemy.transform.position = pos;

			var rot = enemy.transform.eulerAngles;
			rot.y = data.AppearRotateY;
			enemy.transform.eulerAngles = rot;

			m_RemovingData.Add( data );
		}

		RemoveEnemyAppearData();

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

	private void BuildEnemyAppearData()
	{
		if( m_StageEnemyList == null )
		{
			return;
		}

		m_StageEnemyAppearData.Clear();
		m_StageEnemyAppearData.AddRange( m_StageEnemyList.sheets[0].list );
	}

	private void RemoveEnemyAppearData()
	{
		foreach( var data in m_RemovingData )
		{
			m_StageEnemyAppearData.Remove( data );
		}

		m_RemovingData.Clear();
	}

	private Vector3 GetViewportWorldPoint( Camera camera, XL_StageEnemyParam.Param param )
	{
		Vector3 farPos = camera.ViewportToWorldPoint( new Vector3( param.AppearViewportX, param.AppearViewportY, camera.nearClipPlane ) );
		Vector3 originPos = camera.transform.position;
		Vector3 dir = ( farPos - originPos ).normalized;

		Vector3 axis = Vector3.up;
		float h = Vector3.Dot( new Vector3( 0, ParamDef.BASE_Y_POS, 0 ), axis );
		return originPos + dir * ( h - Vector3.Dot( axis, originPos ) ) / ( Vector3.Dot( axis, dir ) );
	}
}

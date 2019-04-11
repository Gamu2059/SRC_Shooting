using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テストステージの敵配置やカメラ制御などを管理する。
/// </summary>
public class TestStageController : ControllableMonoBehaviour
{
	public enum E_TEST_STAGE_STATE
	{
		FIRST_HALF,
		SECOND_HALF,
		BOSS
	}

	[Header( "敵出現パラメータ" )]

	[SerializeField]
	private StageEnemyParam m_StageEnemyParam;

	[SerializeField]
	private XL_StageEnemyParam m_StageEnemyList;

	[Header( "カメラ移動パラメータ" )]

	[SerializeField]
	private float m_MoveSpeed;



	private List<XL_StageEnemyParam.Param> m_StageEnemyAppearData;
	private List<XL_StageEnemyParam.Param> m_RemovingData;

	[SerializeField]
	private E_TEST_STAGE_STATE m_StageState;

	[SerializeField]
	private float m_BuildEnemyTimeCount;


	public override void OnInitialize()
	{
		base.OnInitialize();

		m_StageEnemyAppearData = new List<XL_StageEnemyParam.Param>();
		m_RemovingData = new List<XL_StageEnemyParam.Param>();
	}

	public override void OnFinalize()
	{
		base.OnFinalize();
	}

	public override void OnStart()
	{
		base.OnStart();

		m_BuildEnemyTimeCount = 0;
		m_StageState = E_TEST_STAGE_STATE.FIRST_HALF;

		BuildEnemyAppearData();
		BattleMainAudioManager.Instance.PlayBGM( BattleMainAudioManagerKeyWord.Stage1 );
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		ControlViewMoving();
		AppearEnemy();
	}

	public override void OnLateUpdate()
	{
		base.OnLateUpdate();
	}

	public override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
	}

	private void ControlViewMoving()
	{
		var moveRoot = StageManager.Instance.GetMoveObjectHolder();

		switch( m_StageState )
		{
			case E_TEST_STAGE_STATE.FIRST_HALF:
				moveRoot.transform.Translate( Vector3.forward * m_MoveSpeed * Time.deltaTime );

				if( m_BuildEnemyTimeCount >= 80 )
				{
					m_StageState = E_TEST_STAGE_STATE.SECOND_HALF;
				}


				break;

			case E_TEST_STAGE_STATE.SECOND_HALF:
				moveRoot.transform.Translate( Vector3.forward * m_MoveSpeed * Time.deltaTime );

				if( m_BuildEnemyTimeCount >= 120 )
				{
					m_StageState = E_TEST_STAGE_STATE.BOSS;
				}

				break;

			case E_TEST_STAGE_STATE.BOSS:
				var bgm = BattleMainAudioManagerKeyWord.Stage1Boss;

				if( !BattleMainAudioManager.Instance.IsPlayingBGM( bgm ) )
				{

					BattleMainAudioManager.Instance.PlayBGM( bgm );
				}

				break;
		}
	}

	private void AppearEnemy()
	{
		foreach( var data in m_StageEnemyAppearData )
		{
			if( data.Time >= m_BuildEnemyTimeCount )
			{
				continue;
			}

			var enemy = EnemyCharaManager.Instance.CreateEnemy( m_StageEnemyParam.GetEnemyControllers()[data.EnemyMoveId], data.OtherParameters );

			if( enemy == null )
			{
				continue;
			}

			enemy.SetBulletSetParam( m_StageEnemyParam.GetBulletSets()[data.BulletSetId] );

            var pos = EnemyCharaManager.Instance.GetPositionFromFieldViewPortPosition(data.AppearViewportX, data.AppearViewportY);
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

		m_BuildEnemyTimeCount += Time.deltaTime;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テストステージの敵配置やカメラ制御などを管理する。
/// </summary>
public class TestStageController : StageController
{
	public enum E_TEST_STAGE_STATE
	{
		FIRST_HALF,
		SECOND_HALF,
		BOSS
	}

    [SerializeField]
    private float m_MoveSpeed;

	[SerializeField]
	private E_TEST_STAGE_STATE m_StageState;

	public override void OnStart()
	{
		base.OnStart();

		m_StageState = E_TEST_STAGE_STATE.FIRST_HALF;
		BattleMainAudioManager.Instance.PlayBGM( BattleMainAudioManagerKeyWord.Stage1 );
	}

	protected override void ControlViewMoving()
	{
		//var moveRoot = BattleRealStageManager.Instance.GetMoveObjectHolder();

		//switch( m_StageState )
		//{
		//	case E_TEST_STAGE_STATE.FIRST_HALF:
		//		moveRoot.transform.Translate( Vector3.forward * m_MoveSpeed * Time.deltaTime );

		//		if( m_BuildEnemyTimeCount >= 80 )
		//		{
		//			m_StageState = E_TEST_STAGE_STATE.SECOND_HALF;
		//		}


		//		break;

		//	case E_TEST_STAGE_STATE.SECOND_HALF:
		//		moveRoot.transform.Translate( Vector3.forward * m_MoveSpeed * Time.deltaTime );

		//		if( m_BuildEnemyTimeCount >= 120 )
		//		{
		//			m_StageState = E_TEST_STAGE_STATE.BOSS;
		//		}

		//		break;

		//	case E_TEST_STAGE_STATE.BOSS:
		//		var bgm = BattleMainAudioManagerKeyWord.Stage1Boss;

		//		if( !BattleMainAudioManager.Instance.IsPlayingBGM( bgm ) )
		//		{

		//			BattleMainAudioManager.Instance.PlayBGM( bgm );
		//		}

		//		break;
		//}
	}

	protected override void AppearEnemy()
	{
		foreach( var data in m_StageEnemyAppearData )
		{
			//if( data.Time >= m_BuildEnemyTimeCount )
			//{
			//	continue;
			//}

			//var enemy = EnemyCharaManager.Instance.CreateEnemy( m_StageEnemyParam.GetEnemyControllers()[data.EnemyMoveId], data.OtherParameters );

			//if( enemy == null )
			//{
			//	continue;
			//}

			//enemy.SetBulletSetParam( m_StageEnemyParam.GetBulletSets()[data.BulletSetId] );

   //         var pos = EnemyCharaManager.Instance.GetPositionFromFieldViewPortPosition(data.AppearViewportX, data.AppearViewportY);
			//pos.x += data.AppearOffsetX;
			//pos.y += data.AppearOffsetY;
			//pos.z += data.AppearOffsetZ;
			//enemy.transform.position = pos;

			//var rot = enemy.transform.eulerAngles;
			//rot.y = data.AppearRotateY;
			//enemy.transform.eulerAngles = rot;

			//m_RemovingData.Add( data );
		}

		RemoveEnemyAppearData();

		m_BuildEnemyTimeCount += Time.deltaTime;
	}
}

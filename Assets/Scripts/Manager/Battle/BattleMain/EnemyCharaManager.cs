using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵コントローラの管理を行う。
/// </summary>
public class EnemyCharaManager : BattleSingletonMonoBehavior<EnemyCharaManager>
{
    public const string HOLDER_NAME = "[EnemyCharaHolder]";

    /// <summary>
    /// ステージ領域の左下に対するオフセット左下領域
    /// </summary>
    [SerializeField]
    private Vector2 m_OffsetMinField;

    /// <summary>
    /// ステージ領域の右上に対するオフセット右上領域
    /// </summary>
    [SerializeField]
    private Vector2 m_OffsetMaxField;

    /// <summary>
    /// 消滅可能になるまでの最小時間
    /// </summary>
    [SerializeField]
    private float m_CanOutTime;


    #region Field

	private Transform m_EnemyCharaHolder;

	/// <summary>
	/// UPDATE状態の敵を保持するリスト。
	/// </summary>
	private List<EnemyController> m_UpdateEnemies;

	/// <summary>
	/// 破棄状態に遷移する敵のリスト。
	/// </summary>
	private List<EnemyController> m_GotoDestroyEnemies;

	/// <summary>
	/// ボスのオブジェクトのみを保持する。
	/// </summary>
	private List<EnemyController> m_BossControllers;

    #endregion



    #region Get Set

	/// <summary>
	/// ゲームサイクルに入っている敵を取得する。
	/// </summary>
	public List<EnemyController> GetUpdateEnemies()
	{
		return m_UpdateEnemies;
	}

	/// <summary>
	/// ステージ上の全てのボス敵を取得する。
	/// </summary>
	public List<EnemyController> GetBossEnemies()
	{
		return m_BossControllers;
	}

    /// <summary>
    /// 消滅可能になるまでの最小時間を取得する。
    /// </summary>
    public float GetCanOutTime()
    {
        return m_CanOutTime;
    }

    #endregion



    protected override void OnAwake()
	{
		base.OnAwake();

		m_UpdateEnemies = new List<EnemyController>();
		m_GotoDestroyEnemies = new List<EnemyController>();
		m_BossControllers = new List<EnemyController>();
	}

	protected override void OnDestroyed()
	{
		base.OnDestroyed();
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
	}

	public override void OnFinalize()
	{
		base.OnFinalize();

		DestroyAllEnemyImmediate();
	}

	public override void OnStart()
	{
		base.OnStart();

		if( StageManager.Instance != null && StageManager.Instance.GetEnemyCharaHolder() != null )
		{
			m_EnemyCharaHolder = StageManager.Instance.GetEnemyCharaHolder().transform;
		}
		else if( m_EnemyCharaHolder == null )
		{
			var obj = new GameObject( HOLDER_NAME );
			obj.transform.position = Vector3.zero;
			m_EnemyCharaHolder = obj.transform;
		}
	}

	public override void OnUpdate()
	{
        // Update処理
		foreach( var enemy in m_UpdateEnemies )
		{
			if( enemy == null )
			{
				m_GotoDestroyEnemies.Add( enemy );
				continue;
			}

            if (enemy.GetCycle() == E_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                enemy.OnStart();
                enemy.SetCycle(E_OBJECT_CYCLE.UPDATE);
            }

			enemy.OnUpdate();
		}
	}

	public override void OnLateUpdate()
	{
        // LateUpdate処理
		foreach( var enemy in m_UpdateEnemies )
		{
			if( enemy == null )
			{
				m_GotoDestroyEnemies.Add( enemy );
				continue;
			}

			enemy.OnLateUpdate();
		}

		GotoDestroyFromUpdate();
	}

	private void GotoDestroyFromUpdate()
	{
		int count = m_GotoDestroyEnemies.Count;

		for( int i = 0; i < count; i++ )
		{
			int idx = count - i - 1;
			var enemy = m_GotoDestroyEnemies[idx];

			if( enemy == null )
			{
				continue;
			}

			m_GotoDestroyEnemies.RemoveAt( idx );
			m_UpdateEnemies.Remove( enemy );
            enemy.SetCycle(E_OBJECT_CYCLE.DESTROYED);
			enemy.OnFinalize();
			Destroy( enemy.gameObject );
		}

		m_GotoDestroyEnemies.Clear();

		m_UpdateEnemies.RemoveAll( ( e ) => e == null );
	}


	/// <summary>
	/// 敵キャラを登録する。
	/// いずれこのメソッドは外部から参照できなくする予定です。
	/// </summary>
	public EnemyController RegistEnemy( EnemyController controller )
	{
		if( controller == null || m_UpdateEnemies.Contains( controller ) || m_GotoDestroyEnemies.Contains( controller ) )
		{
			return null;
		}

		controller.transform.SetParent( m_EnemyCharaHolder );
		m_UpdateEnemies.Add( controller );
        controller.SetCycle(E_OBJECT_CYCLE.STANDBY_UPDATE);
		controller.OnInitialize();
		return controller;
	}

	/// <summary>
	/// 敵キャラのプレハブから敵キャラを新規作成する。
	/// </summary>
	public EnemyController CreateEnemy( EnemyController enemyPrefab )
	{
		if( enemyPrefab == null )
		{
			return null;
		}

		EnemyController controller = Instantiate( enemyPrefab );
		return RegistEnemy( controller );
	}

	public EnemyController CreateEnemy( EnemyController enemyPrefab, string paramString )
	{
		var enemy = CreateEnemy( enemyPrefab );

		if( enemy == null )
		{
			return null;
		}

		enemy.SetStringParam( paramString );

		return enemy;
	}

	/// <summary>
	/// 敵キャラを破棄する。
	/// これを呼び出したタイミングの次のLateUpdateで削除される。
	/// </summary>
	public void DestroyEnemy( EnemyController controller )
	{
		if( controller == null || !m_UpdateEnemies.Contains( controller ) )
		{
			return;
		}

		m_GotoDestroyEnemies.Add( controller );
        controller.SetCycle(E_OBJECT_CYCLE.STANDBY_DESTROYED);
	}

	/// <summary>
	/// 全ての敵キャラを破棄する。
	/// これを呼び出したタイミングの次のLateUpdateで削除される。
	/// </summary>
	public void DestroyAllEnemy()
	{
		m_GotoDestroyEnemies.AddRange( m_UpdateEnemies );
		m_UpdateEnemies.Clear();
	}

	/// <summary>
	/// 敵キャラを破棄する。
	/// これを呼び出したタイミングで即座に削除される。
	/// </summary>
	public void DestroyEnemyImmediate( EnemyController controller )
	{
		if( controller == null )
		{
			return;
		}

		controller.OnFinalize();
		Destroy( controller.gameObject );
	}

	/// <summary>
	/// 全ての敵キャラを破棄する。
	/// これを呼び出したタイミングで即座に削除される。
	/// </summary>
	public void DestroyAllEnemyImmediate()
	{
		foreach( var enemy in m_UpdateEnemies )
		{
			DestroyEnemyImmediate( enemy );
		}

		m_UpdateEnemies.Clear();
	}

    /// <summary>
    /// 動体フィールド領域のビューポート座標から、実際の座標を取得する。
    /// </summary>
    /// <param name="x">フィールド領域x座標</param>
    /// <param name="y">フィールド領域y座標</param>
    /// <returns></returns>
    public Vector3 GetPositionFromFieldViewPortPosition(float x, float y)
    {
        var minPos = StageManager.Instance.GetMinLocalPositionField();
        var maxPos = StageManager.Instance.GetMaxLocalPositionField();
        minPos += m_OffsetMinField;
        maxPos += m_OffsetMaxField;

        var factX = (maxPos.x - minPos.x) * x + minPos.x;
        var factZ = (maxPos.y - minPos.y) * y + minPos.y;
        var pos = new Vector3(factX, ParamDef.BASE_Y_POS, factZ);

        return pos;
    }
}

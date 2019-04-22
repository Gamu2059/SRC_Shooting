using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵の振る舞いの制御を行う。
/// </summary>
public class CommandEnemyCharaManager : BattleSingletonMonoBehavior<CommandEnemyCharaManager>
{
    public const string HOLDER_NAME = "[CommandEnemyCharaHolder]";

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
    private List<CommandEnemyController> m_UpdateEnemies;

    /// <summary>
    /// 破棄状態に遷移する敵のリスト。
    /// </summary>
    private List<CommandEnemyController> m_GotoDestroyEnemies;

    #endregion



    #region Get Set

    /// <summary>
    /// ゲームサイクルに入っている敵を取得する。
    /// </summary>
    public List<CommandEnemyController> GetUpdateEnemies()
    {
        return m_UpdateEnemies;
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

        m_UpdateEnemies = new List<CommandEnemyController>();
        m_GotoDestroyEnemies = new List<CommandEnemyController>();
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

        if (CommandStageManager.Instance != null && CommandStageManager.Instance.GetEnemyCharaHolder() != null)
        {
            m_EnemyCharaHolder = CommandStageManager.Instance.GetEnemyCharaHolder().transform;
        }
        else if (m_EnemyCharaHolder == null)
        {
            var obj = new GameObject(HOLDER_NAME);
            obj.transform.position = Vector3.zero;
            m_EnemyCharaHolder = obj.transform;
        }
    }

    public override void OnUpdate()
    {
        // Update処理
        foreach (var enemy in m_UpdateEnemies)
        {
            if (enemy == null)
            {
                m_GotoDestroyEnemies.Add(enemy);
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
        foreach (var enemy in m_UpdateEnemies)
        {
            if (enemy == null)
            {
                m_GotoDestroyEnemies.Add(enemy);
                continue;
            }

            enemy.OnLateUpdate();
        }

        GotoDestroyFromUpdate();
    }

    private void GotoDestroyFromUpdate()
    {
        int count = m_GotoDestroyEnemies.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var enemy = m_GotoDestroyEnemies[idx];

            if (enemy == null)
            {
                continue;
            }

            m_GotoDestroyEnemies.RemoveAt(idx);
            m_UpdateEnemies.Remove(enemy);
            enemy.SetCycle(E_OBJECT_CYCLE.DESTROYED);
            enemy.OnFinalize();
            Destroy(enemy.gameObject);
        }

        m_GotoDestroyEnemies.Clear();

        m_UpdateEnemies.RemoveAll((e) => e == null);
    }


    /// <summary>
    /// 敵キャラを登録する。
    /// いずれこのメソッドは外部から参照できなくする予定です。
    /// </summary>
    public CommandEnemyController RegistEnemy(CommandEnemyController controller)
    {
        if (controller == null || m_UpdateEnemies.Contains(controller) || m_GotoDestroyEnemies.Contains(controller))
        {
            return null;
        }

        controller.transform.SetParent(m_EnemyCharaHolder);
        m_UpdateEnemies.Add(controller);
        controller.SetCycle(E_OBJECT_CYCLE.STANDBY_UPDATE);
        controller.OnInitialize();
        return controller;
    }

    /// <summary>
    /// 敵キャラのプレハブから敵キャラを新規作成する。
    /// </summary>
    public CommandEnemyController CreateEnemy(CommandEnemyController enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            return null;
        }

        CommandEnemyController controller = Instantiate(enemyPrefab);
        return RegistEnemy(controller);
    }

    public CommandEnemyController CreateEnemy(CommandEnemyController enemyPrefab, string paramString)
    {
        var enemy = CreateEnemy(enemyPrefab);

        if (enemy == null)
        {
            return null;
        }

        enemy.SetStringParam(paramString);

        return enemy;
    }

    /// <summary>
    /// 敵キャラを破棄する。
    /// これを呼び出したタイミングの次のLateUpdateで削除される。
    /// </summary>
    public void DestroyEnemy(CommandEnemyController controller)
    {
        if (controller == null || !m_UpdateEnemies.Contains(controller))
        {
            return;
        }

        m_GotoDestroyEnemies.Add(controller);
    }

    /// <summary>
    /// 全ての敵キャラを破棄する。
    /// これを呼び出したタイミングの次のLateUpdateで削除される。
    /// </summary>
    public void DestroyAllEnemy()
    {
        m_GotoDestroyEnemies.AddRange(m_UpdateEnemies);
        m_UpdateEnemies.Clear();
    }

    /// <summary>
    /// 敵キャラを破棄する。
    /// これを呼び出したタイミングで即座に削除される。
    /// </summary>
    public void DestroyEnemyImmediate(CommandEnemyController controller)
    {
        if (controller == null)
        {
            return;
        }

        controller.OnFinalize();
        Destroy(controller.gameObject);
    }

    /// <summary>
    /// 全ての敵キャラを破棄する。
    /// これを呼び出したタイミングで即座に削除される。
    /// </summary>
    public void DestroyAllEnemyImmediate()
    {
        foreach (var enemy in m_UpdateEnemies)
        {
            DestroyEnemyImmediate(enemy);
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
        pos += StageManager.Instance.GetMoveObjectHolder().transform.position;

        return pos;
    }
}

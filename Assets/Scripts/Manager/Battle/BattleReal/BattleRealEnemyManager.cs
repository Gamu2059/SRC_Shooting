using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// リアルモードの敵キャラを管理する。
/// </summary>
public class BattleRealEnemyManager : ControllableObject
{
    public static BattleRealEnemyManager Instance => BattleRealManager.Instance.EnemyManager;

    [SerializeField, Tooltip("このステージで登場する敵データ")]
    private StageEnemyParam m_StageEnemyParam;

    [SerializeField, Tooltip("このステージで使用する敵出現データ")]
    private XlBattleMainEnemyParam m_EnemyParam;

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



    public override void OnInitialize()
    {
        base.OnInitialize();
        m_UpdateEnemies = new List<EnemyController>();
        m_GotoDestroyEnemies = new List<EnemyController>();
        m_BossControllers = new List<EnemyController>();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();

        DestroyAllEnemyImmediate();
    }

    public override void OnStart()
    {
        base.OnStart();

        m_EnemyCharaHolder = BattleRealStageManager.Instance.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.ENEMY);
        BuildEnemyAppearEvents();
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
            GameObject.Destroy(enemy.gameObject);
        }

        m_GotoDestroyEnemies.Clear();

        m_UpdateEnemies.RemoveAll((e) => e == null);
    }


    /// <summary>
    /// 敵キャラを登録する。
    /// いずれこのメソッドは外部から参照できなくする予定です。
    /// </summary>
    public EnemyController RegistEnemy(EnemyController controller)
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
    public EnemyController CreateEnemy(EnemyController enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            return null;
        }

        EnemyController controller = GameObject.Instantiate(enemyPrefab);
        return RegistEnemy(controller);
    }

    /// <summary>
    /// 敵リストから敵を新規作成する。
    /// </summary>
    public void CreateEnemyFromEnemyParam(int enemyListIndex)
    {
        int xlParamSize = m_EnemyParam.param.Count;

        if (enemyListIndex < 0 || enemyListIndex >= xlParamSize)
        {
            return;
        }

        var paramData = m_EnemyParam.param[enemyListIndex];
        var enemy = CreateEnemy(m_StageEnemyParam.GetEnemyControllers()[paramData.EnemyId]);

        if (enemy == null)
        {
            return;
        }

        enemy.SetBulletSetParam(m_StageEnemyParam.GetBulletSets()[paramData.BulletSetId]);
        enemy.SetArguments(paramData.OtherParameters);
        enemy.SetDropItemParam(paramData.Drop);
        enemy.SetDefeatParam(paramData.Defeat);
        enemy.InitHp(paramData.Hp);

        var pos = GetPositionFromFieldViewPortPosition(paramData.AppearViewportX, paramData.AppearViewportY);
        pos.x += paramData.AppearOffsetX;
        pos.y += paramData.AppearOffsetY;
        pos.z += paramData.AppearOffsetZ;
        enemy.transform.position = pos;

        var rot = enemy.transform.eulerAngles;
        rot.y = paramData.AppearRotateY;
        enemy.transform.eulerAngles = rot;
    }

    /// <summary>
    /// 敵キャラを破棄する。
    /// これを呼び出したタイミングの次のLateUpdateで削除される。
    /// </summary>
    public void DestroyEnemy(EnemyController controller)
    {
        if (controller == null || !m_UpdateEnemies.Contains(controller))
        {
            return;
        }

        m_GotoDestroyEnemies.Add(controller);
        controller.SetCycle(E_OBJECT_CYCLE.STANDBY_DESTROYED);
    }

    /// <summary>
    /// 全ての敵キャラを破棄する。
    /// これを呼び出したタイミングの次のLateUpdateで削除される。
    /// </summary>
    public void DestroyAllEnemy()
    {
        foreach (var enemy in m_UpdateEnemies)
        {
            DestroyEnemy(enemy);
        }

        m_UpdateEnemies.Clear();
    }

    /// <summary>
    /// 敵キャラを破棄する。
    /// これを呼び出したタイミングで即座に削除される。
    /// </summary>
    public void DestroyEnemyImmediate(EnemyController controller)
    {
        if (controller == null)
        {
            return;
        }

        controller.OnFinalize();
        GameObject.Destroy(controller.gameObject);
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
        var stageManager = BattleRealStageManager.Instance;
        var minPos = stageManager.MinLocalFieldPosition;
        var maxPos = stageManager.MaxLocalFieldPosition;
        minPos += m_OffsetMinField;
        maxPos += m_OffsetMaxField;

        var factX = (maxPos.x - minPos.x) * x + minPos.x;
        var factZ = (maxPos.y - minPos.y) * y + minPos.y;
        var pos = new Vector3(factX, ParamDef.BASE_Y_POS, factZ);

        return pos;
    }

    /// <summary>
    /// 敵の生成イベントをEventManagerに投げる
    /// </summary>
    private void BuildEnemyAppearEvents()
    {
        //for (int i = 0; i < m_EnemyParam.param.Count; i++)
        //{
        //    var param = m_EnemyParam.param[i];

        //    EventTriggerCondition condition = EventTriggerConditionTranslator.TranslateString(param.Conditions);

        //    EventTriggerParamSet.EventTriggerParam eventParam = new EventTriggerParamSet.EventTriggerParam();
        //    eventParam.Condition = condition;

        //    EventContent content = new EventContent();
        //    content.EventType = EventContent.E_EVENT_TYPE.APPEAR_ENEMY;
        //    content.AppearEnemyIndex = i;

        //    eventParam.Contents = new[] { content };

        //    //BattleRealEventManager.Instance.AddEventParam(eventParam);
        //}
    }
}

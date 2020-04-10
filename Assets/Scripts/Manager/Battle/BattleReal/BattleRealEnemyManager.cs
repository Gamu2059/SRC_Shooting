#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// リアルモードの敵キャラを管理する。
/// </summary>
public class BattleRealEnemyManager : Singleton<BattleRealEnemyManager>, IColliderProcess
{
    #region Field

    public BattleRealEnemyManagerParamSet ParamSet { get; private set; }

    private Transform m_EnemyEvacuationHolder;

    //// <summary>
    /// STANDBY状態の敵を保持するリスト。
    /// </summary>
    private List<BattleRealEnemyBase> m_StandbyEnemies;

    /// <summary>
    /// UPDATE状態の敵を保持するリスト。
    /// </summary>
    public List<BattleRealEnemyBase> Enemies { get; private set; }

    /// <summary>
    /// UPDATE状態の敵の中でもボスだけを保持するリスト。
    /// </summary>
    public List<BattleRealEnemyBase> BossEnemies { get; private set; }

    /// <summary>
    /// 破棄状態に遷移する敵のリスト。
    /// </summary>
    private List<BattleRealEnemyBase> m_GotoPoolEnemies;

    /// <summary>
    /// POOL状態の敵のディクショナリ。
    /// </summary>
    private Dictionary<string, LinkedList<GameObject>> m_PoolEnemies;

    private static List<BattleRealEnemyBase> m_ReservedRegisterEnemies = new List<BattleRealEnemyBase>();

    #endregion

    #region Open Callback

    public Action ToHackingAction { get; set; }
    public Action FromHackingAction { get; set; }

    #endregion

    #region Closed Callback

    private Action RequestToHackingAction { get; set; }

    #endregion

    public static BattleRealEnemyManager Builder(BattleRealManager realManager, BattleRealEnemyManagerParamSet param)
    {
        var manager = Create();
        manager.SetParam(param);
        manager.SetCallback(realManager);
        manager.OnInitialize();
        return manager;
    }

    private void SetParam(BattleRealEnemyManagerParamSet param)
    {
        ParamSet = param;
    }

    private void SetCallback(BattleRealManager manager)
    {
        manager.ChangeStateAction += OnChangeStateBattleRealManager;
        RequestToHackingAction += manager.RequestStartHacking;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StandbyEnemies = new List<BattleRealEnemyBase>();
        Enemies = new List<BattleRealEnemyBase>();
        BossEnemies = new List<BattleRealEnemyBase>();
        m_GotoPoolEnemies = new List<BattleRealEnemyBase>();
        m_PoolEnemies = new Dictionary<string, LinkedList<GameObject>>();
    }

    public override void OnFinalize()
    {
        RequestToHackingAction = null;
        FromHackingAction = null;
        ToHackingAction = null;
        DestroyAllEnemy();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        var stageManager = BattleRealStageManager.Instance;
        m_EnemyEvacuationHolder = stageManager.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.ENEMY);
        BuildEnemyGroupAppearEvents();
    }

    public override void OnUpdate()
    {
        // デバッグ用
        //foreach (var enemy in m_ReservedRegisterEnemies)
        //{
        //    Register(enemy);
        //}

        //m_ReservedRegisterEnemies.Clear();

        // Start
        foreach (var enemy in m_StandbyEnemies)
        {
            if (enemy == null)
            {
                continue;
            }

            enemy.OnStart();
        }

        GotoUpdateEnemy();

        // Update処理
        foreach (var enemy in Enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            enemy.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var enemy in Enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            enemy.OnLateUpdate();
        }
    }

    public override void OnFixedUpdate()
    {
        // FixedUpdate処理
        foreach (var enemy in Enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            enemy.OnFixedUpdate();
        }
    }

    #endregion

    #region Impl IColliderProcess

    public void ClearColliderFlag()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            enemy.ClearColliderFlag();
        }
    }

    public void UpdateCollider()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            enemy.UpdateCollider();
        }
    }

    public void ProcessCollision()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            enemy.ProcessCollision();
        }
    }

    #endregion

    /// <summary>
    /// 敵を登録する。
    /// デバッグ専用。
    /// </summary>
    [Obsolete]
    public static void RegisterEnemy(BattleRealEnemyBase enemy)
    {
        if (enemy == null || m_ReservedRegisterEnemies.Contains(enemy))
        {
            return;
        }

        m_ReservedRegisterEnemies.Add(enemy);
    }

    [Obsolete]
    private void Register(BattleRealEnemyBase enemy)
    {
        if (enemy == null || m_StandbyEnemies.Contains(enemy) || Enemies.Contains(enemy) || m_GotoPoolEnemies.Contains(enemy))
        {
            return;
        }

        var type = enemy.GetType().FullName;
        if (!m_PoolEnemies.ContainsKey(type))
        {
            m_PoolEnemies.Add(type, new LinkedList<GameObject>());
        }
        enemy.SetLookId(type);
        CheckStandByEnemy(enemy);
    }

    /// <summary>
    /// 破棄フラグが立っているものをプールに戻す
    /// </summary>
    public void GotoPool()
    {
        GotoPoolEnemy();
    }

    /// <summary>
    /// UPDATE状態にする。
    /// </summary>
    private void GotoUpdateEnemy()
    {
        foreach (var enemy in m_StandbyEnemies)
        {
            if (enemy == null)
            {
                continue;
            }
            else if (enemy.GetCycle() != E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                CheckPoolEnemy(enemy);
            }

            enemy.SetCycle(E_POOLED_OBJECT_CYCLE.UPDATE);
            Enemies.Add(enemy);

            if (enemy.IsBoss)
            {
                BossEnemies.Add(enemy);
            }
        }

        m_StandbyEnemies.Clear();
    }

    /// <summary>
    /// 削除フラグが立っている敵を一斉に削除する
    /// </summary>
    private void GotoPoolEnemy()
    {
        int count = m_GotoPoolEnemies.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var enemy = m_GotoPoolEnemies[idx];
            if (enemy == null)
            {
                continue;
            }

            enemy.OnFinalize();
            enemy.SetCycle(E_POOLED_OBJECT_CYCLE.POOLED);
            enemy.gameObject.SetActive(false);
            enemy.transform.SetParent(m_EnemyEvacuationHolder);

            m_GotoPoolEnemies.RemoveAt(idx);
            Enemies.Remove(enemy);

            if (enemy.IsBoss)
            {
                BossEnemies.Remove(enemy);
            }

            var poolId = enemy.GetLookId();
            if (!m_PoolEnemies.ContainsKey(poolId))
            {
                m_PoolEnemies.Add(poolId, new LinkedList<GameObject>());
            }
            m_PoolEnemies[poolId].AddLast(enemy.gameObject);
            GameObject.Destroy(enemy);
        }

        m_GotoPoolEnemies.Clear();
    }

    /// <summary>
    /// プールから敵を取得する。
    /// 足りなければ生成する。
    /// </summary>
    private GameObject GetPoolingEnemy(BattleRealEnemyLookParamSet lookParamSet)
    {
        if (lookParamSet == null)
        {
            return null;
        }

        var poolId = lookParamSet.LookId;
        GameObject enemyObj = null;

        if (m_PoolEnemies.ContainsKey(poolId))
        {
            if (m_PoolEnemies[poolId].Count > 0)
            {
                var node = m_PoolEnemies[poolId].First;
                enemyObj = node.Value;
            }
        }

        if (enemyObj == null)
        {
            enemyObj = GameObject.Instantiate(lookParamSet.EnemyPrefab);
            enemyObj.transform.SetParent(m_EnemyEvacuationHolder);

            if (!m_PoolEnemies.ContainsKey(poolId))
            {
                m_PoolEnemies.Add(poolId, new LinkedList<GameObject>());
            }
            m_PoolEnemies[poolId].AddLast(enemyObj);
        }

        return enemyObj;
    }

    /// <summary>
    /// 敵をSTANDBY状態にして制御下に入れる。
    /// </summary>
    private void CheckStandByEnemy(BattleRealEnemyBase enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("指定された敵を追加できませんでした。");
            return;
        }

        var poolId = enemy.GetLookId();
        if (!m_PoolEnemies.ContainsKey(poolId))
        {
            Debug.LogError("指定された敵を追加できませんでした。");
            return;
        }

        m_PoolEnemies[poolId].Remove(enemy.gameObject);
        m_StandbyEnemies.Add(enemy);
        enemy.gameObject.SetActive(true);
        enemy.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE);
        enemy.OnInitialize();
    }

    /// <summary>
    /// 指定した敵を制御から外すためにチェックする。
    /// </summary>
    private void CheckPoolEnemy(BattleRealEnemyBase enemy)
    {
        if (enemy == null || m_GotoPoolEnemies.Contains(enemy))
        {
            Debug.LogWarning("指定した敵を削除できませんでした。");
            return;
        }

        enemy.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_POOL);
        m_GotoPoolEnemies.Add(enemy);
    }

    /// <summary>
    /// 敵グループの生成リストから敵を新規作成する。
    /// </summary>
    public BattleRealEnemyBase CreateEnemy(BattleRealEnemyParamSetBase paramSet)
    {
        if (paramSet == null)
        {
            return null;
        }

        var difficulty = DataManager.Instance.Difficulty;
        var param = paramSet.GetEnemyParam(difficulty);
        if (param == null)
        {
            Debug.LogWarningFormat("EnemyParamBase is not found. file:{0}, difficulty:{1}", paramSet.name, difficulty);
            return null;
        }

        var enemyObj = GetPoolingEnemy(param.EnemyLookParamSet);
        if (enemyObj == null)
        {
            return null;
        }

        var controllerClassName = paramSet.GetControllerClassName();
        Type controllerType = null;

        try
        {
            controllerType = Type.GetType(controllerClassName);
        }
        catch (Exception)
        {
            Debug.LogWarningFormat("指定したクラス名のクラスを認識できませんでした。 {0}", controllerClassName);
            return null;
        }

        if (controllerType == null)
        {
            Debug.LogWarningFormat("指定したクラス名のクラス情報は取得できませんでした。 {0}", controllerClassName);
            return null;
        }

        BattleRealEnemyBase enemy = enemyObj.AddComponent(controllerType) as BattleRealEnemyBase;

        if (enemy == null)
        {
            Debug.LogWarningFormat("指定したクラスは敵コンポーネントではありません。{0}", controllerClassName);
            GameObject.Destroy(enemyObj);
            return null;
        }

        enemy.SetLookId(param.EnemyLookParamSet.LookId);
        enemy.SetParam(param);
        CheckStandByEnemy(enemy);
        return enemy;
    }

    /// <summary>
    /// 敵の生成イベントをEventManagerに投げる
    /// </summary>
    private void BuildEnemyGroupAppearEvents()
    {
        if (ParamSet == null || ParamSet.Generator == null)
        {
            return;
        }

        var groups = ParamSet.Generator.Contents;
        foreach(var param in groups)
        {
            var eventParam = ScriptableObject.CreateInstance<BattleRealEventTriggerParam>();
            var content = new BattleRealEventContent();
            content.EventType = BattleRealEventContent.E_EVENT_TYPE.APPEAR_ENEMY_GROUP;
            content.EnemyGroupParam = param.Param;
            eventParam.Condition = param.Condition;
            eventParam.Contents = new[] { content };
            BattleRealEventManager.Instance.AddEventParam(eventParam);
        }
    }

    /// <summary>
    /// 敵キャラを破棄する。
    /// これを呼び出したタイミングの次のLateUpdateで破棄される。
    /// </summary>
    public void DestroyEnemy(BattleRealEnemyBase enemy)
    {
        CheckPoolEnemy(enemy);
    }

    /// <summary>
    /// 全ての敵キャラを破棄する。
    /// これを呼び出したタイミングの次のLateUpdateで破棄される。
    /// </summary>
    public void DestroyAllEnemy()
    {
        foreach (var enemy in Enemies)
        {
            DestroyEnemy(enemy);
        }

        Enemies.Clear();
    }

    /// <summary>
    /// 全ての敵キャラを退場扱いにする。
    /// </summary>
    public void RetireAllEnemy()
    {
        Enemies.ForEach(e => e.Retire());
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
        minPos += ParamSet.MinOffsetFieldPosition;
        maxPos += ParamSet.MaxOffsetFieldPosition;

        var factX = (maxPos.x - minPos.x) * x + minPos.x;
        var factZ = (maxPos.y - minPos.y) * y + minPos.y;
        var pos = new Vector3(factX, ParamDef.BASE_Y_POS, factZ);

        return pos;
    }

    /// <summary>
    /// 敵が敵フィールドの範囲外に出ているかどうかを判定する。
    /// </summary>
    public bool IsOutOfField(BattleRealEnemyBase enemy)
    {
        if (enemy == null)
        {
            return true;
        }

        var stageManager = BattleRealStageManager.Instance;
        var minPos = stageManager.MinLocalFieldPosition;
        var maxPos = stageManager.MaxLocalFieldPosition;
        minPos += ParamSet.MinOffsetFieldPosition;
        maxPos += ParamSet.MaxOffsetFieldPosition;

        var pos = enemy.transform.position;

        return pos.x < minPos.x || pos.x > maxPos.x || pos.z < minPos.y || pos.z > maxPos.y;
    }

    /// <summary>
    /// BattleRealManager監視用
    /// ステートが切り替わった時に呼び出される
    /// </summary>
    private void OnChangeStateBattleRealManager(E_BATTLE_REAL_STATE state)
    {
        switch (state)
        {
            case E_BATTLE_REAL_STATE.TO_HACKING:
                ToHackingAction?.Invoke();
                break;
            case E_BATTLE_REAL_STATE.FROM_HACKING:
                FromHackingAction?.Invoke();
                break;
            default:
                break;
        }
    }

    public void RequestToHacking()
    {
        RequestToHackingAction?.Invoke();
    }
}

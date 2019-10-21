using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// リアルモードの敵キャラを管理する。
/// </summary>
public class BattleRealEnemyManager : ControllableObject, IColliderProcess
{
    public static BattleRealEnemyManager Instance => BattleRealManager.Instance.EnemyManager;


    /// <summary>
    /// 消滅可能になるまでの最小時間
    /// </summary>
    [SerializeField]
    private float m_CanOutTime;

    #region Field

    private BattleRealEnemyManagerParamSet m_ParamSet;

    private Transform m_EnemyEvacuationHolder;

    //// <summary>
    /// STANDBY状態の敵を保持するリスト。
    /// </summary>
    private List<BattleRealEnemyController> m_StandbyEnemies;

    /// <summary>
    /// UPDATE状態の敵を保持するリスト。
    /// </summary>
    private List<BattleRealEnemyController> m_UpdateEnemies;
    public List<BattleRealEnemyController> Enemies => m_UpdateEnemies;

    /// <summary>
    /// 破棄状態に遷移する敵のリスト。
    /// </summary>
    private List<BattleRealEnemyController> m_GotoPoolEnemies;

    /// <summary>
    /// POOL状態の敵のディクショナリ。
    /// </summary>
    private Dictionary<string, LinkedList<GameObject>> m_PoolEnemies;

    private static List<BattleRealEnemyController> m_ReservedRegisterEnemies = new List<BattleRealEnemyController>();

    #endregion

    public BattleRealEnemyManager(BattleRealEnemyManagerParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StandbyEnemies = new List<BattleRealEnemyController>();
        m_UpdateEnemies = new List<BattleRealEnemyController>();
        m_GotoPoolEnemies = new List<BattleRealEnemyController>();
        m_PoolEnemies = new Dictionary<string, LinkedList<GameObject>>();
    }

    public override void OnFinalize()
    {
        DestroyAllEnemy();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        var stageManager = BattleRealStageManager.Instance;
        m_EnemyEvacuationHolder = stageManager.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.ENEMY);
    }

    public override void OnUpdate()
    {
        // デバッグ用
        foreach (var enemy in m_ReservedRegisterEnemies)
        {
            Register(enemy);
        }

        m_ReservedRegisterEnemies.Clear();

        // Start
        foreach (var enemy in m_StandbyEnemies)
        {
            if (enemy == null)
            {
                //CheckPoolEnemy(enemy);
                continue;
            }

            enemy.OnStart();
        }

        GotoUpdateEnemy();

        // Update処理
        foreach (var enemy in m_UpdateEnemies)
        {
            if (enemy == null)
            {
                //CheckPoolEnemy(enemy);
                continue;
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
                //CheckPoolEnemy(enemy);
                continue;
            }

            enemy.OnLateUpdate();
        }
    }

    #endregion


    #region Impl IColliderProcess

    public void ClearColliderFlag()
    {
        foreach (var enemy in m_UpdateEnemies)
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
        foreach (var enemy in m_UpdateEnemies)
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
        foreach (var enemy in m_UpdateEnemies)
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
    public static void RegisterEnemy(BattleRealEnemyController enemy)
    {
        if (enemy == null || m_ReservedRegisterEnemies.Contains(enemy))
        {
            return;
        }

        m_ReservedRegisterEnemies.Add(enemy);
    }

    private void Register(BattleRealEnemyController enemy)
    {
        if (enemy == null || m_StandbyEnemies.Contains(enemy) || m_UpdateEnemies.Contains(enemy) || m_GotoPoolEnemies.Contains(enemy))
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
            m_UpdateEnemies.Add(enemy);
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
            m_UpdateEnemies.Remove(enemy);

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
    private GameObject GetPoolingEnemy(BattleRealEnemyGenerateParamSet generateParamSet, BattleRealEnemyBehaviorParamSet behaviorParamSet)
    {
        if (generateParamSet == null || behaviorParamSet == null)
        {
            return null;
        }

        var lookParamSet = behaviorParamSet.EnemyLookParamSet;
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
    private void CheckStandByEnemy(BattleRealEnemyController enemy)
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
    private void CheckPoolEnemy(BattleRealEnemyController enemy)
    {
        if (enemy == null || m_GotoPoolEnemies.Contains(enemy))
        {
            Debug.LogError("指定した敵を削除できませんでした。");
            return;
        }

        enemy.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_POOL);
        m_GotoPoolEnemies.Add(enemy);
    }

    /// <summary>
    /// 敵グループの生成リストから敵を新規作成する。
    /// </summary>
    public BattleRealEnemyController CreateEnemy(BattleRealEnemyGenerateParamSet generateParamSet, BattleRealEnemyBehaviorParamSet behaviorParamSet)
    {
        if (generateParamSet == null || behaviorParamSet == null)
        {
            return null;
        }

        Type behaviorType = null;
        try
        {
            behaviorType = Type.GetType(behaviorParamSet.BehaviorClass);
        }
        catch (Exception)
        {
            return null;
        }

        var enemyObj = GetPoolingEnemy(generateParamSet, behaviorParamSet);
        if (enemyObj == null)
        {
            return null;
        }

        var enemy = enemyObj.AddComponent(behaviorType) as BattleRealEnemyController;
        if (enemy == null)
        {
            GameObject.Destroy(enemyObj);
            return null;
        }

        var poolId = behaviorParamSet.EnemyLookParamSet.LookId;
        enemy.SetLookId(poolId);
        enemy.SetParamSet(generateParamSet, behaviorParamSet);
        CheckStandByEnemy(enemy);
        return enemy;
    }

    /// <summary>
    /// 敵キャラを破棄する。
    /// これを呼び出したタイミングの次のLateUpdateで破棄される。
    /// </summary>
    public void DestroyEnemy(BattleRealEnemyController enemy)
    {
        CheckPoolEnemy(enemy);
    }

    /// <summary>
    /// 全ての敵キャラを破棄する。
    /// これを呼び出したタイミングの次のLateUpdateで破棄される。
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
        minPos += m_ParamSet.MinOffsetFieldPosition;
        maxPos += m_ParamSet.MaxOffsetFieldPosition;

        var factX = (maxPos.x - minPos.x) * x + minPos.x;
        var factZ = (maxPos.y - minPos.y) * y + minPos.y;
        var pos = new Vector3(factX, ParamDef.BASE_Y_POS, factZ);

        return pos;
    }

    /// <summary>
    /// 敵が敵フィールドの範囲外に出ているかどうかを判定する。
    /// </summary>
    public bool IsOutOfField(BattleRealEnemyController enemy)
    {
        if (enemy == null)
        {
            return true;
        }

        var stageManager = BattleRealStageManager.Instance;
        var minPos = stageManager.MinLocalFieldPosition;
        var maxPos = stageManager.MaxLocalFieldPosition;
        minPos += m_ParamSet.MinOffsetFieldPosition;
        maxPos += m_ParamSet.MaxOffsetFieldPosition;

        var pos = enemy.transform.position;

        return pos.x < minPos.x || pos.x > maxPos.x || pos.z < minPos.y || pos.z > maxPos.y;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵の振る舞いの制御を行う。
/// </summary>
public class CommandEnemyCharaManager : BattleSingletonMonoBehavior<CommandEnemyCharaManager>
{
    [Header("Holder ")]

    [SerializeField]
    private Transform m_EnemyCharaHolder;

    [Header("Offset Field")]

    [SerializeField]
    private Vector2 m_OffsetMinField;

    [SerializeField]
    private Vector2 m_OffsetMaxField;


    #region Field

    /// <summary>
    /// STANDBY状態の弾を保持するリスト。
    /// </summary>
    [SerializeField]
    private List<CommandEnemyController> m_StandbyEnemies;

    /// <summary>
    /// UPDATE状態の弾を保持するリスト。
    /// </summary>
    [SerializeField]
    private List<CommandEnemyController> m_UpdateEnemies;

    /// <summary>
    /// UPDATE状態に遷移する弾のリスト。
    /// </summary>
    [SerializeField]
    private List<CommandEnemyController> m_GotoUpdateEnemies;

    /// <summary>
    /// POOL状態に遷移する弾のリスト。
    /// </summary>
    [SerializeField]
    private List<CommandEnemyController> m_GotoDestroyEnemies;

    /// <summary>
    /// ボスのオブジェクトのみを保持する。
    /// </summary>
    [SerializeField]
    private List<CommandEnemyController> m_BossControllers;

    #endregion



    #region Get Set

    /// <summary>
    /// スタンバイ状態の敵を取得する。
    /// </summary>
    public List<CommandEnemyController> GetStandbyEnemies()
    {
        return m_StandbyEnemies;
    }

    /// <summary>
    /// ゲームサイクルに入っている敵を取得する。
    /// </summary>
    public List<CommandEnemyController> GetUpdateEnemies()
    {
        return m_UpdateEnemies;
    }

    /// <summary>
    /// ステージ上の全てのボス敵を取得する。
    /// </summary>
    public List<CommandEnemyController> GetBossEnemies()
    {
        return m_BossControllers;
    }

    #endregion



    protected override void OnAwake()
    {
        base.OnAwake();

        m_StandbyEnemies = new List<CommandEnemyController>();
        m_UpdateEnemies = new List<CommandEnemyController>();
        m_GotoUpdateEnemies = new List<CommandEnemyController>();
        m_GotoDestroyEnemies = new List<CommandEnemyController>();
        m_BossControllers = new List<CommandEnemyController>();
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

        if (StageManager.Instance != null && StageManager.Instance.GetEnemyCharaHolder() != null)
        {
            m_EnemyCharaHolder = StageManager.Instance.GetEnemyCharaHolder().transform;
        }
        else if (m_EnemyCharaHolder == null)
        {
            var obj = new GameObject("[EnemyCharaHolder]");
            obj.transform.position = Vector3.zero;
            m_EnemyCharaHolder = obj.transform;
        }
    }

    public override void OnUpdate()
    {
        foreach (var enemy in m_StandbyEnemies)
        {
            if (enemy == null)
            {
                m_GotoUpdateEnemies.Add(enemy);
                continue;
            }

            enemy.OnStart();
            m_GotoUpdateEnemies.Add(enemy);
        }

        GotoUpdateFromStandby();

        foreach (var enemy in m_UpdateEnemies)
        {
            if (enemy == null)
            {
                m_GotoDestroyEnemies.Add(enemy);
                continue;
            }

            enemy.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
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

    private void GotoUpdateFromStandby()
    {
        int count = m_GotoUpdateEnemies.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var enemy = m_GotoUpdateEnemies[idx];

            if (enemy == null)
            {
                continue;
            }

            m_GotoUpdateEnemies.RemoveAt(idx);
            m_StandbyEnemies.Remove(enemy);
            m_UpdateEnemies.Add(enemy);
        }

        m_GotoUpdateEnemies.Clear();

        m_StandbyEnemies.RemoveAll((e) => e == null);
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
        if (controller == null || m_StandbyEnemies.Contains(controller) || m_UpdateEnemies.Contains(controller) || m_GotoUpdateEnemies.Contains(controller) || m_GotoDestroyEnemies.Contains(controller))
        {
            return null;
        }

        controller.transform.SetParent(m_EnemyCharaHolder);
        m_StandbyEnemies.Add(controller);
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
        m_GotoDestroyEnemies.AddRange(m_StandbyEnemies);
        m_GotoDestroyEnemies.AddRange(m_UpdateEnemies);
        m_GotoDestroyEnemies.AddRange(m_GotoUpdateEnemies);
        m_StandbyEnemies.Clear();
        m_UpdateEnemies.Clear();
        m_GotoUpdateEnemies.Clear();
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
        foreach (var enemy in m_StandbyEnemies)
        {
            DestroyEnemyImmediate(enemy);
        }

        foreach (var enemy in m_UpdateEnemies)
        {
            DestroyEnemyImmediate(enemy);
        }

        foreach (var enemy in m_GotoUpdateEnemies)
        {
            DestroyEnemyImmediate(enemy);
        }

        m_StandbyEnemies.Clear();
        m_UpdateEnemies.Clear();
        m_GotoUpdateEnemies.Clear();
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
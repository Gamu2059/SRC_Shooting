﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// リアルモードの敵キャラを管理する。
/// </summary>
[Serializable]
public class BattleRealEnemyGroupManager : Singleton<BattleRealEnemyGroupManager>
{
    #region Field

    private Transform m_EnemyGroupHolder;
    private Transform m_EnemyEvacuationHolder;

    //// <summary>
    /// STANDBY状態の敵グループを保持するリスト。
    /// </summary>
    private List<BattleRealEnemyGroupController> m_StandbyEnemyGroups;

    /// <summary>
    /// UPDATE状態の敵グループを保持するリスト。
    /// </summary>
    private List<BattleRealEnemyGroupController> m_UpdateEnemyGroups;

    /// <summary>
    /// POOL状態の敵グループを保持するリスト。
    /// </summary>
    private List<BattleRealEnemyGroupController> m_PoolEnemyGroups;

    /// <summary>
    /// 破棄状態に遷移する敵グループのリスト。
    /// </summary>
    private List<BattleRealEnemyGroupController> m_GotoPoolEnemyGroups;

    #endregion

    public static BattleRealEnemyGroupManager Builder()
    {
        var manager = Create();
        manager.OnInitialize();
        return manager;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StandbyEnemyGroups = new List<BattleRealEnemyGroupController>();
        m_UpdateEnemyGroups = new List<BattleRealEnemyGroupController>();
        m_PoolEnemyGroups = new List<BattleRealEnemyGroupController>();
        m_GotoPoolEnemyGroups = new List<BattleRealEnemyGroupController>();
    }

    public override void OnFinalize()
    {
        m_GotoPoolEnemyGroups.Clear();
        m_PoolEnemyGroups.Clear();
        m_UpdateEnemyGroups.Clear();
        m_StandbyEnemyGroups.Clear();

        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        var stageManager = BattleRealStageManager.Instance;
        m_EnemyGroupHolder = stageManager.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.ENEMY_GROUP);
    }

    public override void OnUpdate()
    {
        // Start処理
        foreach (var enemyGroup in m_StandbyEnemyGroups)
        {
            if (enemyGroup == null)
            {
                CheckPoolEnemyGroup(enemyGroup);
                continue;
            }

            enemyGroup.OnStart();
        }

        GotoUpdateEnemyGroup();

        // Update処理
        foreach (var enemyGroup in m_UpdateEnemyGroups)
        {
            if (enemyGroup == null)
            {
                CheckPoolEnemyGroup(enemyGroup);
                continue;
            }

            enemyGroup.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var enemyGroup in m_UpdateEnemyGroups)
        {
            if (enemyGroup == null)
            {
                CheckPoolEnemyGroup(enemyGroup);
                continue;
            }

            enemyGroup.OnLateUpdate();
        }
    }



    public override void OnFixedUpdate()
    {
        // FixedUpdate処理
        foreach (var enemyGroup in m_UpdateEnemyGroups)
        {
            if (enemyGroup == null)
            {
                CheckPoolEnemyGroup(enemyGroup);
                continue;
            }

            enemyGroup.OnFixedUpdate();
        }
    }

    #endregion

    /// <summary>
    /// 削除フラグが立っているものをプールに戻す
    /// </summary>
    public void GotoPool()
    {
        GotoPoolEnemyGroup();
    }

    /// <summary>
    /// UPDATE状態にする。
    /// </summary>
    private void GotoUpdateEnemyGroup()
    {
        foreach (var enemyGroup in m_StandbyEnemyGroups)
        {
            if (enemyGroup == null)
            {
                continue;
            }
            else if (enemyGroup.GetCycle() != E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                CheckPoolEnemyGroup(enemyGroup);
            }

            enemyGroup.SetCycle(E_POOLED_OBJECT_CYCLE.UPDATE);
            m_UpdateEnemyGroups.Add(enemyGroup);
        }

        m_StandbyEnemyGroups.Clear();
    }

    /// <summary>
    /// 削除フラグが立っている敵グループを一斉に削除する
    /// </summary>
    private void GotoPoolEnemyGroup()
    {
        int count = m_GotoPoolEnemyGroups.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var enemyGroup = m_GotoPoolEnemyGroups[idx];
            if (enemyGroup == null)
            {
                continue;
            }

            enemyGroup.OnFinalize();
            enemyGroup.SetCycle(E_POOLED_OBJECT_CYCLE.POOLED);
            enemyGroup.gameObject.SetActive(false);

            m_GotoPoolEnemyGroups.RemoveAt(idx);
            m_UpdateEnemyGroups.Remove(enemyGroup);
            m_PoolEnemyGroups.Add(enemyGroup);
        }

        m_GotoPoolEnemyGroups.Clear();
    }

    /// <summary>
    /// プールから敵グループを取得する。
    /// 足りなければ生成する。
    /// </summary>
    private BattleRealEnemyGroupController GetPoolingEnemyGroup(BattleRealEnemyGroupParam enemyGroupParam)
    {
        if (enemyGroupParam == null)
        {
            return null;
        }

        //var behaviorClass = groupGenerateParamSet.EnemyGroupBehaviorParamSet.BehaviorClass;
        //Type behaviorType = null;

        //try
        //{
        //    behaviorType = Type.GetType(behaviorClass);
        //}
        //catch (Exception)
        //{
        //    return null;
        //}

        //if (behaviorType == null)
        //{
        //    return null;
        //}

        BattleRealEnemyGroupController enemyGroup = null;

        for (int i = 0; i < m_PoolEnemyGroups.Count; i++)
        {
            var pooledGroup = m_PoolEnemyGroups[i];
            if (pooledGroup != null && pooledGroup is BattleRealEnemyGroupController)
            {
                enemyGroup = pooledGroup;
                break;
            }
        }

        if (enemyGroup == null)
        {
            var groupObj = new GameObject("EnemyGroup");
            enemyGroup = groupObj.AddComponent<BattleRealEnemyGroupController>();

            if (enemyGroup == null)
            {
                //Debug.LogError(behaviorClass + "は、BattleRealEnemyGroupControllerを継承していません。");
                GameObject.Destroy(groupObj);
                return null;
            }

            enemyGroup.transform.SetParent(m_EnemyGroupHolder);
            m_PoolEnemyGroups.Add(enemyGroup);
        }

        return enemyGroup;
    }

    /// <summary>
    /// 敵グループをSTANDBY状態にして制御下に入れる。
    /// </summary>
    private void CheckStandByEnemyGroup(BattleRealEnemyGroupController enemyGroup)
    {
        if (enemyGroup == null || !m_PoolEnemyGroups.Contains(enemyGroup))
        {
            Debug.LogError("指定された敵グループを追加できませんでした。");
            return;
        }

        m_PoolEnemyGroups.Remove(enemyGroup);
        m_StandbyEnemyGroups.Add(enemyGroup);
        enemyGroup.gameObject.SetActive(true);
        enemyGroup.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE);
        enemyGroup.OnInitialize();
    }

    /// <summary>
    /// 指定した敵グループを制御から外すためにチェックする。
    /// </summary>
    private void CheckPoolEnemyGroup(BattleRealEnemyGroupController enemyGroup)
    {
        if (enemyGroup == null || m_GotoPoolEnemyGroups.Contains(enemyGroup))
        {
            Debug.LogError("指定した敵グループを削除できませんでした。");
            return;
        }

        enemyGroup.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_POOL);
        m_GotoPoolEnemyGroups.Add(enemyGroup);
    }

    /// <summary>
    /// 敵グループの生成リストから敵を新規作成する。
    /// </summary>
    public void CreateEnemyGroup(BattleRealEnemyGroupParam enemyGroupParam)
    {
        if (enemyGroupParam == null)
        {
            return;
        }

        var enemyGroup = GetPoolingEnemyGroup(enemyGroupParam);
        if (enemyGroup == null)
        {
            return;
        }

        enemyGroup.SetParam(enemyGroupParam);
        CheckStandByEnemyGroup(enemyGroup);
    }

    /// <summary>
    /// 敵グループを破棄する。
    /// これを呼び出したタイミングの次のLateUpdateで破棄される。
    /// </summary>
    public void DestroyEnemyGroup(BattleRealEnemyGroupController enemyGroup)
    {
        CheckPoolEnemyGroup(enemyGroup);
    }

    /// <summary>
    /// 全ての敵グループを破棄する。
    /// これを呼び出したタイミングの次のLateUpdateで破棄される。
    /// </summary>
    public void DestroyAllEnemyGroup()
    {
        foreach (var enemyGroup in m_UpdateEnemyGroups)
        {
            DestroyEnemyGroup(enemyGroup);
        }

        m_UpdateEnemyGroups.Clear();
    }
}

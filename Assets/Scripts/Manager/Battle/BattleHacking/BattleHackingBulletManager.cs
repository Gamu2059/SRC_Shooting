﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// コマンドイベントの全ての弾の制御を管理する。
/// </summary>
[Serializable]
public class BattleHackingBulletManager : ControllableObject, IColliderProcess
{
    public static BattleHackingBulletManager Instance => BattleHackingManager.Instance.BulletManager;

    #region Field

    private BattleHackingBulletManagerParamSet m_ParamSet;

    private Transform m_BulletHolder;

    //// <summary>
    /// STANDBY状態の弾を保持するリスト。
    /// </summary>
    private List<BattleHackingBulletController> m_StandbyBullets;

    /// <summary>
    /// UPDATE状態の弾を保持するリスト。
    /// </summary>
    private List<BattleHackingBulletController> m_UpdateBullets;
    public List<BattleHackingBulletController> Bullets => m_UpdateBullets;

    /// <summary>
    /// POOL状態の弾を保持するリスト。
    /// </summary>
    private List<BattleHackingBulletController> m_PoolBullets;

    /// <summary>
    /// POOL状態に遷移する弾のリスト。
    /// </summary>
    private List<BattleHackingBulletController> m_GotoPoolBullets;

    #endregion

    #region Get

    /// <summary>
    /// UPDATE状態の弾を保持するリストを取得する。
    /// </summary>
    public List<BattleHackingBulletController> GetUpdateBullets()
    {
        return m_UpdateBullets;
    }

    #endregion

    public BattleHackingBulletManager(BattleHackingBulletManagerParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    #region Game Cycle

    /// <summary>
    /// マネージャが初期化される時に呼び出される。
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StandbyBullets = new List<BattleHackingBulletController>();
        m_UpdateBullets = new List<BattleHackingBulletController>();
        m_PoolBullets = new List<BattleHackingBulletController>();
        m_GotoPoolBullets = new List<BattleHackingBulletController>();
    }

    /// <summary>
    /// マネージャが破棄される時に呼び出される。
    /// </summary>
    public override void OnFinalize()
    {
        base.OnFinalize();
        CheckPoolAllBullet();
        m_StandbyBullets = null;
        m_UpdateBullets = null;
        m_PoolBullets = null;
        m_GotoPoolBullets = null;
    }

    public override void OnStart()
    {
        base.OnStart();

        m_BulletHolder = BattleHackingStageManager.Instance.GetHolder(BattleHackingStageManager.E_HOLDER_TYPE.BULLET);
    }

    public override void OnUpdate()
    {
        // Start処理
        foreach (var bullet in m_StandbyBullets)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.OnStart();
        }

        GotoUpdateFromStandby();

        // Update処理
        foreach (var bullet in m_UpdateBullets)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var bullet in m_UpdateBullets)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.OnLateUpdate();
        }
    }

    #endregion


    #region Impl IColliderProcess

    public void ClearColliderFlag()
    {
        foreach (var bullet in m_UpdateBullets)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.ClearColliderFlag();
        }
    }

    public void UpdateCollider()
    {
        foreach (var bullet in m_UpdateBullets)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.UpdateCollider();
        }
    }

    public void ProcessCollision()
    {
        foreach (var bullet in m_UpdateBullets)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.ProcessCollision();
        }
    }

    #endregion

    /// <summary>
    /// ハッキングモードの終了時の処理
    /// </summary>
    public void OnPutAway()
    {
        CheckPoolAllBullet();
    }

    /// <summary>
    /// 破棄フラグが立っているものをプールに戻す
    /// </summary>
    public void GotoPool()
    {
        GotoPoolFromUpdate();
    }

    /// <summary>
    /// UPDATE状態にする。
    /// </summary>
    private void GotoUpdateFromStandby()
    {
        foreach (var bullet in m_StandbyBullets)
        {
            if (bullet == null)
            {
                continue;
            }
            else if (bullet.GetCycle() != E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                CheckPoolBullet(bullet);
            }

            bullet.SetCycle(E_POOLED_OBJECT_CYCLE.UPDATE);
            m_UpdateBullets.Add(bullet);
        }

        m_StandbyBullets.Clear();
    }

    /// <summary>
    /// POOL状態にする。
    /// </summary>
    private void GotoPoolFromUpdate()
    {
        int count = m_GotoPoolBullets.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var bullet = m_GotoPoolBullets[idx];
            bullet.OnFinalize();
            bullet.SetCycle(E_POOLED_OBJECT_CYCLE.POOLED);
            bullet.gameObject.SetActive(false);
            m_GotoPoolBullets.RemoveAt(idx);
            m_UpdateBullets.Remove(bullet);
            m_PoolBullets.Add(bullet);
        }

        m_GotoPoolBullets.Clear();
    }

    /// <summary>
    /// 弾をSTANDBY状態にして制御下に入れる。
    /// </summary>
    public void CheckStandbyBullet(BattleHackingBulletController bullet)
    {
        if (bullet == null || !m_PoolBullets.Contains(bullet))
        {
            Debug.LogWarning("指定された弾を追加できませんでした。");
            return;
        }

        m_PoolBullets.Remove(bullet);
        m_StandbyBullets.Add(bullet);
        bullet.gameObject.SetActive(true);
        bullet.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE);
        bullet.OnInitialize();
    }

    /// <summary>
    /// 指定した弾を制御から外すためにチェックする。
    /// </summary>
    public void CheckPoolBullet(BattleHackingBulletController bullet)
    {
        if (bullet == null || m_GotoPoolBullets.Contains(bullet))
        {
            Debug.LogError("指定した弾を削除できませんでした。");
            return;
        }

        bullet.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_POOL);
        m_GotoPoolBullets.Add(bullet);
    }

    /// <summary>
    /// プールから弾を取得する。
    /// 足りなければ生成する。
    /// </summary>
    /// <param name="bulletPrefab">取得や生成の情報源となる弾のプレハブ</param>
    public BattleHackingBulletController GetPoolingBullet(BattleHackingBulletController bulletPrefab)
    {
        if (bulletPrefab == null)
        {
            return null;
        }

        string bulletId = bulletPrefab.GetBulletGroupId();
        BattleHackingBulletController bullet = null;

        foreach (var b in m_PoolBullets)
        {
            if (b != null && b.GetBulletGroupId() == bulletId)
            {
                bullet = b;
                break;
            }
        }

        if (bullet == null)
        {
            bullet = GameObject.Instantiate(bulletPrefab);
            bullet.transform.SetParent(m_BulletHolder);
            m_PoolBullets.Add(bullet);
        }

        return bullet;
    }

    /// <summary>
    /// 全ての弾をプールに戻す。
    /// </summary>
    private void CheckPoolAllBullet()
    {
        foreach (var bullet in m_StandbyBullets)
        {
            CheckPoolBullet(bullet);
        }
        m_StandbyBullets.Clear();

        foreach(var bullet in m_UpdateBullets)
        {
            CheckPoolBullet(bullet);
        }

        GotoPoolFromUpdate();
    }

    /// <summary>
    /// 弾が弾フィールドの範囲外に出ているかどうかを判定する。
    /// </summary>
    public bool IsOutOfBulletField(BattleHackingBulletController bullet)
    {
        if (bullet == null)
        {
            return true;
        }

        var stageManager = BattleRealStageManager.Instance;
        var minPos = stageManager.MinLocalFieldPosition;
        var maxPos = stageManager.MaxLocalFieldPosition;
        minPos += m_ParamSet.MinOffsetFieldPosition;
        maxPos += m_ParamSet.MaxOffsetFieldPosition;

        var pos = bullet.GetPosition();

        return pos.x < minPos.x || pos.x > maxPos.x || pos.z < minPos.y || pos.z > maxPos.y;
    }
}

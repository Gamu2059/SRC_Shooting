using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// コマンドイベントの全ての弾の制御を管理する。
/// </summary>
public class CommandBulletManager : BattleSingletonMonoBehavior<CommandBulletManager>
{
    public const string HOLDER_NAME = "[CommandBulletHolder]";

    #region Field

    private Transform m_BulletHolder;

    //// <summary>
    /// STANDBY状態の弾を保持するリスト。
    /// </summary>
    private List<CommandBulletController> m_StandbyBullets;

    /// <summary>
    /// UPDATE状態の弾を保持するリスト。
    /// </summary>
    private List<CommandBulletController> m_UpdateBullets;

    /// <summary>
    /// POOL状態の弾を保持するリスト。
    /// </summary>
    private List<CommandBulletController> m_PoolBullets;

    /// <summary>
    /// POOL状態に遷移する弾のリスト。
    /// </summary>
    private List<CommandBulletController> m_GotoPoolBullets;

    #endregion

    #region Get

    /// <summary>
    /// UPDATE状態の弾を保持するリストを取得する。
    /// </summary>
    public List<CommandBulletController> GetUpdateBullets()
    {
        return m_UpdateBullets;
    }

    #endregion

    /// <summary>
    /// マネージャが初期化される時に呼び出される。
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StandbyBullets = new List<CommandBulletController>();
        m_UpdateBullets = new List<CommandBulletController>();
        m_PoolBullets = new List<CommandBulletController>();
        m_GotoPoolBullets = new List<CommandBulletController>();
    }

    /// <summary>
    /// マネージャが破棄される時に呼び出される。
    /// </summary>
    public override void OnFinalize()
    {
        base.OnFinalize();
        CheckPoolAllBulletImmediate();
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

    /// <summary>
    /// コマンドイベントが有効になった時に呼び出される。
    /// </summary>
    public override void OnEnableObject()
    {
        base.OnEnableObject();
    }

    /// <summary>
    /// コマンドイベントが無効になった時に呼び出される。
    /// </summary>
    public override void OnDisableObject()
    {
        base.OnDisableObject();
        CheckPoolAllBulletImmediate();
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
            bullet.SetCycle(E_POOLED_OBJECT_CYCLE.POOLED);
            m_GotoPoolBullets.RemoveAt(idx);
            m_UpdateBullets.Remove(bullet);
            m_PoolBullets.Add(bullet);
        }

        m_GotoPoolBullets.Clear();
    }

    /// <summary>
    /// 弾をSTANDBY状態にして制御下に入れる。
    /// </summary>
    public void CheckStandbyBullet(CommandBulletController bullet)
    {
        if (bullet == null || !m_PoolBullets.Contains(bullet))
        {
            Debug.LogError("指定された弾を追加できませんでした。");
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
    public void CheckPoolBullet(CommandBulletController bullet)
    {
        if (bullet == null || m_GotoPoolBullets.Contains(bullet))
        {
            Debug.LogError("指定した弾を削除できませんでした。");
            return;
        }

        bullet.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_POOL);
        bullet.OnFinalize();
        m_GotoPoolBullets.Add(bullet);
        bullet.gameObject.SetActive(false);
    }

    /// <summary>
    /// プールから弾を取得する。
    /// 足りなければ生成する。
    /// </summary>
    /// <param name="bulletPrefab">取得や生成の情報源となる弾のプレハブ</param>
    public CommandBulletController GetPoolingBullet(CommandBulletController bulletPrefab)
    {
        if (bulletPrefab == null)
        {
            return null;
        }

        string bulletId = bulletPrefab.GetBulletGroupId();
        CommandBulletController bullet = null;

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
            bullet = Instantiate(bulletPrefab);
            bullet.transform.SetParent(m_BulletHolder);
            m_PoolBullets.Add(bullet);
        }

        return bullet;
    }

    /// <summary>
    /// 全ての弾をプールに戻す。
    /// </summary>
    private void CheckPoolAllBulletImmediate()
    {
        foreach(var bullet in m_UpdateBullets)
        {
            CheckPoolBullet(bullet);
        }

        GotoPoolFromUpdate();
    }
}

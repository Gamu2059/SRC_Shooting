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

    protected override void OnAwake()
    {
        base.OnAwake();

        m_UpdateBullets = new List<CommandBulletController>();
        m_PoolBullets = new List<CommandBulletController>();
        m_GotoPoolBullets = new List<CommandBulletController>();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
        m_UpdateBullets.Clear();
        m_PoolBullets.Clear();
    }

    public override void OnStart()
    {
        base.OnStart();

        if (CommandStageManager.Instance != null && CommandStageManager.Instance.GetBulletHolder() != null)
        {
            m_BulletHolder = CommandStageManager.Instance.GetBulletHolder().transform;
        }
        else if (m_BulletHolder == null)
        {
            var obj = new GameObject(HOLDER_NAME);
            obj.transform.position = Vector3.zero;
            m_BulletHolder = obj.transform;
        }
    }

    public override void OnUpdate()
    {
        // Update処理
        foreach (var bullet in m_UpdateBullets)
        {
            if (bullet == null)
            {
                continue;
            }

            if (bullet.GetCycle() == E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                bullet.OnStart();
                bullet.SetCycle(E_POOLED_OBJECT_CYCLE.UPDATE);
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
        m_UpdateBullets.Add(bullet);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// リアルモードの弾を管理する。
/// </summary>
[Serializable]
public class BattleRealBulletManager : ControllableObject
{
    public static BattleRealBulletManager Instance => BattleRealManager.Instance.BulletManager;

    #region Field

    private BattleRealBulletManagerParamSet m_ParamSet;

    private Transform m_BulletHolder;

    //// <summary>
    /// STANDBY状態の弾を保持するリスト。
    /// </summary>
    private List<BulletController> m_StandbyBullets;

    /// <summary>
    /// UPDATE状態の弾を保持するリスト。
    /// </summary>
    private List<BulletController> m_UpdateBullets;

    /// <summary>
    /// POOL状態の弾を保持するリスト。
    /// </summary>
    private List<BulletController> m_PoolBullets;

    /// <summary>
    /// POOL状態に遷移する弾のリスト。
    /// </summary>
    private List<BulletController> m_GotoPoolBullets;

    #endregion

    public BattleRealBulletManager(BattleRealBulletManagerParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StandbyBullets = new List<BulletController>();
        m_UpdateBullets = new List<BulletController>();
        m_PoolBullets = new List<BulletController>();
        m_GotoPoolBullets = new List<BulletController>(); ;
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
        m_StandbyBullets.Clear();
        m_UpdateBullets.Clear();
        m_PoolBullets.Clear();
    }

    public override void OnStart()
    {
        base.OnStart();

        m_BulletHolder = BattleRealStageManager.Instance.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.BULLET);
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
    public void CheckStandbyBullet(BulletController bullet)
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
    public void CheckPoolBullet(BulletController bullet)
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
    public BulletController GetPoolingBullet(BulletController bulletPrefab)
    {
        if (bulletPrefab == null)
        {
            return null;
        }

        string bulletId = bulletPrefab.GetBulletGroupId();
        BulletController bullet = null;

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
    /// 弾が弾フィールドの範囲外に出ているかどうかを判定する。
    /// </summary>
    public bool IsOutOfBulletField(BulletController bullet)
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

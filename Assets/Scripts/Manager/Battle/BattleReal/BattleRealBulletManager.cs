using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// リアルモードの弾を管理する。
/// </summary>
[Serializable]
public class BattleRealBulletManager : Singleton<BattleRealBulletManager>, IColliderProcess
{
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
    public List<BulletController> Bullets { get; private set; }

    /// <summary>
    /// POOL状態の弾を保持するリスト。
    /// </summary>
    private List<BulletController> m_PoolBullets;

    /// <summary>
    /// POOL状態に遷移する弾のリスト。
    /// </summary>
    private List<BulletController> m_GotoPoolBullets;

    /// <summary>
    /// UPDATE状態かつ、TROOPがPLAYERの弾を保持するリスト。
    /// </summary>
    public List<BulletController> PlayerBullets { get; private set; }

    /// <summary>
    /// UPDATE状態かつ、TROOPがENEMYの弾を保持するリスト。
    /// </summary>
    public List<BulletController> EnemyBullets { get; private set; }

    #endregion

    #region Open Callback

    public Action ToHackingAction { get; set; }
    public Action FromHackingAction { get; set; }

    #endregion

    public static BattleRealBulletManager Builder(BattleRealManager realManager, BattleRealBulletManagerParamSet param)
    {
        var manager = Create();
        manager.SetParam(param);
        manager.SetCallback(realManager);
        manager.OnInitialize();
        return manager;
    }

    private void SetParam(BattleRealBulletManagerParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    private void SetCallback(BattleRealManager manager)
    {
        manager.ChangeStateAction += OnChangeStateBattleRealManager;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_StandbyBullets = new List<BulletController>();
        Bullets = new List<BulletController>();
        m_PoolBullets = new List<BulletController>();
        m_GotoPoolBullets = new List<BulletController>();
        PlayerBullets = new List<BulletController>();
        EnemyBullets = new List<BulletController>();
    }

    public override void OnFinalize()
    {
        EnemyBullets.Clear();
        PlayerBullets.Clear();
        m_PoolBullets.Clear();
        Bullets.Clear();
        m_StandbyBullets.Clear();
        base.OnFinalize();
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
        foreach (var bullet in Bullets)
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
        foreach (var bullet in Bullets)
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
        foreach (var bullet in Bullets)
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
        foreach (var bullet in Bullets)
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
        foreach (var bullet in Bullets)
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
            Bullets.Add(bullet);

            switch (bullet.GetTroop())
            {
                case E_CHARA_TROOP.PLAYER:
                    PlayerBullets.Add(bullet);
                    break;
                case E_CHARA_TROOP.ENEMY:
                    EnemyBullets.Add(bullet);
                    break;
            }
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
            Bullets.Remove(bullet);
            m_PoolBullets.Add(bullet);

            switch (bullet.GetTroop())
            {
                case E_CHARA_TROOP.PLAYER:
                    PlayerBullets.Remove(bullet);
                    break;
                case E_CHARA_TROOP.ENEMY:
                    EnemyBullets.Remove(bullet);
                    break;
            }
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
    public bool IsOutOfField(BulletController bullet)
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

    /// <summary>
    /// 全ての敵の弾をプールに送る
    /// </summary>
    public void CheckPoolAllEnemyBullet()
    {
        foreach (var bullet in m_StandbyBullets)
        {
            if (bullet.GetTroop() == E_CHARA_TROOP.ENEMY)
            {
                CheckPoolBullet(bullet);
            }
        }

        foreach (var bullet in EnemyBullets)
        {
            CheckPoolBullet(bullet);
        }
    }

    /// <summary>
    /// 指定したキャラが撃った弾をプールに送る
    /// </summary>
    public void CheckPoolBullet(BattleRealCharaController targetOwner)
    {
        if (targetOwner == null)
        {
            return;
        }

        foreach (var bullet in m_StandbyBullets)
        {
            if (bullet == null)
            {
                continue;
            }

            if (bullet.GetBulletOwner() == targetOwner)
            {
                CheckPoolBullet(bullet);
            }
        }

        foreach (var bullet in Bullets)
        {
            if (bullet == null)
            {
                continue;
            }

            if (bullet.GetBulletOwner() == targetOwner)
            {
                CheckPoolBullet(bullet);
            }
        }
    }

    private void OnChangeStateBattleRealManager(E_BATTLE_REAL_STATE state)
    {
        switch(state)
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
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// キャラの制御を行うコンポーネント。
/// </summary>
public class BattleRealCharaController : BattleRealObjectBase
{
    #region Field Inspector


    [SerializeField, Tooltip("キャラが用いる弾の組み合わせ")]
    private BulletSetParam m_BulletSetParam;

    public float NowHp { get; private set; }
    public float MaxHp { get; private set; }

    #endregion

    #region Field

    public E_CHARA_TROOP Troop { get; protected set; }

    private HitSufferController<BulletController> m_BulletSuffer;
    private HitSufferController<BattleRealCharaController> m_CharaSuffer;
    private HitSufferController<BattleRealCharaController> m_CharaHit;
    private HitSufferController<BattleRealItemController> m_ItemHit;

    public List<ControllableMonoBehavior> AutoControlBehaviors { get; private set; }

    #endregion

    #region Get Set

    public BulletSetParam GetBulletSetParam()
    {
        return m_BulletSetParam;
    }

    /// <summary>
    /// いずれ消す
    /// </summary>
    /// <param name="param"></param>
    public void SetBulletSetParam(BulletSetParam param)
    {
        m_BulletSetParam = param;
    }

    public BulletParam GetBulletParam(int index = 0)
    {
        return m_BulletSetParam.GetBulletParam(index);
    }

    public int GetBulletPrefabsCount()
    {
        return m_BulletSetParam.GetBulletPrefabsCount();
    }

    #endregion

    #region Game Cycle

    protected override void OnAwake()
    {
        base.OnAwake();
        m_BulletSuffer = new HitSufferController<BulletController>();
        m_CharaSuffer = new HitSufferController<BattleRealCharaController>();
        m_CharaHit = new HitSufferController<BattleRealCharaController>();
        m_ItemHit = new HitSufferController<BattleRealItemController>();
    }

    protected override void OnDestroyed()
    {
        m_ItemHit?.OnFinalize();
        m_CharaHit?.OnFinalize();
        m_CharaSuffer?.OnFinalize();
        m_BulletSuffer?.OnFinalize();
        m_ItemHit = null;
        m_CharaHit = null;
        m_CharaSuffer = null;
        m_BulletSuffer = null;
        base.OnDestroyed();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        if (m_BulletSuffer == null)
        {
            m_BulletSuffer = new HitSufferController<BulletController>();
        }
        m_BulletSuffer.OnEnter = OnEnterSufferBullet;
        m_BulletSuffer.OnStay = OnStaySufferBullet;
        m_BulletSuffer.OnExit = OnExitSufferBullet;

        if (m_CharaSuffer == null)
        {
            m_CharaSuffer = new HitSufferController<BattleRealCharaController>();
        }
        m_CharaSuffer.OnEnter = OnEnterSufferChara;
        m_CharaSuffer.OnStay = OnStaySufferChara;
        m_CharaSuffer.OnExit = OnExitSufferChara;

        if (m_CharaHit == null)
        {
            m_CharaHit = new HitSufferController<BattleRealCharaController>();
        }
        m_CharaHit.OnEnter = OnEnterHitChara;
        m_CharaHit.OnStay = OnStayHitChara;
        m_CharaHit.OnExit = OnExitHitChara;

        if (m_ItemHit == null)
        {
            m_ItemHit = new HitSufferController<BattleRealItemController>();
        }
        m_ItemHit.OnEnter = OnEnterHitItem;
        m_ItemHit.OnStay = OnStayHitItem;
        m_ItemHit.OnExit = OnExitHitItem;
    }

    public override void OnFinalize()
    {
        AutoControlBehaviors.Clear();
        m_ItemHit.OnFinalize();
        m_CharaHit.OnFinalize();
        m_CharaSuffer.OnFinalize();
        m_BulletSuffer.OnFinalize();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        AutoControlBehaviors = new List<ControllableMonoBehavior>();
        var behaviors = GetComponents<ControllableMonoBehavior>();
        foreach (var b in behaviors)
        {
            if (b is IAutoControlOnCharaController)
            {
                AutoControlBehaviors.Add(b);
                b.OnStart();
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        foreach (var b in AutoControlBehaviors)
        {
            if (b is IAutoControlOnCharaController c && c.IsEnableController)
            {
                b.OnUpdate();
            }
        }
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        foreach (var b in AutoControlBehaviors)
        {
            if (b is IAutoControlOnCharaController c && c.IsEnableController)
            {
                b.OnLateUpdate();
            }
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        foreach (var b in AutoControlBehaviors)
        {
            if (b is IAutoControlOnCharaController c && c.IsEnableController)
            {
                b.OnFixedUpdate();
            }
        }
    }

    #endregion

    /// <summary>
    /// HPを初期化する
    /// </summary>
    /// <param name="hp">最大HP</param>
    public void InitHp(float hp)
    {
        MaxHp = NowHp = hp;
    }

    /// <summary>
    /// このキャラを回復する。
    /// </summary>
    public void Recover(float recover)
    {
        if (recover <= 0)
        {
            return;
        }

        NowHp = Mathf.Clamp(NowHp + recover, 0, MaxHp);
        OnRecover(recover);
    }

    protected virtual void OnRecover(float recover) { }

    /// <summary>
    /// このキャラにダメージを与える。
    /// HPが0になった場合は死ぬ。
    /// </summary>
    public void Damage(float damage)
    {
        if (damage <= 0)
        {
            return;
        }

        NowHp = Mathf.Clamp(NowHp - damage, 0, MaxHp);
        OnDamage(damage);

        if (NowHp == 0)
        {
            Dead();
        }
    }

    protected virtual void OnDamage(float damage) { }

    /// <summary>
    /// このキャラを死亡させる。
    /// </summary>
    public virtual void Dead() { }

    #region Impl IColliderProcess

    public override void ClearColliderFlag()
    {
        m_BulletSuffer.ClearUpdateFlag();
        m_CharaSuffer.ClearUpdateFlag();
        m_CharaHit.ClearUpdateFlag();
        m_ItemHit.ClearUpdateFlag();
    }

    public override void ProcessCollision()
    {
        m_BulletSuffer.ProcessCollision();
        m_CharaSuffer.ProcessCollision();
        m_CharaHit.ProcessCollision();
        m_ItemHit.ProcessCollision();
    }

    #endregion

    #region Suffer Bullet

    /// <summary>
    /// 他の弾から当てられた時の処理。
    /// </summary>
    /// <param name="attackBullet">他の弾</param>
    /// <param name="attackData">他の弾の衝突情報</param>
    /// <param name="targetData">このキャラの衝突情報</param>
    /// <param name="hitPosList">衝突座標リスト</param>
    public void SufferBullet(BulletController attackBullet, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_BulletSuffer.Put(attackBullet, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterSufferBullet(HitSufferData<BulletController> sufferData)
    {

    }

    protected virtual void OnStaySufferBullet(HitSufferData<BulletController> sufferData)
    {

    }

    protected virtual void OnExitSufferBullet(HitSufferData<BulletController> sufferData)
    {

    }

    #endregion

    #region Suffer Chara

    /// <summary>
    /// 他のキャラから当てられた時の処理。
    /// </summary>
    /// <param name="attackChara">他のキャラ</param>
    /// <param name="attackData">他のキャラの衝突情報</param>
    /// <param name="targetData">このキャラの衝突情報</param>
    /// <param name="hitPosList">衝突座標リスト</param>
    public void SufferChara(BattleRealCharaController attackChara, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_CharaSuffer.Put(attackChara, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterSufferChara(HitSufferData<BattleRealCharaController> sufferData)
    {
    }

    protected virtual void OnStaySufferChara(HitSufferData<BattleRealCharaController> sufferData)
    {
    }

    protected virtual void OnExitSufferChara(HitSufferData<BattleRealCharaController> sufferData)
    {
    }

    #endregion

    #region Hit Chara

    /// <summary>
    /// 他のキャラに当たった時の処理。
    /// </summary>
    /// <param name="targetChara">他のキャラ</param>
    /// <param name="attackData">このキャラの衝突情報</param>
    /// <param name="targetData">他のキャラの衝突情報</param>
    /// <param name="hitPosList">衝突座標リスト</param>
    public void HitChara(BattleRealCharaController targetChara, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_CharaHit.Put(targetChara, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterHitChara(HitSufferData<BattleRealCharaController> hitData)
    {

    }

    protected virtual void OnStayHitChara(HitSufferData<BattleRealCharaController> hitData)
    {

    }

    protected virtual void OnExitHitChara(HitSufferData<BattleRealCharaController> hitData)
    {

    }

    #endregion

    #region Hit Item

    /// <summary>
    /// 他のアイテムに当たった時の処理。
    /// </summary>
    /// <param name="targetItem">他のアイテム</param>
    /// <param name="attackData">このキャラの衝突情報</param>
    /// <param name="targetData">他のアイテムの衝突情報</param>
    /// <param name="hitPosList">衝突座標リスト</param>
    public void HitItem(BattleRealItemController targetItem, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_ItemHit.Put(targetItem, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterHitItem(HitSufferData<BattleRealItemController> hitData)
    {

    }

    protected virtual void OnStayHitItem(HitSufferData<BattleRealItemController> hitData)
    {

    }

    protected virtual void OnExitHitItem(HitSufferData<BattleRealItemController> hitData)
    {

    }

    #endregion

    /// <summary>
    /// キャラクタに付属しているコンポーネントを取得する。
    /// </summary>
    public T GetAutoController<T>() where T : ControllableMonoBehavior, IAutoControlOnCharaController
    {
        if (AutoControlBehaviors == null)
        {
            return null;
        }

        foreach (var b in AutoControlBehaviors)
        {
            if (b is T c)
            {
                return c;
            }
        }

        return null;
    }
}

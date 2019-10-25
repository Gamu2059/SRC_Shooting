﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハッキングモードのプレイヤーコントローラ
/// </summary>
public class BattleHackingPlayerController : CommandCharaController
{
    public const string OUT_WALL_COLLIDE_NAME = "OUT WALL COLLIDE";
    public const string CRITICAL_COLLIDE_NAME = "CRITICAL COLLIDE";

    [SerializeField, Tooltip("弾を撃つ間隔")]
    private float m_ShotInterval;

    [SerializeField, Tooltip("弾を撃つ基準点")]
    private Transform[] m_ShotPositions;

    private float m_ShotTimeCount;

    public override void OnStart()
    {
        base.OnStart();

        m_ShotTimeCount = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_ShotTimeCount -= Time.deltaTime;
    }

    /// <summary>
    /// 通常弾を発射する。
    /// このメソッドをオーバーロードしてそれぞれのキャラ固有の処理を記述して下さい。
    /// </summary>
    public virtual void ShotBullet()
    {
        if (m_ShotTimeCount > 0)
        {
            return;
        }

        if (m_ShotPositions == null)
        {
            return;
        }

        var shotParam = new CommandBulletShotParam(this);
        for (int i = 0; i < m_ShotPositions.Length; i++)
        {
            shotParam.Position = m_ShotPositions[i].position - transform.parent.position;
            CommandBulletController.ShotBullet(shotParam);
        }

        m_ShotTimeCount = m_ShotInterval;
    }

    protected override void OnEnterSufferBullet(HitSufferData<CommandBulletController> sufferData)
    {
        base.OnEnterSufferBullet(sufferData);

        var selfColliderType = sufferData.SufferCollider.Transform.ColliderType;
        if (selfColliderType == E_COLLIDER_TYPE.CRITICAL)
        {
            Damage(1);
        }
    }

    protected override void OnEnterSufferChara(HitSufferData<CommandCharaController> sufferData)
    {
        base.OnEnterSufferChara(sufferData);

        var selfColliderType = sufferData.SufferCollider.Transform.ColliderType;
        if (selfColliderType == E_COLLIDER_TYPE.CRITICAL)
        {
            Damage(1);
        }
    }

    public override void Dead()
    {
        if (BattleManager.Instance.m_PlayerNotDead)
        {
            return;
        }

        base.Dead();
        BattleHackingManager.Instance.DeadPlayer();
    }
}

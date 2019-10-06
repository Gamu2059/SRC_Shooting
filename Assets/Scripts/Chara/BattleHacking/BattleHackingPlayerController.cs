using System.Collections;
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
    private Transform m_ShotPosition;

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
        //if (m_ShotTimeCount > 0)
        //{
        //    return;
        //}

        //var shotParam = new CommandBulletShotParam(this);
        //if (m_ShotPosition != null)
        //{
        //    shotParam.Position = m_ShotPosition.position - transform.parent.position;
        //}

        //CommandBulletController.ShotBullet(shotParam);
        //m_ShotTimeCount = m_ShotInterval;
    }

    public override void SufferBullet(CommandBulletController attackBullet, ColliderData attackData, ColliderData targetData)
    {
        if (targetData.CollideName == CRITICAL_COLLIDE_NAME)
        {
            base.SufferBullet(attackBullet, attackData, targetData);
        }
    }

    public override void SufferChara(CommandCharaController attackChara, ColliderData attackData, ColliderData targetData)
    {
        if (targetData.CollideName == CRITICAL_COLLIDE_NAME)
        {
            base.SufferChara(attackChara, attackData, targetData);
        }
    }

    public override void Dead()
    {
        if (BattleManager.Instance.m_PlayerNotDead)
        {
            return;
        }

        base.Dead();

        BattleHackingManager.Instance.RequestChangeState(E_BATTLE_HACKING_STATE.GAME_OVER);
    }
}

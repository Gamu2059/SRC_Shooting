using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPlayerController : CommandCharaController
{
    public const string OUT_WALL_COLLIDE_NAME = "OUT WALL COLLIDE";
    public const string CRITICAL_COLLIDE_NAME = "CRITICAL COLLIDE";

    [SerializeField, Tooltip("キャラの移動速度")]
    private float m_MoveSpeed = 5f;

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
        if (m_ShotTimeCount > 0)
        {
            return;
        }

        var shotParam = new CommandBulletShotParam(this);
        if (m_ShotPosition != null)
        {
            shotParam.Position = m_ShotPosition.position - transform.parent.position;
        }

        CommandBulletController.ShotBullet(shotParam);
        m_ShotTimeCount = m_ShotInterval;
    }

    /// <summary>
    /// キャラを移動させる。
    /// 移動速度はキャラに現在設定されているものとなる。
    /// </summary>
    /// <param name="moveDirection"> 移動方向 </param>
    public virtual void Move(Vector3 moveDirection)
    {
        Vector3 move = moveDirection.normalized * m_MoveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }

    public override void SufferWall(CommandWallController attackWall, ColliderData attackData, ColliderData targetData)
    {
        if (attackData.CollideName == OUT_WALL_COLLIDE_NAME)
        {
            return;
        }

        if (targetData.CollideName == CRITICAL_COLLIDE_NAME)
        {
            base.SufferWall(attackWall, attackData, targetData);
        }
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
        BattleManager.Instance.TransitionBattleMain();
    }
}

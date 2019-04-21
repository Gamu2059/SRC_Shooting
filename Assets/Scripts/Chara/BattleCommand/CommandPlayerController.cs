using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPlayerController : CommandCharaController
{
    public const string OUT_WALL_COLLIDE_NAME = "OUT WALL COLLIDE";

    [SerializeField, Tooltip("キャラの移動速度")]
    private float m_MoveSpeed = 5f;

    [SerializeField, Tooltip("弾を撃つ間隔")]
    private float m_ShotInterval;

    private float m_ShotTimeCount;

    private void Start()
    {
        // 開発時専用で、自動的にマネージャにキャラを追加するためにUnityのStartを用いています
        CommandPlayerCharaManager.Instance.RegistChara(this);
    }

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

        CommandBulletController.ShotBullet(this);
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
        if (attackData.CollideName != OUT_WALL_COLLIDE_NAME)
        {
            base.SufferWall(attackWall, attackData, targetData);
        }
    }

    public override void Dead()
    {
        //if (BattleManager.Instance.m_PlayerNotDead)
        //{
        //    return;
        //}

        //base.Dead();

        //gameObject.SetActive(false);
        //BattleManager.Instance.GameOver();

        Debug.Log("Dead");
    }
}

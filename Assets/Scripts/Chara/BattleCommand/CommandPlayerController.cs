using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPlayerController : CommandCharaController
{
    [SerializeField, Tooltip("キャラの移動速度")]
    private float m_MoveSpeed = 5f;



    public float GetMoveSpeed()
    {
        return m_MoveSpeed;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        m_MoveSpeed = moveSpeed;
    }



    private void Start()
    {
        // 開発時専用で、自動的にマネージャにキャラを追加するためにUnityのStartを用いています
        CommandPlayerCharaManager.Instance.RegistChara(this);
    }

    /// <summary>
    /// 通常弾を発射する。
    /// このメソッドをオーバーロードしてそれぞれのキャラ固有の処理を記述して下さい。
    /// </summary>
    public virtual void ShotBullet()
    {
        // 何もオーバーロードしない場合は適当に弾を飛ばす
        CommandBulletController.ShotBullet(this);
    }

    /// <summary>
    /// ボムを使用する。
    /// </summary>
    public virtual void ShotBomb()
    {

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


    public override void OnSuffer(BulletController bullet, ColliderData colliderData)
    {
        base.OnSuffer(bullet, colliderData);
    }

    public override void OnSufferChara(CharaController hitChara, ColliderData hitData, ColliderData sufferData)
    {
        base.OnSufferChara(hitChara, hitData, sufferData);
        Damage(1);
    }

    public override void OnHitItem(ItemController sufferItem, ColliderData hitData, ColliderData sufferData)
    {
        //base.OnHitItem(sufferItem, hitData, sufferData);

        //if (sufferData.CollideName != ItemController.GAIN_COLLIDE)
        //{
        //    return;
        //}

        //switch (sufferItem.GetItemType())
        //{
        //    case E_ITEM_TYPE.SMALL_SCORE:
        //    case E_ITEM_TYPE.BIG_SCORE:
        //        BattleManager.Instance.AddScore(sufferItem.GetPoint());
        //        break;
        //    case E_ITEM_TYPE.SMALL_SCORE_UP:
        //    case E_ITEM_TYPE.BIG_SCORE_UP:
        //        break;
        //    case E_ITEM_TYPE.SMALL_EXP:
        //    case E_ITEM_TYPE.BIG_EXP:
        //        break;
        //    case E_ITEM_TYPE.SMALL_BOMB:
        //    case E_ITEM_TYPE.BIG_BOMB:
        //        break;
        //}
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントの弾やキャラの当たり判定を管理する。
/// </summary>
public class CommandCollisionManager : SingletonMonoBehavior<CommandCollisionManager>
{
    public override void OnUpdate()
    {
        UpdateColliderData();
        CheckCollision();
    }

    /// <summary>
    /// 衝突情報を更新する。
    /// </summary>
    public void UpdateColliderData()
    {
        var player = CommandPlayerCharaManager.Instance.GetController();
        var enemies = CommandEnemyCharaManager.Instance.GetUpdateEnemies();
        var bullets = CommandBulletManager.Instance.GetUpdateBullets();

        player.UpdateColliderData();

        foreach (var enemy in enemies)
        {
            enemy.UpdateColliderData();
        }

        foreach (var bullet in bullets)
        {
            bullet.UpdateColliderData();
        }
    }

    /// <summary>
    /// 衝突判定を行う。
    /// </summary>
    public void CheckCollision()
    {
        CheckCollisionBulletToBullet();
        CheckCollisionBulletToChara();
        CheckCollisionEnemyToPlayer();
        CheckCollisionPlayerToItem();
    }

    /// <summary>
    /// 弾から弾への衝突判定を行う。
    /// </summary>
    private void CheckCollisionBulletToBullet()
    {
        var bullets = BulletManager.Instance.GetUpdateBullets();

        foreach (var bullet in bullets)
        {
            if (!bullet.CanHitBullet())
            {
                continue;
            }

            foreach (var targetBullet in bullets)
            {
                if (bullet == targetBullet || bullet.GetTroop() == targetBullet.GetTroop())
                {
                    continue;
                }

                Collision.CheckCollide(bullet, targetBullet, (attackData, targetData) => {
                    targetBullet.SufferBullet(bullet, attackData, targetData);
                    bullet.HitBullet(targetBullet, attackData, targetData);
                });
            }
        }
    }

    /// <summary>
    /// 弾からキャラへの衝突判定を行う。
    /// </summary>
    private void CheckCollisionBulletToChara()
    {
        var bullets = BulletManager.Instance.GetUpdateBullets();
        var player = PlayerCharaManager.Instance.GetCurrentController();
        var enemies = EnemyCharaManager.Instance.GetUpdateEnemies();

        foreach (var bullet in bullets)
        {
            if (bullet.GetTroop() == E_CHARA_TROOP.ENEMY)
            {
                Collision.CheckCollide(bullet, player, (attackData, targetData) => {
                    Debug.Log(111);
                    player.SufferBullet(bullet, attackData, targetData);
                    bullet.HitChara(player, attackData, targetData);
                });
            }
            else
            {
                foreach (var enemy in enemies)
                {
                    Collision.CheckCollide(bullet, enemy, (attackData, targetData) => {
                        Debug.Log(222);
                        enemy.SufferBullet(bullet, attackData, targetData);
                        bullet.HitChara(enemy, attackData, targetData);
                    });
                }
            }
        }
    }

    /// <summary>
    /// 敵キャラからプレイヤーキャラへの衝突判定を行う。
    /// </summary>
    private void CheckCollisionEnemyToPlayer()
    {
        var player = PlayerCharaManager.Instance.GetCurrentController();
        var enemies = EnemyCharaManager.Instance.GetUpdateEnemies();

        foreach (var enemy in enemies)
        {
            Collision.CheckCollide(enemy, player, (attackData, targetData) => {
                player.SufferChara(enemy, attackData, targetData);
                enemy.HitChara(player, attackData, targetData);
            });
        }
    }

    /// <summary>
    /// プレイヤーキャラからアイテムへの衝突判定を行う。
    /// </summary>
    private void CheckCollisionPlayerToItem()
    {
        var player = PlayerCharaManager.Instance.GetCurrentController();
        var items = ItemManager.Instance.GetUpdateItems();

        foreach (var item in items)
        {
            Collision.CheckCollide(player, item, (attackData, targetData) => {
                item.SufferChara(player, attackData, targetData);
                player.HitItem(item, attackData, targetData);
            });
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾やキャラの当たり判定を管理する。
/// </summary>
public class CollisionManager : BattleSingletonMonoBehavior<CollisionManager>
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
        var player = PlayerCharaManager.Instance.GetCurrentController();
        var enemies = EnemyCharaManager.Instance.GetUpdateEnemies();
        var bullets = BulletManager.Instance.GetUpdateBullets();
        var items = ItemManager.Instance.GetUpdateItems();

        player.UpdateColliderData();
        enemies.ForEach(e => e.UpdateColliderData());
        bullets.ForEach(b => b.UpdateColliderData());
        items.ForEach(i => i.UpdateColliderData());
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

            // UPDATE状態にないものは飛ばす
            if (bullet.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                continue;
            }

            foreach (var targetBullet in bullets)
            {

                if (bullet == targetBullet || bullet.GetTroop() == targetBullet.GetTroop())
                {
                    continue;
                }

                // UPDATE状態にないものは飛ばす
                if (targetBullet.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
                {
                    continue;
                }

                Collision.CheckCollide(bullet, targetBullet, (attackData, targetData) =>
                {
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
            // UPDATE状態にないものは飛ばす
            if (bullet.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                continue;
            }

            if (bullet.GetTroop() == E_CHARA_TROOP.ENEMY)
            {
                Collision.CheckCollide(bullet, player, (attackData, targetData) =>
                {
                    player.SufferBullet(bullet, attackData, targetData);
                    bullet.HitChara(player, attackData, targetData);
                });
            }
            else
            {
                foreach (var enemy in enemies)
                {
                    // UPDATE状態にないものは飛ばす
                    if (enemy.GetCycle() != E_OBJECT_CYCLE.UPDATE)
                    {
                        continue;
                    }

                    Collision.CheckCollide(bullet, enemy, (attackData, targetData) =>
                    {
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
            // UPDATE状態にないものは飛ばす
            if (enemy.GetCycle() != E_OBJECT_CYCLE.UPDATE)
            {
                continue;
            }

            Collision.CheckCollide(enemy, player, (attackData, targetData) =>
            {
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
            // UPDATE状態にないものは飛ばす
            if (item.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                continue;
            }

            Collision.CheckCollide(player, item, (attackData, targetData) =>
            {
                item.SufferChara(player, attackData, targetData);
                player.HitItem(item, attackData, targetData);
            });
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealCollisionManager : BattleCollisionManagerBase<BattleRealCollisionManager>
{
    public static BattleRealCollisionManager Builder()
    {
        var manager = Create();
        manager.OnInitialize();
        return manager;
    }

    /// <summary>
    /// 衝突をチェックする。
    /// </summary>
    public override void CheckCollision()
    {
        CheckCollisionBulletToChara();
        CheckEnemyToPlayer();
        CheckPlayerToItem();
        CheckBulletToBullet();
    }

    /// <summary>
    /// 衝突情報を描画する。
    /// </summary>
    public override void DrawCollider()
    {
        return;
        //if (!BattleManager.Instance.m_IsDrawColliderArea)
        //{
        //    return;
        //}

        var player = BattleRealPlayerManager.Instance.Player;
        DrawCollider(player);

        var enemies = BattleRealEnemyManager.Instance.Enemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            DrawCollider(enemies[i]);
        }

        var bullets = BattleRealBulletManager.Instance.Bullets;
        for (int i = 0; i < bullets.Count; i++)
        {
            DrawCollider(bullets[i]);
        }

        var items = BattleRealItemManager.Instance.Items;
        for (int i = 0; i < items.Count; i++)
        {
            DrawCollider(items[i]);
        }
    }

    protected override Vector2 CalcViewportPos(Vector2 worldPos)
    {
        if (BattleRealStageManager.Instance == null)
        {
            return Vector2.one / 2f;
        }

        return BattleRealStageManager.Instance.CalcViewportPosFromWorldPosition(worldPos.x, worldPos.y, true);
    }

    /// <summary>
    /// 弾からキャラへの衝突判定を行う。
    /// </summary>
    private void CheckCollisionBulletToChara()
    {
        var bullets = BattleRealBulletManager.Instance.Bullets;
        var player = BattleRealPlayerManager.Instance.Player;
        var enemies = BattleRealEnemyManager.Instance.Enemies;

        foreach (var bullet in bullets)
        {
            // UPDATE状態にないものは飛ばす
            if (bullet.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                continue;
            }

            if (bullet.GetTroop() == E_CHARA_TROOP.ENEMY)
            {
                Collision.CheckCollide(bullet, player, (attackData, targetData, hitPosList) =>
                {
                    attackData.IsCollide = true;
                    targetData.IsCollide = true;
                    player.SufferBullet(bullet, attackData, targetData, hitPosList);
                    bullet.HitChara(player, attackData, targetData, hitPosList);
                });
            }
            else
            {
                foreach (var enemy in enemies)
                {
                    // UPDATE状態にないものは飛ばす
                    if (enemy.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
                    {
                        continue;
                    }

                    Collision.CheckCollide(bullet, enemy, (attackData, targetData, hitPosList) =>
                    {
                        attackData.IsCollide = true;
                        targetData.IsCollide = true;
                        enemy.SufferBullet(bullet, attackData, targetData, hitPosList);
                        bullet.HitChara(enemy, attackData, targetData, hitPosList);
                    });
                }
            }
        }
    }

    /// <summary>
    /// 敵からプレイヤーへの衝突判定を行う。
    /// </summary>
    private void CheckEnemyToPlayer()
    {
        var player = BattleRealPlayerManager.Instance.Player;
        var enemies = BattleRealEnemyManager.Instance.Enemies;

        foreach (var enemy in enemies)
        {
            // UPDATE状態にないものは飛ばす
            if (enemy.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                return;
            }

            Collision.CheckCollide(enemy, player, (attackData, targetData, hitPosList) =>
            {
                attackData.IsCollide = true;
                targetData.IsCollide = true;
                player.SufferChara(player, attackData, targetData, hitPosList);
                enemy.HitChara(enemy, attackData, targetData, hitPosList);
            });

            // ボスならプレイヤーからの被弾も在り得る
            if (enemy.IsBoss)
            {
                Collision.CheckCollide(player, enemy, (attackData, targetData, hitPosList) =>
                {
                    attackData.IsCollide = true;
                    targetData.IsCollide = true;
                    enemy.SufferChara(player, attackData, targetData, hitPosList);
                    player.HitChara(enemy, attackData, targetData, hitPosList);
                });
            }
        }
    }

    /// <summary>
    /// プレイヤーからアイテムへの衝突判定を行う。
    /// </summary>
    private void CheckPlayerToItem()
    {
        var items = BattleRealItemManager.Instance.Items;
        var player = BattleRealPlayerManager.Instance.Player;

        foreach (var item in items)
        {
            // UPDATE状態にないものは飛ばす
            if (item.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                return;
            }

            Collision.CheckCollide(player, item, (attackData, targetData, hitPosList) =>
            {
                attackData.IsCollide = true;
                targetData.IsCollide = true;
                item.SufferChara(player, attackData, targetData, hitPosList);
                player.HitItem(item, attackData, targetData, hitPosList);
            });
        }
    }

    /// <summary>
    /// 弾から弾への衝突判定を行う。
    /// </summary>
    private void CheckBulletToBullet()
    {
        var playerBullets = BattleRealBulletManager.Instance.PlayerBullets;
        var enemyBullets = BattleRealBulletManager.Instance.EnemyBullets;

        // プレイヤーの弾から敵の弾へ
        // 基本的に弾から弾へ当たるシチュエーションは、プレイヤーの弾から敵の弾へというものしかないはず
        foreach (var playerBullet in playerBullets)
        {
            // UPDATE状態にないものは飛ばす
            if (playerBullet.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE) {
                continue;
            }

            // 当たらないなら飛ばす
            if (!playerBullet.CanHitBullet())
            {
                continue;
            }

            foreach (var enemyBullet in enemyBullets)
            {
                // UPDATE状態にないものは飛ばす
                if (enemyBullet.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
                {
                    continue;
                }

                Collision.CheckCollide(playerBullet, enemyBullet, (attackData, targetData, hitPosList) =>
                {
                    attackData.IsCollide = true;
                    targetData.IsCollide = true;
                    enemyBullet.SufferBullet(playerBullet, attackData, targetData, hitPosList);
                    playerBullet.HitBullet(enemyBullet, attackData, targetData, hitPosList);
                });
            }
        }
    }
}

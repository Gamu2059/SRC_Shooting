using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingCollisionManager : BattleCollisionManagerBase
{
    public static BattleHackingCollisionManager Instance => BattleHackingManager.Instance.CollisionManager;

    /// <summary>
    /// 衝突をチェックする。
    /// </summary>
    public override void CheckCollision()
    {
        CheckCollisionBulletToChara();
        CheckEnemyToPlayer();
    }

    /// <summary>
    /// 衝突情報を描画する。
    /// </summary>
    public override void DrawCollider()
    {
        if (!BattleManager.Instance.m_IsDrawColliderArea)
        {
            return;
        }

        var player = BattleHackingPlayerManager.Instance.Player;
        DrawCollider(player);

        var enemies = BattleHackingEnemyManager.Instance.Enemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            DrawCollider(enemies[i]);
        }

        var bullets = BattleHackingBulletManager.Instance.Bullets;
        for (int i = 0; i < bullets.Count; i++)
        {
            DrawCollider(bullets[i]);
        }
    }

    protected override Vector2 CalcViewportPos(Vector2 worldPos)
    {
        if (BattleManager.Instance == null || BattleHackingStageManager.Instance == null)
        {
            return Vector2.one / 2f;
        }

        return BattleHackingStageManager.Instance.CalcViewportPosFromWorldPosition(worldPos.x, worldPos.y);
    }

    /// <summary>
    /// 弾からキャラへの衝突判定を行う。
    /// </summary>
    private void CheckCollisionBulletToChara()
    {
        var bullets = BattleHackingBulletManager.Instance.Bullets;
        var player = BattleHackingPlayerManager.Instance.Player;
        var enemies = BattleHackingEnemyManager.Instance.Enemies;

        for (int i = 0; i < bullets.Count; i++)
        {
            var bullet = bullets[i];

            // UPDATE状態にないものは飛ばす
            if (bullet.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                continue;
            }

            if (bullet.GetTroop() == E_CHARA_TROOP.ENEMY)
            {
                Collision.CheckCollideFast(bullet, player, (attackData, targetData) =>
                {
                    attackData.IsCollide = true;
                    targetData.IsCollide = true;
                    player.SufferBullet(bullet, attackData, targetData, null);
                    bullet.HitChara(player, attackData, targetData, null);
                });
            }
            else
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    var enemy = enemies[j];

                    // UPDATE状態にないものは飛ばす
                    if (enemy.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
                    {
                        continue;
                    }

                    Collision.CheckCollideFast(bullet, enemy, (attackData, targetData) =>
                    {
                        attackData.IsCollide = true;
                        targetData.IsCollide = true;
                        enemy.SufferBullet(bullet, attackData, targetData, null);
                        bullet.HitChara(enemy, attackData, targetData, null);
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
        var player = BattleHackingPlayerManager.Instance.Player;
        var enemies = BattleHackingEnemyManager.Instance.Enemies;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            // UPDATE状態にないものは飛ばす
            if (enemy.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                return;
            }

            Collision.CheckCollideFast(enemy, player, (attackData, targetData) =>
            {
                attackData.IsCollide = true;
                targetData.IsCollide = true;
                player.SufferChara(player, attackData, targetData, null);
                enemy.HitChara(enemy, attackData, targetData, null);
            });
        }
    }
}

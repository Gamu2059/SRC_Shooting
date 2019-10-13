using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealCollisionManager : BattleCollisionManagerBase, IUpdateCollider
{
    public static BattleRealCollisionManager Instance => BattleRealManager.Instance.CollisionManager;

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public void UpdateCollider()
    {
        BattleRealPlayerManager.Instance.UpdateCollider();
        BattleRealEnemyManager.Instance.UpdateCollider();
        BattleRealBulletManager.Instance.UpdateCollider();
    }

    /// <summary>
    /// 衝突をチェックする。
    /// </summary>
    public void CheckCollision()
    {
        CheckCollisionBulletToChara();
        CheckEnemyToPlayer();
    }

    /// <summary>
    /// 衝突情報を描画する。
    /// </summary>
    public void DrawCollider()
    {
        if (!BattleManager.Instance.m_IsDrawColliderArea)
        {
            return;
        }

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
    }

    protected override Vector2 CalcViewportPos(Vector2 worldPos)
    {
        if (BattleManager.Instance == null || BattleRealStageManager.Instance == null)
        {
            return Vector2.one / 2f;
        }

        return BattleRealStageManager.Instance.CalcViewportPosFromWorldPosition(worldPos.x, worldPos.y);
    }



    /// <summary>
    /// 弾からキャラへの衝突判定を行う。
    /// </summary>
    private void CheckCollisionBulletToChara()
    {
        var bullets = BattleRealBulletManager.Instance.Bullets;
        var player = BattleRealPlayerManager.Instance.Player;
        var enemies = BattleRealEnemyManager.Instance.Enemies;

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
                Collision.CheckCollide(bullet, player, (attackData, targetData, hitPosList) =>
                {
                    player.SufferBullet(bullet, attackData, targetData);
                    bullet.HitChara(player, attackData, targetData);
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

                    Collision.CheckCollide(bullet, enemy, (attackData, targetData, hitPosList) =>
                    {
                        attackData.IsCollide = true;
                        targetData.IsCollide = true;
                        enemy.SufferBullet(bullet, attackData, targetData);
                        bullet.HitChara(enemy, attackData, targetData);
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

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            // UPDATE状態にないものは飛ばす
            if (enemy.GetCycle() != E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                return;
            }

            Collision.CheckCollide(enemy, player, (attackData, targetData, hitPosList) =>
            {
                attackData.IsCollide = true;
                targetData.IsCollide = true;
                enemy.SufferChara(player, attackData, targetData);
                player.HitChara(enemy, attackData, targetData);
            });
        }
    }
}

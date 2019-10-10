using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealCollisionManager : BattleCollisionManagerBase, IUpdateCollider
{
    public static BattleRealCollisionManager Instance => BattleRealManager.Instance.CollisionManager;

    public void UpdateCollider()
    {
        BattleRealEnemyManager.Instance.UpdateCollider();
        BattleRealBulletManager.Instance.UpdateCollider();
    }

    public void CheckCollision()
    {
        CheckCollisionBulletToChara();
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
                Collision.CheckCollide(bullet, player, (attackData, targetData) =>
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

                    Collision.CheckCollide(bullet, enemy, (attackData, targetData) =>
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
}

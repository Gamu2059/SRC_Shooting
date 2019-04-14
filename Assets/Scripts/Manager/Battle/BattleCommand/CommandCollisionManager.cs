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
        CheckCollisionBulletToWall();
        CheckCollisionWallToPlayer();
        CheckCollisionBulletToChara();
        CheckCollisionEnemyToPlayer();
    }

    /// <summary>
    /// 弾から壁への衝突判定を行う。
    /// </summary>
    private void CheckCollisionBulletToWall()
    {
        var bullets = CommandBulletManager.Instance.GetUpdateBullets();
        var walls = CommandWallManager.Instance.GetUpdateWalls();

        foreach (var bullet in bullets)
        {
            foreach (var wall in walls)
            {
                Collision.CheckCollide(bullet, wall, (attackData, targetData) =>
                {
                    wall.SufferBullet(bullet, attackData, targetData);
                    bullet.HitWall(wall, attackData, targetData);
                });
            }
        }
    }

    /// <summary>
    /// 壁からプレイヤーキャラへの衝突判定を行う。
    /// </summary>
    private void CheckCollisionWallToPlayer()
    {
        var player = CommandPlayerCharaManager.Instance.GetController();
        var walls = CommandWallManager.Instance.GetUpdateWalls();

        foreach (var wall in walls)
        {
            Collision.CheckCollide(wall, player, (attackData, targetData) =>
            {
                player.SufferWall(wall, attackData, targetData);
                wall.HitChara(player, attackData, targetData);
            });
        }
    }

    /// <summary>
    /// 弾からキャラへの衝突判定を行う。
    /// </summary>
    private void CheckCollisionBulletToChara()
    {
        var bullets = CommandBulletManager.Instance.GetUpdateBullets();
        var player = CommandPlayerCharaManager.Instance.GetController();
        var enemies = CommandEnemyCharaManager.Instance.GetUpdateEnemies();

        foreach (var bullet in bullets)
        {
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
        var player = CommandPlayerCharaManager.Instance.GetController();
        var enemies = CommandEnemyCharaManager.Instance.GetUpdateEnemies();

        foreach (var enemy in enemies)
        {
            Collision.CheckCollide(enemy, player, (attackData, targetData) =>
            {
                player.SufferChara(enemy, attackData, targetData);
                enemy.HitChara(player, attackData, targetData);
            });
        }
    }
}

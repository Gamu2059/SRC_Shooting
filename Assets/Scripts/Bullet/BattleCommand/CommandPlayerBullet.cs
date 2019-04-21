using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントのプレイヤーの弾。
/// </summary>
public class CommandPlayerBullet : CommandBulletController
{
    public override void HitWall(CommandWallController targetWall, ColliderData attackData, ColliderData targetData)
    {
        base.HitWall(targetWall, attackData, targetData);
        DestroyBullet();
    }
}

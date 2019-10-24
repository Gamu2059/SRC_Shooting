using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingBossBehavior : ControllableObject
{
    protected BattleHackingEnemyController Enemy { get; private set; }
    protected BattleHackingBossBehaviorParamSet BehaviorParamSet { get; private set; }

    public BattleHackingBossBehavior(BattleHackingEnemyController enemy, BattleHackingBossBehaviorParamSet paramSet)
    {
        Enemy = enemy;
        BehaviorParamSet = paramSet;
    }

    public virtual void OnEnd()
    {

    }

    /// <summary>
    /// ボスのワールド座標を設定する。
    /// </summary>
    public void SetPosition(Vector3 pos)
    {
        if (Enemy == null)
        {
            return;
        }

        pos.y = Enemy.transform.position.y;
        Enemy.transform.position = pos;
    }

    /// <summary>
    /// ボスのワールド回転を設定する。
    /// </summary>
    public void SetRotation(float angle)
    {
        if (Enemy == null)
        {
            return;
        }

        var angles = Enemy.transform.eulerAngles;
        angles.y = angle;
        Enemy.transform.eulerAngles = angles;
    }

    public CommandBulletController Shot()
    {
        return CommandBulletController.ShotBullet(Enemy);
    }

    public CommandBulletController Shot(CommandBulletShotParam p)
    {
        p.BulletOwner = Enemy;
        return CommandBulletController.ShotBullet(p);
    }
}

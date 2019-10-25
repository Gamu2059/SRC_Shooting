using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealBossBehavior : ControllableObject
{
    protected BattleRealEnemyController Enemy { get; private set; }
    protected BattleRealBossBehaviorParamSet BehaviorParamSet { get; private set; }

    public BattleRealBossBehavior(BattleRealEnemyController enemy, BattleRealBossBehaviorParamSet paramSet)
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

    public BulletController Shot()
    {
        return BulletController.ShotBullet(Enemy);
    }

    public BulletController Shot(BulletShotParam p)
    {
        p.BulletOwner = Enemy;
        return BulletController.ShotBullet(p);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class BattleRealBossBehavior : ControllableObject
{
    protected BattleRealEnemyBase Enemy { get; private set; }
    protected BattleRealBossBehaviorUnitParamSet BehaviorParamSet { get; private set; }

    public BattleRealBossBehavior(BattleRealEnemyBase enemy, BattleRealBossBehaviorUnitParamSet paramSet)
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
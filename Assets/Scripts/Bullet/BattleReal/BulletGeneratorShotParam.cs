#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// BulletGeneratorで弾が生成しやすいようにしたBulletShotParam
/// </summary>
public struct BulletGeneratorShotParam
{
    public BattleRealCharaController BulletOwner;
    public BulletController Bullet;
    public BulletParam BulletParam;
    public Vector3 Position;
    public float Rotation;
    public Vector3 Scale;
}

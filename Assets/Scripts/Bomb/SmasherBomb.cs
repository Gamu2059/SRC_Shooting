using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherBomb : LaserController
{
    private Vector3 offset;

    private E_BOMB_ROTATE_DIR BOMB_ROTATE_DIR;

    private float angleSpeed;

    private Vector3 finalyLaserDirection;

    private float timeCount;

    // 常に発射地点をプレイヤーの周囲に固定する
    public override void OnUpdate()
    {
        base.OnUpdate();
        timeCount += Time.deltaTime;
        SetRotation(Vector3.Lerp(Vector3.forward, finalyLaserDirection, (timeCount * angleSpeed) / Vector3.Angle(Vector3.forward, finalyLaserDirection)));
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        SetTimeCount();
    }

    public void SetOffset(Vector3 v)
    {
        offset = v;
    }

    public void SetTimeCount()
    {
        timeCount = 0;
    }

    public void SetBombRotateDir(E_BOMB_ROTATE_DIR _BOMB_ROTATE_DIR)
    {
        BOMB_ROTATE_DIR = _BOMB_ROTATE_DIR;
    }

    public void SetAngleSpeed(float anglespeed)
    {
        angleSpeed = anglespeed;
    }

    public void SetOpenAngle(float openangle)
    {
        finalyLaserDirection = new Vector3(0, openangle * (int)BOMB_ROTATE_DIR, 0);
    }

    public override void HitChara(CharaController targetChara, ColliderData attackData, ColliderData targetData)
    {
        base.HitChara(targetChara, attackData, targetData);
    }

    public override void HitBullet(BulletController targetBullet, ColliderData attackData, ColliderData targetData)
    {
        base.HitBullet(targetBullet, attackData, targetData);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherBomb : LaserController
{

    private Vector3 offset;

    private E_BOMB_ROTATE_DIR BOMB_ROTATE_DIR;

    private float angleSpeed;

    // 常に発射地点をプレイヤーの周囲に固定する
    public override void OnUpdate()
    {
        base.OnUpdate();
        //var bombOwner = GetBulletOwner();
        //Vector3 pos = bombOwner.transform.localPosition + offset;
        //pos.y = ParamDef.BASE_Y_POS;
        //SetPosition(pos);
        Vector3 rot = transform.localEulerAngles;
        rot.y += (int)BOMB_ROTATE_DIR * angleSpeed * Time.deltaTime;
        SetRotation(rot);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOffset(Vector3 v)
    {
        offset = v;
    }

    public void SetBombRotateDir(E_BOMB_ROTATE_DIR _BOMB_ROTATE_DIR)
    {
        BOMB_ROTATE_DIR = _BOMB_ROTATE_DIR;
    }

    public void SetAngleSpeed(float anglespeed)
    {
        Debug.Log(anglespeed);
        angleSpeed = anglespeed;
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

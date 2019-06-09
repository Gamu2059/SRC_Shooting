using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherBomb : LaserController
{

    private Vector3 offset;

    // 常に発射地点をプレイヤーの周囲に固定する
    public override void OnUpdate()
    {
        base.OnUpdate();
        //var bombOwner = GetBulletOwner();
        //Vector3 pos = bombOwner.transform.localPosition + offset;
        //pos.y = ParamDef.BASE_Y_POS;
        //SetPosition(pos);
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
}

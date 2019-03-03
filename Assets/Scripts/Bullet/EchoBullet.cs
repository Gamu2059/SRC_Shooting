using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : Bullet
{
    
    public override void OnHitCharacter(CharaControllerBase chara)
    {
        base.OnHitCharacter(chara);
        
    }

    // 被弾したキャラクターの座標を中心に8方向に弾を飛ばす
    void DiffusionBullet(CharaControllerBase chara)
    {
        
    }
}

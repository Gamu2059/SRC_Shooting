using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DanmakuPolymorphism : System.Object
{
    // 毎フレーム行う処理
    public abstract void Updates(BattleRealEnemyBase enemyController);
}

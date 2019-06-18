using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントのボス敵。
/// </summary>
public class CommandEnemyBoss : CommandEnemyController
{

    public override void Dead()
    {
        base.Dead();
        BattleManager.Instance.TransitionBattleMain();
    }
}

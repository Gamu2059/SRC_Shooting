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
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleRealEnemyBehaviorUnitやBattleRealEnemyBehaviorGroupの特定のタイミングで呼び出したい関数をオプショナルで追加するためのもの。
/// </summary>
public abstract class BattleRealEnemyBehaviorOptionFuncBase : ScriptableObject
{
    public abstract void Call(BattleRealEnemyBase enemy);
}

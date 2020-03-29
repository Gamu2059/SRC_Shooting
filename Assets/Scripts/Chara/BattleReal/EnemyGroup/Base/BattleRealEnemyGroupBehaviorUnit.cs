#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵グループを動かすための振る舞いの基底クラス。
/// </summary>
public class BattleRealEnemyGroupBehaviorUnit : ScriptableObject, IControllableGameCycle
{
    protected BattleRealEnemyGroupController EnemyGroup { get; private set; }

    public void SetEnemyGroup(BattleRealEnemyGroupController enemyGroup)
    {
        EnemyGroup = enemyGroup;
    }

    public virtual void OnInitialize() { }

    public virtual void OnFinalize() { }

    public virtual void OnStart() { }

    public virtual void OnUpdate() { }

    public virtual void OnLateUpdate() { }

    public virtual void OnFixedUpdate() { }

    public virtual void OnEnd() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵を動かすための振る舞いの基底クラス。
/// </summary>
public class BattleRealEnemyBehaviorUnit : ScriptableObject, IControllableGameCycle
{
    protected BattleRealEnemyBase m_Enemy { get; private set; }

    public void SetEnemy(BattleRealEnemyBase enemy)
    {
        m_Enemy = enemy;
    }

    public virtual void OnInitialize() { }

    public virtual void OnFinalize() { }

    public virtual void OnStart() { }

    public virtual void OnUpdate() { }

    public virtual void OnLateUpdate() { }

    public virtual void OnFixedUpdate() { }

    public virtual void OnEnd() { }
}

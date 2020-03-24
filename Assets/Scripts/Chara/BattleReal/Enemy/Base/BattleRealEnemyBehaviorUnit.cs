#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵を動かすための振る舞いの基底クラス。
/// </summary>
public class BattleRealEnemyBehaviorUnit : BattleRealEnemyBehaviorElement
{
    [Header("End Parameter")]

    [SerializeField, Tooltip("割り込み終了関数によって終了しなかった場合の、デフォルトの終了値")]
    private bool m_DefaultEndValue;

    protected BattleRealEnemyBase Enemy { get; private set; }
    protected BattleRealEnemyBehaviorController Controller { get; private set; }
    protected float CurrentTime { get; private set; }

    public void OnStartUnit(BattleRealEnemyBase enemy, BattleRealEnemyBehaviorController controller)
    {
        Enemy = enemy;
        Controller = controller;
        CurrentTime = 0;

        OnStart();
    }

    public void OnUpdateUnit(float deltaTime)
    {
        CurrentTime += deltaTime;
        OnUpdate(deltaTime);
    }

    public void OnLateUpdateUnit(float deltaTime)
    {
        OnLateUpdate(deltaTime);
    }

    public void OnEndUnit() 
    {
        OnEnd();

        Controller = null;
        Enemy = null;
    }

    public bool IsEndUnit()
    {
        return IsEnd();
    }

    #region Have to Override Method

    protected virtual void OnStart() { }

    protected virtual void OnUpdate(float deltaTime) { }

    protected virtual void OnLateUpdate(float deltaTime) { }

    protected virtual void OnEnd() { }

    protected virtual bool IsEnd() { return m_DefaultEndValue; }

    #endregion
}

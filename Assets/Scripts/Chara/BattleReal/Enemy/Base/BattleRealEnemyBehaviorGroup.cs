#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵を動かすための複数処理機構。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemySequence/Group/Default", fileName = "default.behavior_group.asset", order = 0)]
public class BattleRealEnemyBehaviorGroup : BattleRealEnemyBehaviorElement
{
    [SerializeField]
    private List<BattleRealEnemyBehaviorElement> m_Elements;

    protected BattleRealEnemyBase Enemy { get; private set; }
    protected BattleRealEnemyBehaviorController Controller { get; private set; }
    public int CurrentIndex { get; private set; }

    public void OnStartGroup(BattleRealEnemyBase enemy, BattleRealEnemyBehaviorController controller)
    {
        Enemy = enemy;
        Controller = controller;
        CurrentIndex = 0;

        OnStart();
    }

    public void OnLoopedGroup()
    {
        CurrentIndex = 0;

        OnLooped();
    }

    public void OnEndGroup()
    {
        OnEnd();

        Controller = null;
        Enemy = null;
    }

    public void Forward()
    {
        CurrentIndex++;
    }

    public BattleRealEnemyBehaviorElement GetCurrentReferenceElement()
    {
        return GetReferenceElementAt(CurrentIndex);
    }

    public BattleRealEnemyBehaviorElement GetNextReferenceElement()
    {
        return GetReferenceElementAt(CurrentIndex + 1);
    }

    public BattleRealEnemyBehaviorElement GetReferenceElementAt(int index)
    {
        if (m_Elements == null || index < 0 || IsLastOver(index))
        {
            return null;
        }

        return m_Elements[index];
    }

    public bool IsLastOver(int index)
    {
        return m_Elements == null || index >= m_Elements.Count;
    }

    #region Have to Override Mehtod

    public virtual void OnStart() { }

    public virtual void OnLooped() { }

    public virtual void OnEnd() { }

    /// <summary>
    /// このグループが終了するかどうかを判定する。
    /// 終了する場合はtrueを返す。
    /// </summary>
    public virtual bool IsEndGroup()
    {
        return true;
    }

    #endregion
}

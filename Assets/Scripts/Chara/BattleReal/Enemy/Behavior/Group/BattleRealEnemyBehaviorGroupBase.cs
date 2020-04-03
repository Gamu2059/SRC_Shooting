#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵を動かすための複数処理機構。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Group/Default", fileName = "default.behavior_group.asset", order = 0)]
public class BattleRealEnemyBehaviorGroupBase : BattleRealEnemyBehaviorElement
{
    #region Field Inspector

    [SerializeField]
    private List<BattleRealEnemyBehaviorElement> m_Elements;

    [Header("End Parameter")]

    [SerializeField, Tooltip("終了条件に適用するブール値。無限ループさせたい場合は、これをfalseにしておきます。")]
    private bool m_DefaultEndValue = true;

    [Header("Option Parameter")]

    [SerializeField]
    private BattleRealEnemyBehaviorOptionFuncBase[] m_OnStartOptions;

    [SerializeField]
    private BattleRealEnemyBehaviorOptionFuncBase[] m_OnEndOptions;

    [SerializeField]
    private BattleRealEnemyBehaviorOptionFuncBase[] m_OnLoopedOptions;

    [SerializeField]
    private BattleRealEnemyBehaviorOptionFuncBase[] m_OnStopOptions;

    #endregion

    #region Field

    protected BattleRealEnemyBase Enemy { get; private set; }
    protected BattleRealEnemyBehaviorController Controller { get; private set; }
    public int CurrentIndex { get; private set; }

    #endregion

    /// <summary>
    /// このグループの開始時の処理
    /// </summary>
    public void OnStartGroup(BattleRealEnemyBase enemy, BattleRealEnemyBehaviorController controller)
    {
        Enemy = enemy;
        Controller = controller;
        CurrentIndex = 0;

        if (m_OnStartOptions != null)
        {
            foreach (var option in m_OnStartOptions)
            {
                option?.Call(Enemy);
            }
        }

        OnStart();
    }

    /// <summary>
    /// このグループが終了せずループした時の処理
    /// </summary>
    public void OnLoopedGroup()
    {
        CurrentIndex = 0;

        if (m_OnLoopedOptions != null)
        {
            foreach (var option in m_OnLoopedOptions)
            {
                option?.Call(Enemy);
            }
        }

        OnLooped();
    }

    /// <summary>
    /// このグループの終了時の処理
    /// </summary>
    public void OnEndGroup()
    {
        OnEnd();

        if (m_OnEndOptions != null)
        {
            foreach (var option in m_OnEndOptions)
            {
                option?.Call(Enemy);
            }
        }

        Controller = null;
        Enemy = null;
    }

    /// <summary>
    /// このグループの中断時の処理
    /// </summary>
    public void OnStopGroup()
    {
        OnStop();

        if (m_OnStopOptions != null)
        {
            foreach (var option in m_OnStopOptions)
            {
                option?.Call(Enemy);
            }
        }

        Controller = null;
        Enemy = null;
    }

    /// <summary>
    /// 指し示しているインデックスを一つ進める。
    /// </summary>
    public void Forward()
    {
        CurrentIndex++;
    }

    /// <summary>
    /// 現在指しているインデックスのBehaviorElementを取得する。
    /// </summary>
    public BattleRealEnemyBehaviorElement GetCurrentReferenceElement()
    {
        return GetReferenceElementAt(CurrentIndex);
    }

    /// <summary>
    /// 指しているインデックスの次のBehaviorElementを取得する。
    /// </summary>
    /// <returns></returns>
    public BattleRealEnemyBehaviorElement GetNextReferenceElement()
    {
        return GetReferenceElementAt(CurrentIndex + 1);
    }

    /// <summary>
    /// 指定されたインデックスにあるBehaviorElementを取得する。
    /// </summary>
    public BattleRealEnemyBehaviorElement GetReferenceElementAt(int index)
    {
        var elements = GetCurrentElements();
        if (elements == null || index < 0 || IsLastOver(index))
        {
            return null;
        }

        return elements[index];
    }

    /// <summary>
    /// 指定されたインデックスがElementsの範囲を超えているかどうかを判定する。
    /// </summary>
    public bool IsLastOver(int index)
    {
        var elements = GetCurrentElements();
        return elements == null || index >= elements.Count;
    }

    #region Have to Override Mehtod

    protected virtual void OnStart() { }

    protected virtual void OnLooped() { }

    protected virtual void OnEnd() { }

    protected virtual void OnStop() { }

    /// <summary>
    /// 今適用されているElementsを取得する。<br/>
    /// グループによっては参照するElementsが切り替わる可能性があるため、このメソッドで取得したElementsをキャッシュすることは非推奨。
    /// </summary>
    protected virtual List<BattleRealEnemyBehaviorElement> GetCurrentElements()
    {
        return m_Elements;
    }

    /// <summary>
    /// このグループが終了するかどうかを判定する。
    /// 終了する場合はtrueを返す。
    /// </summary>
    public virtual bool IsEndGroup()
    {
        return m_DefaultEndValue;
    }

    #endregion
}

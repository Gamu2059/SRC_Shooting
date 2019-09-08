using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EventManagerで制御可能なスクリプト。
/// </summary>
public class EventControllableScript : IControllableGameCycle
{
    #region Field

    /// <summary>
    /// サイクル
    /// </summary>
    private E_OBJECT_CYCLE m_Cycle;

    /// <summary>
    /// スクリプトの引数
    /// </summary>
    private ArgumentParamSet m_ParamSet;

    #endregion



    #region Get Set

    public E_OBJECT_CYCLE GetCycle()
    {
        return m_Cycle;
    }

    public void SetCycle(E_OBJECT_CYCLE cycle)
    {
        m_Cycle = cycle;
    }

    public ArgumentParamSet GetParamSet()
    {
        return m_ParamSet;
    }

    #endregion



    /// <summary>
    /// このスクリプトを破棄する。
    /// </summary>
    protected void DestroyScript()
    {
        EventManager.Instance.CheckDestroyScript(this);
    }

    /// <summary>
    /// スクリプトの引数をセットする
    /// </summary>
    public void SetArguments(ArgumentVariable[] argumentVariables)
    {
        m_ParamSet = ArgumentParamSetTranslator.TranslateFromArgumentVariables(argumentVariables);
    }

    public virtual void OnInitialize()
    {
    }

    public virtual void OnFinalize()
    {
    }

    public virtual void OnStart()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnLateUpdate()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }
}

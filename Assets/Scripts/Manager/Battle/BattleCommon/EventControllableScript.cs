using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EventManagerで制御可能なスクリプト。
/// </summary>
public class EventControllableScript : IControllableGameCycle
{
    private E_OBJECT_CYCLE m_Cycle;

    public E_OBJECT_CYCLE GetCycle()
    {
        return m_Cycle;
    }

    public void SetCycle(E_OBJECT_CYCLE cycle)
    {
        m_Cycle = cycle;
    }

    /// <summary>
    /// このスクリプトを破棄する。
    /// </summary>
    protected void DestroyScript()
    {
        EventManager.Instance.CheckDestroyScript(this);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ステートマシンで使用するステート。
/// Created by Sho Yamagami.
/// </summary>
public class State<T> : ControllableObject
{
    public T Key { get; private set; }

    public Action m_OnStart;
    public Action m_OnUpdate;
    public Action m_OnLateUpdate;
    public Action m_OnFixedUpdate;
    public Action m_OnEnd;

    private StateCycleBase m_StateCycle;

    public State(T key)
    {
        Key = key;
        m_StateCycle = null;
    }

    public State(T key, StateCycleBase stateCycle)
    {
        Key = key;
        m_StateCycle = stateCycle;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateCycle?.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_StateCycle?.OnFinalize();
        m_StateCycle = null;
        m_OnStart = null;
        m_OnUpdate = null;
        m_OnLateUpdate = null;
        m_OnFixedUpdate = null;
        m_OnEnd = null;
    }

    public override void OnStart()
    {
        if (m_StateCycle != null)
        {
            m_StateCycle.OnStart();
        }
        else
        {
            m_OnStart?.Invoke();
        }
    }

    public void OnEnd()
    {
        if (m_StateCycle != null)
        {
            m_StateCycle.OnEnd();
        }
        else
        {
            m_OnEnd?.Invoke();
        }
    }

    public override void OnUpdate()
    {
        if (m_StateCycle != null)
        {
            m_StateCycle.OnUpdate();
        }
        else
        {
            m_OnUpdate?.Invoke();
        }
    }

    public override void OnLateUpdate()
    {
        if (m_StateCycle != null)
        {
            m_StateCycle.OnLateUpdate();
        }
        else
        {
            m_OnLateUpdate?.Invoke();
        }
    }

    public override void OnFixedUpdate()
    {
        if (m_StateCycle != null)
        {
            m_StateCycle.OnFixedUpdate();
        }
        else
        {
            m_OnFixedUpdate?.Invoke();
        }
    }
}

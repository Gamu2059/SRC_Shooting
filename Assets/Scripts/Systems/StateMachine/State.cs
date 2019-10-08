using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ステートマシンで使用するステート。
/// Created by Sho Yamagami.
/// </summary>
public class State<T>
{
    public T Key { get; private set; }

    public Action OnStart;
    public Action OnUpdate;
    public Action OnLateUpdate;
    public Action OnFixedUpdate;
    public Action OnEnd;

    public State(T key)
    {
        Key = key;
    }

    public void OnFinalize()
    {
        OnStart = null;
        OnUpdate = null;
        OnLateUpdate = null;
        OnFixedUpdate = null;
        OnEnd = null;
    }

    ~State()
    {
        OnFinalize();
    }
}

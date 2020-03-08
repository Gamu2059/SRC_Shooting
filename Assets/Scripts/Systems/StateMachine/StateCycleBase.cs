using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCycleBase<T> : ControllableObject
{
    protected T Target { get; private set; }

    public StateCycleBase(T target)
    {
        Target = target;
    }

    public virtual void OnEnd()
    {
    }
}

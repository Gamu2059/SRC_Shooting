using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : TimerManagerBase<TimerManager>
{
    public static TimerManager Builder()
    {
        var manager = Create();
        manager.OnInitialize();
        return manager;
    }
}

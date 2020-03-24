using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealTimerManager : TimerManagerBase<BattleRealTimerManager>
{
    public static BattleRealTimerManager Builder()
    {
        var manager = Create();
        manager.OnInitialize();
        return manager;
    }
}

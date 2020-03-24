using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingTimerManager : TimerManagerBase<BattleHackingTimerManager>
{
    public static BattleHackingTimerManager Builder()
    {
        var manager = Create();
        manager.OnInitialize();
        return manager;
    }
}

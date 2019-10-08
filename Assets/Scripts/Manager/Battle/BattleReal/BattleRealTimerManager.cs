using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealTimerManager : TimerManagerBase
{
    public static BattleRealTimerManager Instance => BattleRealManager.Instance.RealTimerManager;
}

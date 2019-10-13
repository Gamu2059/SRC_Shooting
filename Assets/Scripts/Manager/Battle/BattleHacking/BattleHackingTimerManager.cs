using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingTimerManager : TimerManagerBase
{
    public static BattleHackingTimerManager Instance => BattleHackingManager.Instance.HackingTimerManager;
}

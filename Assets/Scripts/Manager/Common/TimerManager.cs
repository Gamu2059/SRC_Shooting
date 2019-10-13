using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : TimerManagerBase
{
    public static TimerManager Instance => GameManager.Instance.TimerManager;
}

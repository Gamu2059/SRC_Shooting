using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotController : ScriptableObject
{
    public abstract void GetshotParam(ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState1 state);
}

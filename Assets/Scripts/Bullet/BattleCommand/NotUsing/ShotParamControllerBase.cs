using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotParamControllerBase : ScriptableObject
{
    public abstract void GetshotParam(ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState state);
}

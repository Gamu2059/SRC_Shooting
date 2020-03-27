using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyParamChange
{
    bool UseLookMoveDirChange { get; }
    bool ApplyLookMoveDir { get; }
    bool UseCriticalColliderEnableChange { get; }
    bool ApplyCriticalColliderEnable { get; }
    bool UseWillDestroyOnOutOfEnemyFieldChange { get; }
    bool ApplyWillDestroyOnOutOfEnemyField { get; }
}

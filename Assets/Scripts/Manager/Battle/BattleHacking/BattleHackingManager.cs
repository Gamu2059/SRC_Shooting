using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingManager : ControllableObject
{
    #region Field

    [SerializeField]
    private BattleHackingParamSet m_ParamSet;

    #endregion

    public BattleHackingManager(BattleHackingParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    #region Game Cycyle

    #endregion
}

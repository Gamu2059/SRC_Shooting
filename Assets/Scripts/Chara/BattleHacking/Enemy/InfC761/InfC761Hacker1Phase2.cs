using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Hacker1Phase2 : InfC761Hacker1Phase
{

    #region Field

    private new InfC761Hacker1Phase2ParamSet m_ParamSet;

    #endregion

    public InfC761Hacker1Phase2(BattleHackingEnemyController enemy, BattleHackingBossBehaviorUnitParamSet paramSet) : base(enemy, paramSet)
    {
        m_ParamSet = paramSet as InfC761Hacker1Phase2ParamSet;
    }


    public override BattleHackingBossBehaviorUnitParamSet GetParamSet()
    {
        return m_ParamSet;
    }
}

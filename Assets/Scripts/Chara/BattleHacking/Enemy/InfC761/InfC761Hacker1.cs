using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// Inf-C-761のハッキングモードの1つ目のコントローラ
/// </summary>
public class InfC761Hacker1 : BattleHackingBoss
{
    #region Field

    private InfC761Hacker1ParamSet m_InfC761Hacker1ParamSet;

    #endregion

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();

        if (BehaviorParamSet is InfC761Hacker1ParamSet paramSet)
        {
            m_InfC761Hacker1ParamSet = paramSet;
        }
    }
}

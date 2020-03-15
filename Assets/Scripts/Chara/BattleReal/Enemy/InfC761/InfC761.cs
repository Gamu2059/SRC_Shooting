using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// Inf-C-761のコントローラ
/// </summary>
public class InfC761 : BattleRealBoss
{
    #region Field

    private InfC761ParamSet m_InfC761ParamSet;

    #endregion

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();

        //if (BehaviorParamSet is InfC761ParamSet paramSet)
        //{
        //    m_InfC761ParamSet = paramSet;
        //}
    }
}

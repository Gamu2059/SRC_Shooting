using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class AutoControllerParam
{
    public static void ChangeAutoController(BattleRealCharaController chara, IAutoControllerChanger param)
    {
        if (chara == null)
        {
            return;
        }

        Type type;
        try
        {
            type = Type.GetType(param.ControllerName);
        }
        catch (Exception)
        {
            Debug.LogWarningFormat("[{0}] : 指定した名前のクラスがありませんでした。 name : {1}", param.GetType().Name, param.ControllerName);
            return;
        }
        
        if (type == null)
        {
            return;
        }

        foreach (var b in chara.AutoControlBehaviors)
        {
            if (b.GetType().Equals(type) && b is IAutoControlOnCharaController c)
            {
                c.IsEnableController = param.ApplyEnableController;
            }
        }
    }
}

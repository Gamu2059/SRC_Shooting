#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Eventを呼ぶためのオプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Option/BattleRealEvent", fileName = "param.behavior_option.asset")]
public class BattleRealEnemyBehaviorEventFunc : BattleRealEnemyBehaviorOptionFuncBase
{
    [SerializeField]
    private BattleRealEventContent m_CallEvent;

    public override void Call(BattleRealEnemyBase enemy)
    {
        if (BattleRealEventManager.Instance == null)
        {
            return;
        }

        BattleRealEventManager.Instance.ExecuteEvent(m_CallEvent);
    }
}

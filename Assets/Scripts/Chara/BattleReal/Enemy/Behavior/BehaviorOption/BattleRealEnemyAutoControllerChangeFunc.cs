#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵のIAutoControlOnCharaControllerを有効/無効を変更するためのオプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemySequence/Option/AutoControllerChange", fileName = "auto_controller_change.behavior_option.asset", order = 10)]
public class BattleRealEnemyAutoControllerChangeFunc : BattleRealEnemyBehaviorOptionFunc, IAutoControllerChanger
{
    [SerializeField]
    private string m_ControllerName;
    public string ControllerName => m_ControllerName;

    [SerializeField]
    private bool m_ApplyEnableController;
    public bool ApplyEnableController => m_ApplyEnableController;

    public override void Call(BattleRealEnemyBase enemy)
    {
        AutoControllerParam.ChangeAutoController(enemy, this);
    }
}

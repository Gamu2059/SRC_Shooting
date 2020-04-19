#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵のIAutoControlOnCharaControllerを有効/無効を変更するためのオプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Option/AutoControllerChange", fileName = "param.behavior_option.asset")]
public class BattleRealEnemyAutoControllerChangeFunc : BattleRealEnemyBehaviorOptionFuncBase, IAutoControllerChanger
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

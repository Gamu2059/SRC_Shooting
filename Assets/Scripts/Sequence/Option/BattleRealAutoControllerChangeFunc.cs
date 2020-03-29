#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵のIAutoControlOnCharaControllerを有効/無効を変更するためのオプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Option/BattleRealAutoControllerChange", fileName = "auto_controller_change.sequence_option.asset")]
public class BattleRealAutoControllerChangeFunc : SequenceOptionFunc, IAutoControllerChanger
{
    [SerializeField]
    private string m_ControllerName;
    public string ControllerName => m_ControllerName;

    [SerializeField]
    private bool m_ApplyEnableController;
    public bool ApplyEnableController => m_ApplyEnableController;

    public override void Call(Transform transform = null)
    {
        if (transform == null)
        {
            return;
        }

        AutoControllerParam.ChangeAutoController(transform.GetComponent<BattleRealCharaController>(), this);
    }
}

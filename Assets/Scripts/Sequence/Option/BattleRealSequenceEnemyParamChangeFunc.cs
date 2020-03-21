#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵のpublicパラメータを変更するためのオプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Option/BattleRealEnemyParamChange", fileName = "enemy_param_change.sequence_option.asset")]
public class BattleRealSequenceEnemyParamChangeFunc : SequenceOptionFunc
{
    [SerializeField]
    private bool m_UseLookMoveDirChange;

    [SerializeField]
    private bool m_ApplyLookMoveDir;

    public override void Call(Transform transform = null)
    {
        if (transform == null)
        {
            Debug.LogWarningFormat("ターゲットが存在しません。");
            return;
        }

        var enemy = transform.GetComponent<BattleRealEnemyBase>();
        if (enemy == null)
        {
            Debug.LogWarningFormat("ターゲットにリアルモードの敵コンポーネントはアタッチされていません。 name : {0}", transform.name);
            return;
        }

        if (m_UseLookMoveDirChange)
        {
            enemy.IsLookMoveDir = m_ApplyLookMoveDir;
        }
    }
}

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
    [SerializeField, Tooltip("LookMoveDirパラメータを変更するかどうか。trueの場合、変更する。")]
    private bool m_UseLookMoveDirChange;

    [SerializeField, Tooltip("LookMoveDirに設定するパラメータ")]
    private bool m_ApplyLookMoveDir;

    [SerializeField, Tooltip("敵自身の被弾コライダーの有効無効を変更するかどうか。trueの場合、変更する。")]
    private bool m_UseCriticalColliderEnableChange;

    [SerializeField, Tooltip("敵自身の被弾コライダーの有効無効を設定するパラメータ")]
    private bool m_ApplyCriticalColliderEnable;

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

        if (m_UseCriticalColliderEnableChange)
        {
            var critical = enemy.GetCollider().GetColliderTransform(E_COLLIDER_TYPE.CRITICAL);
            enemy.GetCollider().SetEnableCollider(critical.Transform, m_ApplyCriticalColliderEnable);
        }
    }
}

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵のpublicパラメータを変更するためのオプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Option/BattleRealEnemyParamChange", fileName = "enemy_param_change.sequence_option.asset")]
public class BattleRealSequenceEnemyParamChangeFunc : SequenceOptionFunc, IEnemyParamChange
{
    [SerializeField, Tooltip("LookMoveDirパラメータを変更するかどうか。trueの場合、変更する。")]
    private bool m_UseLookMoveDirChange;
    public bool UseLookMoveDirChange => m_UseLookMoveDirChange;

    [SerializeField, Tooltip("LookMoveDirに設定するパラメータ")]
    private bool m_ApplyLookMoveDir;
    public bool ApplyLookMoveDir => m_ApplyLookMoveDir;

    [SerializeField, Tooltip("WillDestroyOnOutOfEnemyFieldパラメータを変更するかどうか。trueの場合、変更する。")]
    private bool m_UseWillDestroyOnOutOfEnemyFieldChange;
    public bool UseWillDestroyOnOutOfEnemyFieldChange => m_UseWillDestroyOnOutOfEnemyFieldChange;

    [SerializeField, Tooltip("WillDestroyOnOutOfEnemyFieldに設定するパラメータ")]
    private bool m_ApplyWillDestroyOnOutOfEnemyField;
    public bool ApplyWillDestroyOnOutOfEnemyField => m_ApplyWillDestroyOnOutOfEnemyField;

    [SerializeField, Tooltip("敵自身の被弾コライダーの有効無効を変更するかどうか。trueの場合、変更する。")]
    private bool m_UseCriticalColliderEnableChange;
    public bool UseCriticalColliderEnableChange => m_UseCriticalColliderEnableChange;

    [SerializeField, Tooltip("敵自身の被弾コライダーの有効無効を設定するパラメータ")]
    private bool m_ApplyCriticalColliderEnable;
    public bool ApplyCriticalColliderEnable => m_ApplyCriticalColliderEnable;

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

        EnemyParamChanger.ChangeEnemyParam(enemy, this);
    }
}

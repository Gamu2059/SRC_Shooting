#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 指定した名前のトランスフォームを追従する敵グループの挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGroup/Behavior/Unit/AllowTransform", fileName = "param.enemy_group_behavior.asset")]
public class BattleRealEnemyGroupAllowTransformBehavior : BattleRealEnemyGroupBehaviorUnitBase
{
    #region Define

    [Serializable]
    protected enum E_TARGET_TYPE
    {
        PLAYER,
        ENEMY,
    }

    #endregion

    #region Field Inspector

    [Header("Allow Transform Parameter")]

    [SerializeField, Tooltip("追従するトランスフォームの名前")]
    private string m_TargetTransformName;
    protected string TargetTransformName => m_TargetTransformName;

    [SerializeField, Tooltip("追従対象のタイプ")]
    private E_TARGET_TYPE m_TargetType;
    protected E_TARGET_TYPE TargetType => m_TargetType;

    [SerializeField, Tooltip("座標のラープ値")]
    private float m_PositionAllowLerp;
    protected float PositionAllowLerp => m_PositionAllowLerp;

    [SerializeField, Tooltip("座標の無視データ")]
    private Vector3PassParam m_PositionPass;
    protected Vector3PassParam PositionPass => m_PositionPass;

    [SerializeField, Tooltip("角度のラープ値")]
    private float m_RotationAllowLerp;
    protected float RotationAllowLerp => m_RotationAllowLerp;

    [SerializeField, Tooltip("回転の無視データ")]
    private Vector3PassParam m_RotationPass;
    protected Vector3PassParam RotationPass => m_RotationPass;

    #endregion

    #region Field

    private bool m_IsInvalid;
    private Transform m_Target;

    #endregion

    #region Game Cycle

    public override void OnStart()
    {
        base.OnStart();

        m_IsInvalid = false;
        m_Target = null;
        switch (m_TargetType)
        {
            case E_TARGET_TYPE.PLAYER:
                var player = BattleRealPlayerManager.Instance.Player.transform;
                m_Target = player.Find(m_TargetTransformName, false);
                break;
            case E_TARGET_TYPE.ENEMY:
                var enemies = BattleRealEnemyManager.Instance.Enemies;
                foreach (var enemy in enemies)
                {
                    m_Target = enemy.transform.Find(m_TargetTransformName, false);
                    if (m_Target != null)
                    {
                        break;
                    }
                }
                break;
        }

        if (m_Target == null)
        {
            Debug.LogErrorFormat("対象となるトランスフォームがありませんでした。{0}", m_TargetTransformName);
            m_IsInvalid = true;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_IsInvalid)
        {
            return;
        }

        Apply();
    }

    #endregion

    private void Apply()
    {
        var group = EnemyGroup.transform;
        var pos = Vector3.LerpUnclamped(group.position, m_Target.position, PositionAllowLerp);
        var angles = Vector3.LerpUnclamped(group.eulerAngles, m_Target.eulerAngles, RotationAllowLerp);

        if (PositionPass.IsPassX)
        {
            pos.x = group.position.x;
        }

        if (PositionPass.IsPassY)
        {
            pos.y = group.position.y;
        }

        if (PositionPass.IsPassZ)
        {
            pos.z = group.position.z;
        }

        if (RotationPass.IsPassX)
        {
            angles.x = group.eulerAngles.x;
        }

        if (RotationPass.IsPassY)
        {
            angles.y = group.eulerAngles.y;
        }

        if (RotationPass.IsPassZ)
        {
            angles.z = group.eulerAngles.z;
        }

        group.SetPositionAndRotation(pos, Quaternion.Euler(angles));
    }
}

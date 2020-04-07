#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイヤーの向きに正面方向を向かせる敵グループの挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGroup/Behavior/Unit/PlayerDirection", fileName = "param.enemy_group_behavior.asset")]
public class BattleRealEnemyGroupPlayerDirectionBehavior : BattleRealEnemyGroupBehaviorUnitBase
{
    #region Field Inspector

    [Header("Player Direction Parameter")]

    [SerializeField, Tooltip("生成された瞬間にプレイヤーの方向を向くかどうか")]
    private bool m_IsLookPlayerOnStart;
    protected bool IsLookPlayerOnStart => m_IsLookPlayerOnStart;

    [SerializeField, Tooltip("ある時間でのプレイヤーへ向かう割合の関数")]
    private float m_LerpPlayerRate;
    protected float LerpPlayerRate => m_LerpPlayerRate;

    #endregion

    #region Field

    #endregion

    #region Game Cycle

    public override void OnStart()
    {
        base.OnStart();

        if (IsLookPlayerOnStart)
        {
            if (BattleRealPlayerManager.Instance != null)
            {
                var player = BattleRealPlayerManager.Instance.Player;
                var pos = player.transform.position;
                pos.y = EnemyGroup.transform.position.y;

                EnemyGroup.transform.LookAt(pos);
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Move();
    }

    #endregion

    private void Move()
    {
        var group = EnemyGroup.transform;
        var player = BattleRealPlayerManager.Instance.Player.transform;
        var forward = group.forward;
        var playerForward = player.position - group.position;
        playerForward.y = 0;
        group.forward = Vector3.LerpUnclamped(forward, playerForward, LerpPlayerRate);
    }
}

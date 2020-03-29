#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイヤーのX座標と同じ位置へと移動する敵の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemySequence/Unit/PlayerXFollow", fileName = "player_x_follow.behavior_unit.asset", order = 1010)]
public class BattleRealEnemyPlayerXFollowBehavior : BattleRealEnemyBehaviorUnit
{
    #region Field Inspector

    [Header("Player X Follow Parameter")]

    [SerializeField]
    private float m_XMoveSpeed;

    #endregion

    #region Field

    private float m_TargetX;
    private float m_MoveSumDistance;
    private float m_MoveDir;

    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        var player = BattleRealPlayerManager.Instance.Player;
        if (player.IsDead)
        {
            m_TargetX = Enemy.transform.position.x;
        }
        else
        {
            m_TargetX = player.transform.position.x;
        }

        var delta = m_TargetX - Enemy.transform.position.x;
        m_MoveSumDistance = Mathf.Abs(delta);
        m_MoveDir = Mathf.Sign(delta);
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        var translateX = m_MoveDir * m_XMoveSpeed * Time.deltaTime;
        Enemy.transform.Translate(Vector3.right * translateX, Space.World);
        m_MoveSumDistance -= Mathf.Abs(translateX);
    }

    protected override bool IsEnd()
    {
        return m_MoveSumDistance <= 0;
    }

    protected override void OnEnd()
    {
        base.OnEnd();

        var pos = Enemy.transform.position;
        pos.x = m_TargetX;
        Enemy.transform.position = pos;
    }

    #endregion
}

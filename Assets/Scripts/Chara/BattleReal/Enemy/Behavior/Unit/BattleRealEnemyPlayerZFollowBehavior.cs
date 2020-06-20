using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// プレイヤーのZ座標と同じ位置へと移動する敵の挙動
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Unit/PlayerZFollow", fileName = "param.behavior_unit.asset")]
public class BattleRealEnemyPlayerZFollowBehavior : BattleRealEnemyBehaviorUnitBase
{
    #region Field Inspector

    [Header("Player Z Follow Parameter")]

    [SerializeField]
    private float m_ZMoveSpeed;

    [SerializeField]
    private float m_Offset;

    #endregion

    #region Field

    private float m_TargetZ;
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
            m_TargetZ = Enemy.transform.position.z;
        }
        else
        {
            m_TargetZ = player.transform.position.z;
        }

        var delta = m_TargetZ - Enemy.transform.position.z;        
        m_MoveSumDistance = Mathf.Abs(delta + m_Offset);
        m_MoveDir = Mathf.Sign(delta);
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        var translateZ = m_MoveDir * m_ZMoveSpeed * Time.deltaTime;
        Enemy.transform.Translate(Vector3.forward * translateZ, Space.World);
        m_MoveSumDistance -= Mathf.Abs(translateZ);
    }

    protected override bool IsEnd()
    {
        return m_MoveSumDistance <= 0;
    }

    protected override void OnEnd()
    {
        base.OnEnd();

        var pos = Enemy.transform.position;
        pos.z = m_TargetZ + m_Offset;
        Enemy.transform.position = pos;
    }

    #endregion
}

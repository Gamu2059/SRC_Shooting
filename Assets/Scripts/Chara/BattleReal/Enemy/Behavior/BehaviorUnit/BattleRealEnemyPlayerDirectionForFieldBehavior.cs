#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイヤーに向かって飛んでくるが、フィールド領域ギリギリで静止する敵の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemySequence/Unit/PlayerDirectionForField", fileName = "player_direction_for_field.behavior_unit.asset", order = 1000)]
public class BattleRealEnemyPlayerDirectionForFieldBehavior : BattleRealEnemyBehaviorUnit
{
    #region Field Inspector

    [Header("Player Direction For Field Parameter")]

    [SerializeField, Tooltip("この振る舞いに掛かる時間")]
    private float m_Duration;
    protected float Duration => m_Duration;

    [SerializeField, Tooltip("現在座標から目標座標までの線形補間カーブ")]
    private AnimationCurve m_LerpForTargetCurve;
    protected AnimationCurve LerpForTargetCurve => m_LerpForTargetCurve;

    #endregion

    #region Field

    private Vector3 m_StartPosition;
    private Vector3 m_TargetPosition;

    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        m_StartPosition = Enemy.transform.position;

        var playerForward = Vector3.back;
        var player = BattleRealPlayerManager.Instance.Player;
        if (!player.IsDead)
        {
            playerForward = player.transform.position - m_StartPosition;
            playerForward.y = 0;
            playerForward.Normalize();
        }

        if (playerForward.x == 0 && playerForward.z == 0)
        {
            m_TargetPosition = m_StartPosition;
            return;
        }

        var maxField = BattleRealStageManager.Instance.MaxLocalFieldPosition;
        var minField = BattleRealStageManager.Instance.MinLocalFieldPosition;

        var maxXmaxZForward = maxField.ToVector3XZ() - m_StartPosition;
        var maxXminZForward = new Vector3(maxField.x, minField.y) - m_StartPosition;
        var minXmaxZForward = new Vector3(minField.x, maxField.y) - m_StartPosition;
        var minXminZForward = minField.ToVector3XZ() - m_StartPosition;

        var crossMaxXMaxZ = Vector3.Cross(maxXmaxZForward, playerForward).y;
        var crossMaxXMinZ = Vector3.Cross(maxXminZForward, playerForward).y;
        var crossMinXMaxZ = Vector3.Cross(minXmaxZForward, playerForward).y;
        var crossMinXMinZ = Vector3.Cross(minXminZForward, playerForward).y;

        var xTarget = 0f;
        var zTarget = 0f;
        if (crossMaxXMaxZ >= 0 && crossMaxXMinZ <= 0)
        {
            // 右側のX領域
            xTarget = maxField.x;
            zTarget = Mathf.Abs((xTarget - m_StartPosition.x) / playerForward.x) * playerForward.z;
            zTarget += m_StartPosition.z;
        }
        else if (crossMinXMinZ >= 0 && crossMinXMaxZ <= 0)
        {
            // 左側のX領域
            xTarget = minField.x;
            zTarget = Mathf.Abs((xTarget - m_StartPosition.x) / playerForward.x) * playerForward.z;
            zTarget += m_StartPosition.z;
        }
        else if (crossMaxXMaxZ < 0 && crossMinXMaxZ > 0)
        {
            // 上側のZ領域
            zTarget = maxField.y;
            xTarget = Mathf.Abs((zTarget - m_StartPosition.z) / playerForward.z) * playerForward.x;
            xTarget += m_StartPosition.x;
        }
        else
        {
            // 下側のZ領域
            zTarget = minField.y;
            xTarget = Mathf.Abs((zTarget - m_StartPosition.z) / playerForward.z) * playerForward.x;
            xTarget += m_StartPosition.x;
        }

        m_TargetPosition = new Vector3(xTarget, 0, zTarget);
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Move();
    }

    protected override bool IsEnd()
    {
        return CurrentTime >= Duration;
    }

    #endregion

    private void Move()
    {
        if (Duration <= 0)
        {
            return;
        }

        var rate = CurrentTime / Duration;
        var posLerp = LerpForTargetCurve.Evaluate(rate);
        var pos = Vector3.Lerp(m_StartPosition, m_TargetPosition, posLerp);
        Enemy.transform.position = pos;
    }
}

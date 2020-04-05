#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイヤーに向かって飛んでくる敵の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Unit/PlayerDirection", fileName = "param.behavior_unit.asset", order = 10)]
public class BattleRealEnemyPlayerDirectionBehavior : BattleRealEnemyBehaviorUnitBase
{
    #region Field Inspector

    [Header("Player Direction Parameter")]

    [SerializeField, Tooltip("生成された瞬間にプレイヤーの方向を向くかどうか")]
    private bool m_IsLookPlayerOnStart;
    public bool IsLookPlayerOnStart => m_IsLookPlayerOnStart;

    [SerializeField, Tooltip("ある時間での速度の関数")]
    private AnimationCurve m_SpeedCurve;
    public AnimationCurve SpeedCurve => m_SpeedCurve;

    [SerializeField, Tooltip("ある時間でのプレイヤーへ向かう割合の関数")]
    private AnimationCurve m_LerpPlayerCurve;
    public AnimationCurve LerpPlayerCurve => m_LerpPlayerCurve;

    #endregion

    #region Field

    protected float m_NowSpeed;
    protected float m_Duration;
    
    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        m_NowSpeed = 0;

        // 速度アニメーションの経過時間をキャッシュする
        m_Duration = SpeedCurve.Duration();

        if (IsLookPlayerOnStart)
        {
            if (BattleRealPlayerManager.Instance != null)
            {
                var player = BattleRealPlayerManager.Instance.Player;
                var pos = player.transform.position;
                pos.y = Enemy.transform.position.y;

                Enemy.transform.LookAt(pos);
            }
        }
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Move(deltaTime);
    }

    protected override bool IsEnd()
    {
        return CurrentTime >= m_Duration;
    }

    #endregion

    protected virtual void Move(float deltaTime)
    {
        var speed = SpeedCurve.Evaluate(CurrentTime);
        var lerp = LerpPlayerCurve.Evaluate(CurrentTime);

        var enemy = Enemy.transform;
        var player = BattleRealPlayerManager.Instance.Player.transform;
        var forward = enemy.forward;
        var playerForward = player.position - enemy.position;
        playerForward.y = 0;

        var dir = Vector3.LerpUnclamped(forward, playerForward, lerp);
        enemy.position = dir * speed * deltaTime + enemy.position;
        enemy.forward = dir;
    }
}

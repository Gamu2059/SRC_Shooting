#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using DG.Tweening;

/// <summary>
/// ワールド座標のある一点に向かって移動する敵の挙動
/// </summary>
[Serializable, CreateAssetMenu(menuName= "Param/BattleReal/Enemy/Behavior/Unit/WorldPosDirection", fileName = "param.behavior_unit.asset")]
public class BattleRealEnemyWorldPosDirectionBehavior : BattleRealEnemyBehaviorUnitBase
{
    #region Field Inspector

    [Header("World Pos Direction Parameter")]

    [SerializeField, Tooltip("目標地点")]
    private Vector3 m_TargetWorldPos;
    public Vector3 TargetWorldPos => m_TargetWorldPos;

    [SerializeField, Tooltip("生成された瞬間に目標地点の方向を向くかどうか")]
    private bool m_IsLookTargetOnStart;
    public bool IsLookTargetOnstart => m_IsLookTargetOnStart;

    [SerializeField, Tooltip("ある時間での速度の関数")]
    private AnimationCurve m_SpeedCurve;
    public AnimationCurve SpeedCurve => m_SpeedCurve;

    [SerializeField, Tooltip("ある時間での目標地点に向かう割合の関数")]
    private AnimationCurve m_LerpTargetCurve;
    public AnimationCurve LerpTargetCurve => m_LerpTargetCurve;

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
        m_Duration = SpeedCurve.Duration();

        if (IsLookTargetOnstart)
        {
            var pos = m_TargetWorldPos;
            pos.y = Enemy.transform.position.y;
            Enemy.transform.LookAt(pos);
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

    protected void Move(float deltaTime)
    {
        var speed = SpeedCurve.Evaluate(deltaTime);
        var lerp = LerpTargetCurve.Evaluate(deltaTime);

        var enemyT = Enemy.transform;
        var forward = enemyT.forward;
        var targetForward = m_TargetWorldPos - enemyT.position;
        targetForward.y = 0;

        var dir = Vector3.LerpUnclamped(forward, targetForward, lerp);
        enemyT.position = dir * speed * deltaTime + enemyT.position;
        enemyT.forward = dir;
    }
}

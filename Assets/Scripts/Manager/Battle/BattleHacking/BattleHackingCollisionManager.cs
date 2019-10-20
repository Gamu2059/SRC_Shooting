using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingCollisionManager : BattleCollisionManagerBase
{
    public static BattleHackingCollisionManager Instance => BattleHackingManager.Instance.CollisionManager;

    /// <summary>
    /// 衝突をチェックする。
    /// </summary>
    public override void CheckCollision()
    {
    }

    /// <summary>
    /// 衝突情報を描画する。
    /// </summary>
    public override void DrawCollider()
    {
        if (!BattleManager.Instance.m_IsDrawColliderArea)
        {
            return;
        }

        var bullets = BattleHackingBulletManager.Instance.Bullets;
        for (int i = 0; i < bullets.Count; i++)
        {
            DrawCollider(bullets[i]);
        }
    }

    protected override Vector2 CalcViewportPos(Vector2 worldPos)
    {
        if (BattleManager.Instance == null || BattleHackingStageManager.Instance == null)
        {
            return Vector2.one / 2f;
        }

        return BattleHackingStageManager.Instance.CalcViewportPosFromWorldPosition(worldPos.x, worldPos.y);
    }
}

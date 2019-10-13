using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingCollisionManager : BattleCollisionManagerBase
{
    public static BattleHackingCollisionManager Instance => BattleHackingManager.Instance.CollisionManager;



    protected override Vector2 CalcViewportPos(Vector2 worldPos)
    {
        if (BattleManager.Instance == null || BattleHackingStageManager.Instance == null)
        {
            return Vector2.one / 2f;
        }

        return BattleHackingStageManager.Instance.CalcViewportPosFromWorldPosition(worldPos.x, worldPos.y);
    }
}

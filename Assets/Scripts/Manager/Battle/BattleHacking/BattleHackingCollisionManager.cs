using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingCollisionManager : BattleCollisionManagerBase
{
    public static BattleHackingCollisionManager Instance => BattleHackingManager.Instance.CollisionManager;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハッキングの自機の位置を取得するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/readonly/playerPosition", fileName = "HackingPlayerPosition", order = 0)]
[System.Serializable]
public class HackingPlayerPosition : OperationVector2Base
{

    public override Vector2 GetResultVector2()
    {
        Vector3 playerPositionvec3 = BattleHackingPlayerManager.Instance.Player.transform.position;

        return new Vector2(playerPositionvec3.x, playerPositionvec3.z);
    }
}

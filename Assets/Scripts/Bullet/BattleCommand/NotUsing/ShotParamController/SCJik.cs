//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 発射パラメータを操作するクラス。発射角度が、発射位置から見て、自機を向くようにする。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/ShotParamControllerBase/Jik", fileName = "SCJik", order = 0)]
//[System.Serializable]
//public class SCJik : ShotParamControllerBase
//{

//    public override void GetshotParam(ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState state)
//    {
//        Vector3 playerPositionvec3 = BattleHackingPlayerManager.Instance.Player.transform.position;
//        Vector2 playerPositionVec2 = new Vector2(playerPositionvec3.x, playerPositionvec3.z);

//        //Vector2 enemyPositionVec2 = shotParam.Position.Value;
//        //Vector2 enemyPositionVec2 = shotParam.Position.GetResult();

//        //Vector2 relativePosition = playerPositionVec2 - enemyPositionVec2;
//        //float relativeAngle = Mathf.Atan2(relativePosition.y, relativePosition.x);
//        //relativeAngle %= Calc.TWO_PI;

//        //shotParam.Angle = relativeAngle;
//    }
//}




////shotParam = new ShotParam(shotParam.Position, relativeAngle, shotParam.Speed);

////return shotParam;

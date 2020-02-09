using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射パラメータを操作するクラス。発射角度を黄金角で回転させる。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/ShotParamControllerBase/Goa", fileName = "SCGoa", order = 0)]
[System.Serializable]
public class SCGoa : ShotParamControllerBase
{

    public override void GetshotParam(ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState state)
    {
        shotParam.Angle += Calc.GOLDEN_ANGLE * shotTimer.GetRealShotNum();
    }
}
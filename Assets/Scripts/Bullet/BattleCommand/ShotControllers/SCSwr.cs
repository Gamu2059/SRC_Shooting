using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射パラメータを操作するクラス。発射角度を一定速度で回転させる。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/ShotController/Swr", fileName = "SCSwr", order = 0)]
[System.Serializable]
public class SCSwr : ShotController
{

    [SerializeField, Tooltip("初期角度")]
    public float m_Angle;

    [SerializeField, Tooltip("角速度")]
    public float m_AngleSpeed;


    public override void GetshotParam(ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState1 state)
    {
        shotParam.Angle += m_Angle + m_AngleSpeed * shotTimer.GetLaunchTime();
    }
}




//shotParam = new ShotParam(shotParam.Position, shotParam.Angle + m_Angle + m_AngleSpeed * shotTimer.GetLaunchTime(), shotParam.Speed);

//return shotParam;

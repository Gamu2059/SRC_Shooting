using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射パラメータを操作するクラス。発射位置を一定速度で回転させる。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/ShotController/Rtp", fileName = "SCRtp", order = 0)]
[System.Serializable]
public class SCRtp : ShotController
{

    [SerializeField, Tooltip("半径")]
    public float m_Radius;

    [SerializeField, Tooltip("初期角度")]
    public float m_Angle;

    [SerializeField, Tooltip("角速度")]
    public float m_AngleSpeed;


    public override void GetshotParam(ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState1 state)
    {
        shotParam.Position.Value += Calc.RThetaToVec2(m_Radius, m_Angle + m_AngleSpeed * shotTimer.GetLaunchTime());
    }
}




//shotParam = new ShotParam(shotParam.Position + Calc.RThetaToVec2(m_Radius, m_Angle + m_AngleSpeed * shotTimer.GetLaunchTime()), shotParam.Angle, shotParam.Speed);

//shotParam.SetPosition(shotParam.GetPosition() + Calc.RThetaToVec2(m_Radius, m_Angle + m_AngleSpeed * shotTimer.GetLaunchTime()));

//return shotParam;

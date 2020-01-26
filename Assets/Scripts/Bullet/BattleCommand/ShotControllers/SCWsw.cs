using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射パラメータを操作するクラス。発射角度を一定速度で回転させる。ただし回転角は一周の分数倍。（WayとSwrを合わせている）
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/ShotController/Wsw", fileName = "SCWsw", order = 0)]
[System.Serializable]
public class SCWsw : ShotController
{

    [SerializeField, Tooltip("way数")]
    public int m_N;

    [SerializeField, Tooltip("一回の発射で何way分進むか")]
    public int m_M;


    public override void GetshotParam(ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState1 state)
    {
        int remainedM = m_M * shotTimer.GetRealShotNum() % m_N;

        shotParam.Angle += Calc.TWO_PI * remainedM / m_N;
    }
}




//shotParam = new ShotParam(shotParam.Position, shotParam.Angle + m_Angle + m_AngleSpeed * shotTimer.GetLaunchTime(), shotParam.Speed);

//return shotParam;

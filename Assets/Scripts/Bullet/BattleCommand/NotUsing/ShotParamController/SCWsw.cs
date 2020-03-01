//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 発射パラメータを操作するクラス。発射角度を一定速度で回転させる。ただし回転角は一周の分数倍。（WayとSwrを合わせている）
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/ShotParamControllerBase/Wsw", fileName = "SCWsw", order = 0)]
//[System.Serializable]
//public class SCWsw : ShotParamControllerBase
//{

//    [SerializeField, Tooltip("way数（分母）")]
//    public int m_Denominator;

//    [SerializeField, Tooltip("一回の発射で何way分進むか（分子）")]
//    public int m_Numerator;


//    public override void GetshotParam(ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState state)
//    {
//        //int remainedM = m_Numerator * shotTimer.GetRealShotNum() % m_Denominator;

//        //shotParam.Angle += Calc.TWO_PI * remainedM / m_Denominator;
//    }
//}




////shotParam = new ShotParam(shotParam.Position, shotParam.Angle + m_Angle + m_AngleSpeed * shotTimer.GetLaunchTime(), shotParam.Speed);

////return shotParam;

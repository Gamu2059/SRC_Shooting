//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 等加速度直線運動する軌道を表すクラス。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/TrajectoryBase/ConstAcceleLinearTrajectory", fileName = "ConstAcceleLinearTrajectory", order = 0)]
//[System.Serializable]
//public class ConstAcceleLinearTrajectory : TrajectoryBase
//{
//    [SerializeField, Tooltip("加速度の大きさ")]
//    public float m_Acceleration;


//    public override TransformSimple GetTransform(TrajectoryBasis trajectoryBasis, float time)
//    {
//        return new TransformSimple(
//            trajectoryBasis.m_Transform.m_Position + (trajectoryBasis.m_Speed * time + m_Acceleration * time * time / 2) * Calc.RThetaToVec2(1, trajectoryBasis.m_Transform.m_Angle),
//            trajectoryBasis.m_Transform.m_Angle,
//            trajectoryBasis.m_Transform.m_Scale
//            );
//    }
//}

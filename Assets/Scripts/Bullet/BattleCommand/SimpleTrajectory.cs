using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の軌道のクラス。
/// </summary>
public class SimpleTrajectory : object
{

    /// <summary>
    /// 基準の位置（発射位置）
    /// </summary>
    public TransformSimple m_BaseTransform;

    ///// <summary>
    ///// 初期角度
    ///// </summary>
    //public float m_Angle;

    /// <summary>
    /// 初速度の大きさ
    /// </summary>
    public float m_Speed;


    /// <summary>
    /// 一度でも画面外に出たら、弾を消去するかどうか。（いずれいらなくなりそう？そうでもない？）
    /// </summary>
    public bool m_IsDisappearWhenOut;

    /// <summary>
    /// 発射位置から画面端への最長距離
    /// </summary>
    public float m_LongestDistance;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SimpleTrajectory(TransformSimple baseTransform, float speed)
    {
        m_BaseTransform = baseTransform;
        m_Speed = speed;
    }


    /// <summary>
    /// 引数そのものになるコンストラクタ（継承先用）
    /// </summary>
    public SimpleTrajectory(SimpleTrajectory trajectoryBase) : this(trajectoryBase.m_BaseTransform, trajectoryBase.m_Speed)
    {

    }


    /// <summary>
    /// 弾の位置を取得する。
    /// </summary>
    public virtual TransformSimple GetTransform(float time)
    {
        return new TransformSimple(
            m_BaseTransform.m_Position + m_Speed * time * Calc.RThetaToVec2(1, m_BaseTransform.m_Angle),
            m_BaseTransform.m_Angle,
            m_BaseTransform.m_Scale
            );
    }


    /// <summary>
    /// 弾を消去するかどうか。
    /// </summary>
    public virtual bool IsCompletelyOut(float time, Vector3 basePosition)
    {
        return false;
    }
}




//m_LongestDistance = Calc.GetLongestDistance(basePosition);
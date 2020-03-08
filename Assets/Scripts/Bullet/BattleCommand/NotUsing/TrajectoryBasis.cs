using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 軌道情報のうち、必ず必要なものをまとめたクラス。
/// </summary>
public class TrajectoryBasis : object
{
    /// <summary>
    /// 発射した位置と角度とスケール
    /// </summary>
    public TransformSimple m_Transform;

    /// <summary>
    /// 初速度の大きさ
    /// </summary>
    public float m_Speed;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public TrajectoryBasis(TransformSimple baseTransform, float speed)
    {
        //m_Transform = new TransformSimple(baseTransform);
        m_Speed = speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の物理的な状態を表すクラス。
/// </summary>
public class TransformSimple : object
{
    /// <summary>
    /// 位置
    /// </summary>
    //public Vector2 Position { get; private set; }
    public Vector2 Position { get; set; }

    /// <summary>
    /// 回転角度
    /// </summary>
    //public float Angle { get; private set; }
    public float Angle { get; set; }

    /// <summary>
    /// 大きさ
    /// </summary>
    //public float Scale { get; private set; }
    public float Scale { get; set; }

    /// <summary>
    /// 不透明度
    /// </summary>
    //public float Opacity { get; private set; }
    public float Opacity { get; set; }

    /// <summary>
    /// 衝突判定があるかどうか
    /// </summary>
    //public bool CanCollide { get; private set; }
    public bool CanCollide { get; set; }

    /// <summary>
    /// 弾が生存しているかどうか
    /// </summary>
    //public bool IsAlive { get; private set; }
    public bool IsAlive { get; set; }


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public TransformSimple(
        Vector2 position,
        float angle,
        float scale,
        float opacity,
        bool canCollide,
        bool isAlive
        )
    {
        Position = position;
        Angle = angle;
        Scale = scale;
        Opacity = opacity;
        CanCollide = canCollide;
        IsAlive = isAlive;
    }
}





///// <summary>
///// コンストラクタ（クローン用）
///// </summary>
//public TransformSimple(TransformSimple transform) : this(transform.m_Position, transform.m_Angle, transform.m_Scale)
//{

//}


//public override string ToString()
//{
//    return m_Position.ToString() + ", " + m_Angle.ToString() + ", " + m_Scale.ToString();
//}

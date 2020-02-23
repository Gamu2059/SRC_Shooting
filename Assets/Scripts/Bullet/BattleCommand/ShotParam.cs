using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を発射する時のパラメータの具体的な値を表すクラス。
/// </summary>
[System.Serializable]
public class ShotParam : object
{

    /// <summary>
    /// 弾の見た目の種類
    /// </summary>
    [SerializeField]
    public int BulletIndex { set; get; }

    /// <summary>
    /// 基準の位置（発射位置）
    /// </summary>
    [SerializeField]
    public Vector2 Position { set; get; }

    /// <summary>
    /// 初期角度
    /// </summary>
    [SerializeField]
    public float Angle { set; get; }

    /// <summary>
    /// 初期の大きさ
    /// </summary>
    [SerializeField]
    public float Scale { set; get; }

    /// <summary>
    /// 速度ベクトル
    /// </summary>
    [SerializeField]
    public Vector2 Velocity { set; get; }

    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField]
    public float AngleSpeed { set; get; }

    /// <summary>
    /// 大きさの変化速度
    /// </summary>
    [SerializeField]
    public float ScaleSpeed { set; get; }


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ShotParam(int bulletIndex, Vector2 position, float velocityRad, float scale, Vector2 velocity, float angleSpeed, float scaleSpeed)
    {
        BulletIndex = bulletIndex;
        Position = position;
        Angle = velocityRad;
        Scale = scale;
        Velocity = velocity;
        AngleSpeed = angleSpeed;
        ScaleSpeed = scaleSpeed;
    }


    /// <summary>
    /// 慣性に従って動くとした時、与えられた時間分経過した時の物理的状態を取得する。
    /// </summary>
    public TransformSimple GetTransformInertially(float time)
    {
        return new TransformSimple(
            Position + Velocity * time,
            Angle + AngleSpeed * time,
            Scale + ScaleSpeed * time
            );
    }
}





//public void SetPosition(Vector2 position)
//{
//    Position = position;
//}


//public Vector2 GetPosition()
//{
//    return Position;
//}


//public virtual Vector2 Position
//{
//    set
//    {
//        m_Position = value;
//    }
//    get
//    {
//        return m_Position;
//    }
//}


//public ShotParam(int bulletIndex, OperationVector2Base1 position, float velocityRad, float speed)
//{
//    BulletIndex = bulletIndex;
//    Position = position;
//    Angle = velocityRad;
//    Speed = speed;
//}


//public ShotParam() : this(0, new OperationVector2Init(Vector2.zero), 0, 0)
//{

//}


//public ShotParam(ShotParam shotParam) : this(shotParam.BulletIndex, new OperationVector2Init(shotParam.Position.GetResult()), shotParam.Angle, shotParam.Speed)
//{

//}


///// <summary>
///// 初速度の大きさ
///// </summary>
//[SerializeField]
//public float Speed;


///// <summary>
///// 基準の位置（発射位置）
///// </summary>
//[SerializeField]
////public Boxing1<Vector2> Position;
//public OperationVector2Base1 Position;

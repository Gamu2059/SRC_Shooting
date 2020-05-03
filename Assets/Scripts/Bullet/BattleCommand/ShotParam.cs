using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を発射する時のパラメータの具体的な値を表すクラス。
/// </summary>
//[System.Serializable]
public class ShotParam : object
{

    /// <summary>
    /// 弾の見た目の種類
    /// </summary>
    [SerializeField]
    public int BulletIndex { get; private set; }

    /// <summary>
    /// 基準の位置（発射位置）
    /// </summary>
    [SerializeField]
    public Vector2 Position { get; private set; }

    /// <summary>
    /// 初期角度
    /// </summary>
    [SerializeField]
    public float Angle { get; private set; }

    /// <summary>
    /// 初期の大きさ
    /// </summary>
    [SerializeField]
    public float Scale { get; private set; }

    /// <summary>
    /// 速度ベクトル
    /// </summary>
    [SerializeField]
    public Vector2 Velocity { get; private set; }

    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField]
    public float AngleSpeed { get; private set; }

    /// <summary>
    /// 大きさの変化速度
    /// </summary>
    [SerializeField]
    public float ScaleSpeed { get; private set; }

    /// <summary>
    /// 弾の不透明度
    /// </summary>
    [SerializeField]
    public float Opacity { get; private set; }

    /// <summary>
    /// 衝突判定があるかどうか
    /// </summary>
    [SerializeField]
    public bool CanCollide { get; private set; }


    /// <summary>
    /// コンストラクタ（演算オブジェクトから）
    /// </summary>
    public ShotParam(ShotParamOperation shotParamOperation)
    {
        BulletIndex = shotParamOperation.BulletIndex.GetResultInt();
        Position = shotParamOperation.Position.GetResultVector2();
        Angle = shotParamOperation.Angle.GetResultFloat();
        Scale = shotParamOperation.Scale.GetResultFloat();
        Velocity = shotParamOperation.Velocity.GetResultVector2();
        AngleSpeed = shotParamOperation.AngleSpeed.GetResultFloat();
        ScaleSpeed = shotParamOperation.ScaleSpeed?.GetResultFloat() ?? 0;
        Opacity = shotParamOperation.Opacity?.GetResultFloat() ?? 1;
        CanCollide = shotParamOperation.CanCollide?.GetResultBool() ?? true;
    }


    /// <summary>
    /// 慣性に従って動くとした時、与えられた時間分経過した時の物理的状態を取得する。
    /// </summary>
    public TransformSimple GetTransformInertially(float time)
    {
        Vector2 position = Position + Velocity * time;

        return new TransformSimple(
            Position + Velocity * time,
            Angle + AngleSpeed * time,
            Scale + ScaleSpeed * time,
            Opacity,
            CanCollide,
            -0.91 <= position.x && position.x <= 0.91 && 
            -1.1 <= position.y && position.y <= 1.1
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


///// <summary>
///// コンストラクタ
///// </summary>
//public ShotParam(
//    int bulletIndex,
//    Vector2 position,
//    float velocityRad,
//    float scale,
//    Vector2 velocity,
//    float angleSpeed,
//    float? scaleSpeed,
//    float? opacity,
//    bool? canCollide
//    )
//{
//    BulletIndex = bulletIndex;
//    Position = position;
//    Angle = velocityRad;
//    Scale = scale;
//    Velocity = velocity;
//    AngleSpeed = angleSpeed;
//    ScaleSpeed = scaleSpeed ?? 0;
//    Opacity = opacity ?? 1;
//    CanCollide = canCollide ?? true;
//}

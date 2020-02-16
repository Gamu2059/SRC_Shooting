using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を発射する時のパラメータを表すクラス。
/// </summary>
[System.Serializable]
public class ShotParam : object
{

    /// <summary>
    /// 弾の見た目の種類
    /// </summary>
    [SerializeField]
    public int BulletIndex;

    /// <summary>
    /// 基準の位置（発射位置）
    /// </summary>
    [SerializeField]
    //public Boxing1<Vector2> Position;
    public OperationVector2Base1 Position;

    /// <summary>
    /// 基準の位置（発射位置）
    /// </summary>
    [SerializeField]
    //public Boxing1<Vector2> Position;
    public Vector2 ShotPosition;

    /// <summary>
    /// 初期角度
    /// </summary>
    [SerializeField]
    public float Angle;

    /// <summary>
    /// 初期の大きさ
    /// </summary>
    [SerializeField]
    public float Scale;

    ///// <summary>
    ///// 初速度の大きさ
    ///// </summary>
    //[SerializeField]
    //public float Speed;

    /// <summary>
    /// 速度ベクトル
    /// </summary>
    [SerializeField]
    public Vector2 Velocity;

    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField]
    public float AngleSpeed;

    /// <summary>
    /// 大きさの変化速度
    /// </summary>
    [SerializeField]
    public float ScaleSpeed;


    public ShotParam(int bulletIndex, Vector2 position, float velocityRad, float scale, Vector2 velocity, float angleSpeed, float scaleSpeed)
    {
        BulletIndex = bulletIndex;
        ShotPosition = position;
        Angle = velocityRad;
        Scale = scale;
        //Speed = speed;
        Velocity = velocity;
        AngleSpeed = angleSpeed;
        ScaleSpeed = scaleSpeed;
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

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
    public Boxing1<Vector2> Position;

    /// <summary>
    /// 初期角度
    /// </summary>
    [SerializeField]
    public float Angle;

    /// <summary>
    /// 初速度の大きさ
    /// </summary>
    [SerializeField]
    public float Speed;


    public ShotParam(int bulletIndex, Boxing1<Vector2> position, float velocityRad, float speed)
    {
        BulletIndex = bulletIndex;
        Position = position;
        Angle = velocityRad;
        Speed = speed;
    }


    public ShotParam() : this(0, new Boxing1<Vector2>(Vector2.zero), 0, 0)
    {

    }


    public ShotParam(ShotParam shotParam) : this(shotParam.BulletIndex, new Boxing1<Vector2>(shotParam.Position), shotParam.Angle, shotParam.Speed)
    {

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
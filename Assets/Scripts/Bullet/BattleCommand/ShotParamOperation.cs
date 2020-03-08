using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を発射する時のパラメータを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/shotParam/shotParam", fileName = "ShotParamOperation", order = 0)]
[System.Serializable]
public class ShotParamOperation : ScriptableObject
{

    /// <summary>
    /// 弾の見た目の種類
    /// </summary>
    [SerializeField]
    private OperationIntBase m_BulletIndex;
    public OperationIntBase BulletIndex
    {
        set { m_BulletIndex = value; }
        get { return m_BulletIndex; }
    }

    /// <summary>
    /// 発射位置
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Position;
    public OperationVector2Base Position
    {
        set { m_Position = value; }
        get { return m_Position; }
    }

    /// <summary>
    /// 発射角度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Angle;
    public OperationFloatBase Angle
    {
        set { m_Angle = value; }
        get { return m_Angle; }
    }

    /// <summary>
    /// 大きさ
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Scale;
    public OperationFloatBase Scale
    {
        set { m_Scale = value; }
        get { return m_Scale; }
    }

    /// <summary>
    /// 速度ベクトル
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Velocity;
    public OperationVector2Base Velocity
    {
        set { m_Velocity = value; }
        get { return m_Velocity; }
    }

    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_AngleSpeed;
    public OperationFloatBase AngleSpeed
    {
        set { m_AngleSpeed = value; }
        get { return m_AngleSpeed; }
    }

    /// <summary>
    /// 大きさの変化速度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_ScaleSpeed;
    public OperationFloatBase ScaleSpeed
    {
        set { m_ScaleSpeed = value; }
        get { return m_ScaleSpeed; }
    }


    /// <summary>
    /// 不透明度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Opacity;
    public OperationFloatBase Opacity
    {
        set { m_Opacity = value; }
        get { return m_Opacity; }
    }

    /// <summary>
    /// 衝突判定があるかどうか
    /// </summary>
    [SerializeField]
    private OperationBoolBase m_CanCollide;
    public OperationBoolBase CanCollide
    {
        set { m_CanCollide = value; }
        get { return m_CanCollide; }
    }
}





//public ShotParam(int bulletIndex, OperationVector2Base position, float velocityRad, float speed)
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
//public OperationFloatBase Speed;


//[field: UnityEngine.Serialization.FormerlySerializedAs("m_BulletIndex")]
//[field: SerializeField]
//public OperationIntBase BulletIndex { get; set; }

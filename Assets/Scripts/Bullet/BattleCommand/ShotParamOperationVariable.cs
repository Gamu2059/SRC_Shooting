using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を発射する時のパラメータの変数を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/shotParam/variable", fileName = "ShotParamOperationVariable", order = 0)]
[System.Serializable]
public class ShotParamOperationVariable : ScriptableObject
{

    /// <summary>
    /// 弾の見た目の種類
    /// </summary>
    [SerializeField]
    public OperationIntVariable BulletIndex;

    /// <summary>
    /// 発射位置
    /// </summary>
    [SerializeField]
    public OperationVector2Variable Position;

    /// <summary>
    /// 発射角度
    /// </summary>
    [SerializeField]
    public OperationFloatVariable Angle;

    /// <summary>
    /// 大きさ
    /// </summary>
    [SerializeField]
    public OperationFloatVariable Scale;

    ///// <summary>
    ///// 初速度の大きさ
    ///// </summary>
    //[SerializeField]
    //public OperationFloatVariable Speed;

    /// <summary>
    /// 速度ベクトル
    /// </summary>
    [SerializeField]
    public OperationVector2Variable Velocity;

    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField]
    public OperationFloatVariable AngleSpeed;

    /// <summary>
    /// 大きさの変化速度
    /// </summary>
    [SerializeField]
    public OperationFloatVariable ScaleSpeed;


    public void SetShotParam(ShotParam shotParam)
    {
        BulletIndex.Value = shotParam.BulletIndex;
        Position.Value = shotParam.ShotPosition;
        Angle.Value = shotParam.Angle;
        Scale.Value = shotParam.Scale;
        //Speed.Value = shotParam.Speed;
        Velocity.Value = shotParam.Velocity;
        AngleSpeed.Value = shotParam.AngleSpeed;
        ScaleSpeed.Value = shotParam.ScaleSpeed;
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

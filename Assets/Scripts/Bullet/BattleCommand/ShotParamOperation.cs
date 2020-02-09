using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を発射する時のパラメータを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/shotParam", fileName = "ShotParamOperation", order = 0)]
[System.Serializable]
public class ShotParamOperation : ScriptableObject
{

    /// <summary>
    /// 弾の見た目の種類
    /// </summary>
    [SerializeField]
    public OperationIntBase BulletIndex;

    /// <summary>
    /// 発射位置
    /// </summary>
    [SerializeField]
    public OperationVector2Base Position;

    /// <summary>
    /// 発射角度
    /// </summary>
    [SerializeField]
    public OperationFloatBase Angle;

    /// <summary>
    /// 大きさ
    /// </summary>
    [SerializeField]
    public OperationFloatBase Scale;

    /// <summary>
    /// 初速度の大きさ
    /// </summary>
    [SerializeField]
    public OperationFloatBase Speed;
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

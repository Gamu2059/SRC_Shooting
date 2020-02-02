using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射パラメータを操作するクラス。発射位置を、敵本体からの相対位置で指定する。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/ShotParamControllerBase/Bop", fileName = "SCBop", order = 0)]
[System.Serializable]
public class SCBop : ShotParamControllerBase
{

    //[SerializeField, Tooltip("相対位置")]
    //public Vector2 m_RelativePosition;


    public override void GetshotParam(ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState state)
    {
        //shotParam.Position.Value += state.GetTransform(shotTimer.GetLaunchTime()).m_Position;

        shotParam.Position = new OperationVector2Plus(new OperationVector2Base[]{ shotParam.Position, new OperationVector2Init(state.GetTransform(shotTimer.GetLaunchTime()).m_Position) });
    }
}




//shotParam = new ShotParam(state.GetTransform(shotTimer.GetLaunchTime()).m_Position + shotParam.Position, shotParam.Angle, shotParam.Speed);

//shotParam.SetPosition(state.GetTransform(shotTimer.GetLaunchTime()).m_Position + shotParam.GetPosition());

//return shotParam;

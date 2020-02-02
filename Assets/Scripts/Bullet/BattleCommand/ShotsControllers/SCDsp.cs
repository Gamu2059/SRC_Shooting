using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射パラメータを操作するクラス。速度差をつける。弾数が増える。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/ShotParamListControllerBase/Dsp", fileName = "SCDsp", order = 0)]
[System.Serializable]
public class SCDsp : ShotsControllerInc
{

    [SerializeField, Tooltip("速度の数")]
    public int m_SpeedNum;

    [SerializeField, Tooltip("速度差")]
    public float m_DSpeed;


    public override void GetShotParamListIn(List<ShotParam> array, ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState state)
    {
        for (int speedIndex = -(m_SpeedNum - 1); speedIndex <= m_SpeedNum - 1; speedIndex += 2)
        {
            ShotParam anotherShotParam = new ShotParam(shotParam);

            anotherShotParam.Speed += speedIndex * m_DSpeed / 2;

            array.Add(anotherShotParam);
        }
    }
}




//float newSpeed = shotParam.Speed + speedIndex * m_DSpeed / 2;

//array.Add(new ShotParam(shotParam.BulletIndex, shotParam.Position, shotParam.Angle, newSpeed));

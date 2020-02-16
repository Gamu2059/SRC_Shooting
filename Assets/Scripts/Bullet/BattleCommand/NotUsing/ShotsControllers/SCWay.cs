using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射パラメータを操作するクラス。全方位弾にする。弾数が増える。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/ShotParamListControllerBase/Way", fileName = "SCWay", order = 0)]
[System.Serializable]
public class SCWay : ShotsControllerInc
{

    [SerializeField, Tooltip("way数")]
    public int m_Way;


    public override void GetShotParamListIn(List<ShotParam> array, ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState state)
    {

        for (int wayIndex = 0; wayIndex < m_Way; wayIndex++)
        {
            //ShotParam anotherShotParam = new ShotParam(shotParam);

            //anotherShotParam.Angle += Calc.TWO_PI * wayIndex / m_Way;

            //array.Add(anotherShotParam);
        }
    }
}




//float newAngle = shotParam.Angle + Calc.TWO_PI * wayIndex / m_Way;

//array.Add(new ShotParam(shotParam.BulletIndex, shotParam.Position, newAngle, shotParam.Speed));


//public override List<ShotParam> GetshotsParam(List<ShotParam> array, ShotTimer shotTimer, HackingBossPhaseState1 state)
//{
//    int arraySize = array.Count;

//    for (int i = 0; i < arraySize; i++)
//    {
//        ShotParam shotParam = array[0];
//        array.RemoveAt(0);

//        for (int wayIndex = 0; wayIndex < m_Way; wayIndex++)
//        {
//            ShotParam anotherShotParam = new ShotParam(shotParam);

//            anotherShotParam.Angle += Calc.TWO_PI * wayIndex / m_Way;

//            array.Add(anotherShotParam);
//        }
//    }

//    return array;
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射パラメータ操作のクラス。リストをリストにする。一対多で対応するため、弾数が増える。
/// </summary>
public abstract class ShotsControllerInc : ShotParamListControllerBase
{
    public override void GetshotsParam(List<ShotParam> array, ShotTimer shotTimer, HackingBossPhaseState state)
    {
        int arraySize = array.Count;

        for (int i = 0; i < arraySize; i++)
        {
            ShotParam shotParam = array[0];
            array.RemoveAt(0);

            GetShotParamListIn(array, shotParam, shotTimer, state);
        }
    }


    public abstract void GetShotParamListIn(List<ShotParam> array, ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState state);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射パラメータ操作のクラス。リストをリストにする。一対一で対応するため、弾数は変わらない。
/// </summary>
public abstract class ShotsControllerBij : ShotsController
{
    public override void GetshotsParam(List<ShotParam> array, ShotTimer shotTimer, HackingBossPhaseState1 state)
    {
        int arraySize = array.Count;

        for (int i = 0; i < arraySize; i++)
        {
            GetshotsParamFor(array[i], shotTimer, state);
        }
    }


    public abstract void GetshotsParamFor(ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState1 state);
}

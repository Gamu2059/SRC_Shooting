using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射パラメータ操作のクラス。リストをリストにする。
/// </summary>
public abstract class ShotsController : ScriptableObject
{
    public abstract void GetshotsParam(List<ShotParam> array, ShotTimer shotTimer, HackingBossPhaseState1 state);
}





//[CreateAssetMenu(menuName = "Param/Danmaku/ShotController/SC", fileName = "SC", order = 0)]
//[System.Serializable]

#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DLisJik : System.Object
{

    [SerializeField, Tooltip("弾源がリサージュ曲線の単位弾幕")]
    private UDLisOld uDLis;

    [SerializeField, Tooltip("自機狙いの単位弾幕")]
    private UDJik uDJik;


    public void Updates(BattleRealEnemyBase enemyController,float time)
    {
        uDLis.Updates(enemyController, time);
        uDJik.Updates(enemyController, time);
    }
}

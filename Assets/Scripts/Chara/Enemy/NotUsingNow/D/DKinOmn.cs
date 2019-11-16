#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DKinOmn : System.Object
{

    [SerializeField, Tooltip("金閣寺の単位弾幕1")]
    private UDKin uDKin1;

    [SerializeField, Tooltip("金閣寺の単位弾幕2")]
    private UDKin uDKin2;

    [SerializeField, Tooltip("全方位弾の単位弾幕")]
    private UDOmn2 uDOmn;


    public void Updates(BattleRealEnemyController enemyController, float time)
    {
        uDKin1.Updates(enemyController,time);
        uDKin2.Updates(enemyController, time);
        uDOmn.Updates(enemyController, time);
    }
}

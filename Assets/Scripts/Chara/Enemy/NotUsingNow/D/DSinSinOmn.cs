using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DSinSinOmn : System.Object
{

    [SerializeField, Tooltip("全方位サインカーブの単位弾幕1")]
    private UDSin uDSin1;

    [SerializeField, Tooltip("全方位サインカーブの単位弾幕2")]
    private UDSin uDSin2;

    [SerializeField, Tooltip("全方位弾の単位弾幕")]
    private UDOmn2 uDOmn;


    public void Updates(BattleRealEnemyController enemyController, float time)
    {
        uDSin1.Updates(enemyController, time);
        uDSin2.Updates(enemyController, time);
        uDOmn.Updates(enemyController, time);
    }
}
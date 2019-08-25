using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DSinOmn : System.Object
{

    [SerializeField, Tooltip("全方位サインカーブの単位弾幕")]
    private UDSin uDSin;

    [SerializeField, Tooltip("全方位弾の単位弾幕")]
    private UDOmn2 uDOmn;


    public void Updates(EnemyController enemyController, float time)
    {
        uDSin.Updates(enemyController, time);
        uDOmn.Updates(enemyController, time);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DLisOmn : System.Object
{

    //[SerializeField, Tooltip("現在の形態内の時刻")]
    //private float m_Time;

    // LissajousAliceのクラス
    [SerializeField]
    private UDLis uDLis;

    // OmniDirectionのクラス
    [SerializeField]
    private UDOmn uDOmn;


    public void Updates(EnemyController enemyController, float time)
    {
        uDLis.Updates(enemyController, time);
        uDOmn.Updates(enemyController, time);
    }
}

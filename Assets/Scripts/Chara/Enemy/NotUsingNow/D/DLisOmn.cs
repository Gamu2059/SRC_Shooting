#pragma warning disable 0649

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
    private UDLisOld uDLis;

    // OmniDirectionのクラス
    [SerializeField]
    private UDOmn2 uDOmn;


    public void Updates(BattleRealEnemyController enemyController, float time)
    {
        uDLis.Updates(enemyController, time);
        uDOmn.Updates(enemyController, time);
    }
}

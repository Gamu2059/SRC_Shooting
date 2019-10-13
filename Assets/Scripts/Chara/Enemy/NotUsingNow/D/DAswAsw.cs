using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DAswAsw : System.Object
{

    //[SerializeField, Tooltip("現在の形態内の時刻")]
    //private float m_Time;

    //[SerializeField, Tooltip("形態内の時刻の最大値")]
    //private float m_TimeMax;

    // AliceSwirlSerのクラス
    [SerializeField, Tooltip("弾源渦巻きの単位弾幕1")]
    private UDAsw uDAsw1;

    // AliceSwirlSerのクラス
    [SerializeField, Tooltip("弾源渦巻きの単位弾幕2")]
    private UDAsw uDAsw2;

    // OmniDirectionのクラス
    [SerializeField, Tooltip("全方位弾の単位弾幕")]
    private UDOmn2 uDOmn;


    public void Updates(BattleRealEnemyController enemyController,float time)
    {
        //// 経過時間を進める
        //m_Time += Time.deltaTime;

        //// 時刻が範囲外かどうか
        //if (m_TimeMax < m_Time)
        //{ 
        //    // 経過時間を正しくする
        //    m_Time -= m_TimeMax;
        //}

        // それぞれの弾幕について、その射出時間内なら、その弾幕を撃つ。
        uDAsw1.Updates(enemyController, time);
        uDAsw2.Updates(enemyController, time);
        uDOmn.Updates(enemyController, time);
    }
}

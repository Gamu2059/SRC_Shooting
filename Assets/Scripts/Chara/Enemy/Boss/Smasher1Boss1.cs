using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ある1つのボスのクラス。
[System.Serializable]
public class Smasher1Boss1 : System.Object
{

    [SerializeField, Tooltip("弾幕の種類")]
    private int m_AttackNum;

    [SerializeField, Tooltip("その弾幕の開始からの経過時間")]
    protected float m_Time;

    [SerializeField, Tooltip("攻撃を変える")]
    private bool m_AttackChange;

    [SerializeField, Tooltip("金閣寺と全方位弾の弾幕")]
    private DKinOmn dKinOmn;

    [SerializeField, Tooltip("リサージュと自機狙いの弾幕")]
    private DLisJik dLisJik;

    [SerializeField, Tooltip("弾源渦巻きと全方位弾の弾幕")]
    private DAswAsw dAswAsw;

    [SerializeField, Tooltip("サインカーブと全方位弾の弾幕")]
    private DSinOmn dSinOmn;


    public void Updates(EnemyController enemyController, float time)
    {
        if (m_AttackChange)
        {
            // これだと1フレーム攻撃が停止することになる

            m_Time = 0;

            m_AttackNum++;

            m_AttackChange = false;
        }
        else
        {
            // 時間を進める
            m_Time += Time.deltaTime;

            switch (m_AttackNum)
            {
                case 0:
                    dKinOmn.Updates(enemyController, m_Time);
                    break;

                case 1:
                    dLisJik.Updates(enemyController, m_Time);
                    break;

                case 2:
                    dAswAsw.Updates(enemyController, m_Time);
                    break;

                case 3:
                    dSinOmn.Updates(enemyController, m_Time);
                    break;
            }
        }
    }
}
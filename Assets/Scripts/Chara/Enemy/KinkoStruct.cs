using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinkoStruct : BattleRealEnemyController
{

    // 発射
    [SerializeField]
    private IntervalStruct shot;

    // 角度
    [SerializeField]
    private structDdt m_Angle;

    // 位置
    [SerializeField]
    private Vector3 m_BasePos;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;


    // Start is called before the first frame update
    private void Awake()
    { 
        m_Angle.set(0,1);
        shot.set(1000f,0);
    }

    // Update is called once per frame
    void Update()
    {
        // 角度を進める
        //m_Angle.plus();
        m_Angle.it = m_Angle.ddt * Time.time;
        m_Angle.modulo2PI();

        // 角度を反映させる
        m_Angle.reflectEulerAngles(transform.eulerAngles);

        shot.proceed();

        ShotOrNot();
    }

    private void ShotOrNot()
    {

        if (shot.timeIsMinus())
        {

            // 現在は、本来撃たれた時刻からこれだけ経っている。
            float dTime = shot.getMinusTime();
            shot.reload();

            // 本来撃たれた時刻での角度
            float pastRad = m_Angle.getGoBack(dTime);

            // 弾が撃たれてから弾が進んだ距離
            float distance = m_BulletSpeed * dTime;

            // 発射された弾の位置
            Vector3 pos = new Vector3(distance * Mathf.Cos(pastRad), 0, distance * Mathf.Sin(pastRad)) + m_BasePos;

            // 敵の向きを発射時のものにする
            Vector3 pastAngle = transform.eulerAngles;
            pastAngle.y = -(pastRad * Mathf.Rad2Deg) + 90;

            // 弾を撃つ
            //ShotBullet(0, 0, pos, pastAngle);

            ShotOrNot();
        }
    }
}
#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koinomeiro : BattleRealEnemyBase
{

    // 発射間隔
    [SerializeField]
    private float shotInterval;

    // 次撃つまでの時間
    [SerializeField]
    private float shotTime;

    // 現在の向いている角度
    [SerializeField]
    private float nowRad;

    // 角速度
    [SerializeField]
    private float m_AngleSpeed;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;

    // 弾を撃っているか、休んでいるか
    [SerializeField]
    private bool shootingOrnot;

    // 連続して弾を撃つ回数
    [SerializeField]
    private int numShot;

    // 現在の連続して弾を撃っている回数
    [SerializeField]
    private int nowNumShot;

    // 休憩時間の長さ
    [SerializeField]
    private float restTime;

    // 現在の残りの休憩時間
    [SerializeField]
    private float leftRestTime;


    protected override void Awake()
    {
        shotTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // 角度を進める
        nowRad += m_AngleSpeed * Time.deltaTime;
        nowRad %= Mathf.PI * 2;

        // 角度を反映させる
        Vector3 angle = transform.eulerAngles;
        angle.y = -(nowRad * Mathf.Rad2Deg) + 90;
        transform.eulerAngles = angle;

        // 連続発射中なら
        if (shootingOrnot)
        {
            shotTime -= Time.deltaTime;

            ShotOrNot();
        }

        // 休憩中なら
        else
        {
            leftRestTime -= Time.deltaTime;

            if(leftRestTime < 0)
            {
                shootingOrnot = true;
            }
        }

    }

    private void ShotOrNot()
    {

        if (shotTime < 0f)
        {

            // 現在は、本来撃たれた時刻からこれだけ経っている。
            float dTime = -shotTime;
            shotTime += shotInterval;

            // 本来撃たれた時刻での角度
            float pastRad = nowRad - m_AngleSpeed * dTime;

            // 弾が撃たれてから弾が進んだ距離
            float distance = m_BulletSpeed * dTime;

            // 発射された弾の位置
            Vector3 pos = new Vector3(distance * Mathf.Cos(pastRad), 0, distance * Mathf.Sin(pastRad)) + transform.position;

            // 敵の向きを発射時のものにする
            Vector3 pastAngle = transform.eulerAngles;
            pastAngle.y = -(pastRad * Mathf.Rad2Deg) + 90;

            // 弾を撃つ
            //ShotBullet(0, 0, pos, pastAngle);

            if(nowNumShot <= 0)
            {
                shootingOrnot = false;
                leftRestTime = restTime;
            }
            else
            {
                ShotOrNot();
            }
        }
    }
}
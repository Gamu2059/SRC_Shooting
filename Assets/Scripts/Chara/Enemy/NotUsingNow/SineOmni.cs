#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineOmni : BattleRealEnemyController
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

    // 位置
    [SerializeField]
    private Vector3 basePos;

    // 弾の速さ
    [SerializeField]
    private float bulletSpeed = 10;

    // way数
    [SerializeField]
    private int way;

    // 発射地点の円の半径
    [SerializeField]
    private float circleRadius;

    // 角速度
    [SerializeField]
    private float angleSpeed;

    protected override void Awake()
    {
        shotTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // 角度を進める
        nowRad += angleSpeed * Time.deltaTime;
        nowRad %= Mathf.PI * 2;

        // 角度を反映させる
        Vector3 angle = transform.eulerAngles;
        angle.y = -(nowRad * Mathf.Rad2Deg) + 90;
        transform.eulerAngles = angle;

        shotTime -= Time.deltaTime;

        ShotOrNot();
    }

    private void ShotOrNot()
    {

        if (shotTime < 0f)
        {

            // 現在は、本来撃たれた時刻からこれだけ経っている。
            float dTime = -shotTime;
            shotTime += shotInterval;

            // 弾が撃たれてから弾が進んだ距離
            float distance = bulletSpeed * dTime;

            // 本来撃たれた時刻での角度
            float pastRad = nowRad - angleSpeed * dTime;

            for (int i = 0; i < way; i++)
            {
                // way数による角度
                float wayRad = Mathf.PI * i / way + Mathf.PI / way / 2;

                // 正弦波のどの位置か(1次元に投影した時の位置)
                float phase = Mathf.Cos(pastRad - wayRad);

                // 発射された弾の現在の位置
                Vector3 pos = basePos;
                pos += new Vector3(circleRadius * phase * Mathf.Cos(wayRad), 0, circleRadius * phase * Mathf.Sin(wayRad));
                pos += new Vector3(distance * Mathf.Cos(wayRad + Mathf.PI / 2), 0, distance * Mathf.Sin(wayRad + Mathf.PI / 2));

                // その弾の発射角度
                Vector3 tempAngle = transform.eulerAngles;
                tempAngle.y = -((wayRad + Mathf.PI / 2) * Mathf.Rad2Deg) + 90;

                // 弾を撃つ
                //ShotBullet(0, 0, pos, tempAngle);

                // 発射された弾の現在の位置
                pos = basePos;
                pos += new Vector3(circleRadius * phase * Mathf.Cos(wayRad), 0, circleRadius * phase * Mathf.Sin(wayRad));
                pos += new Vector3(distance * Mathf.Cos(wayRad - Mathf.PI / 2), 0, distance * Mathf.Sin(wayRad - Mathf.PI / 2));

                // その弾の発射角度
                tempAngle = transform.eulerAngles;
                tempAngle.y = -((wayRad - Mathf.PI / 2) * Mathf.Rad2Deg) + 90;

                // 弾を撃つ
                //ShotBullet(0, 0, pos, tempAngle);
            }

            ShotOrNot();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDSin : DanmakuCountAbstract
{

    private enum INT
    {
        何番目の弾か,
        way数,
    }


    private enum FLOAT
    {
        発射間隔,
        弾源円半径,
        角速度,
        前フレームの位相,
        弾速,
    }


    private enum VECTOR3
    {
        発射位置,
    }


    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum(float time)
    {
        return time / m_Float[(int)FLOAT.発射間隔];
    }


    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return m_Float[(int)FLOAT.発射間隔] * m_RealShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(BattleRealEnemyController enemyController, float launchTime, float dTime)
    {

        float pastRad = m_Float[(int)FLOAT.角速度] * launchTime;
        pastRad = Modulo2PI(pastRad);


        pastRad = m_Float[(int)FLOAT.前フレームの位相] + m_Float[(int)FLOAT.発射間隔] * m_Float[(int)FLOAT.角速度];

        m_Float[(int)FLOAT.前フレームの位相] = pastRad;


        //float distance = m_Float[(int)FLOAT.弾速] * dTime;

        for (int i = 0; i < m_Int[(int)INT.way数]; i++)
        {
            // way数による角度
            float wayRad = Mathf.PI * i / m_Int[(int)INT.way数] + Mathf.PI / m_Int[(int)INT.way数] / 2;

            // 正弦波のどの位置か(1次元に投影した時の位置)
            float phase = Mathf.Cos(pastRad - wayRad);

            Vector3 pos = enemyController.transform.position;

            pos += m_Vector3[(int)VECTOR3.発射位置];
            pos += new Vector3(m_Float[(int)FLOAT.弾源円半径] * phase * Mathf.Cos(wayRad), 0, m_Float[(int)FLOAT.弾源円半径] * phase * Mathf.Sin(wayRad));
            //pos += new Vector3(distance * Mathf.Cos(wayRad + Mathf.PI / 2), 0, distance * Mathf.Sin(wayRad + Mathf.PI / 2));

            //Vector3 eulerAngles;

            //eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles,wayRad + Mathf.PI / 2);

            // 弾を撃つ
            //BulletShotParam bulletShotParam = new BulletShotParam(enemyController, m_Int[(int)INT.何番目の弾か], m_Int[(int)INT.何番目の弾パラメータか], 0, pos, eulerAngles, enemyController.transform.localScale);
            //BulletController.ShotBullet(bulletShotParam);

            ShotTouchokuBullet(enemyController, m_Int[(int)INT.何番目の弾か], pos, wayRad + Mathf.PI / 2, m_Float[(int)FLOAT.弾速], dTime);


            pos = enemyController.transform.position;
            pos += m_Vector3[(int)VECTOR3.発射位置];
            pos += new Vector3(m_Float[(int)FLOAT.弾源円半径] * phase * Mathf.Cos(wayRad), 0, m_Float[(int)FLOAT.弾源円半径] * phase * Mathf.Sin(wayRad));
            //pos += new Vector3(distance * Mathf.Cos(wayRad - Mathf.PI / 2), 0, distance * Mathf.Sin(wayRad - Mathf.PI / 2));

            //eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles,wayRad - Mathf.PI / 2);

            // 弾を撃つ
            //bulletShotParam = new BulletShotParam(enemyController, m_Int[(int)INT.何番目の弾か], m_Int[(int)INT.何番目の弾パラメータか], 0, pos, eulerAngles, enemyController.transform.localScale);
            //BulletController.ShotBullet(bulletShotParam);

            ShotTouchokuBullet(enemyController, m_Int[(int)INT.何番目の弾か], pos, wayRad - Mathf.PI / 2, m_Float[(int)FLOAT.弾速], dTime);
        }
    }
}

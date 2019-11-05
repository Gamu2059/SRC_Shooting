﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等速直線運動する弾を発射するときのパラメータのクラス。
/// </summary>
public class CVLMShotParam : object
{

    /// <summary>
    /// 敵本体。
    /// </summary>
    public BattleRealEnemyController m_EnemyController;

    /// <summary>
    /// 弾の外見のインデックス。
    /// </summary>
    public int m_BulletIndex;

    /// <summary>
    /// 発射する位置。
    /// </summary>
    public Vector3 m_Position;

    /// <summary>
    /// 発射する速度の向き。
    /// </summary>
    public float m_VelocityRad;

    /// <summary>
    /// 発射する速さ。
    /// </summary>
    public float m_Speed;

    /// <summary>
    /// 発射されてからの時間。
    /// </summary>
    public float m_DTime;


    /// <summary>
    /// インスタンスを生成する。引数がこのクラスの変数だけのコンストラクタも作るか。継承先用に。
    /// </summary>
    public CVLMShotParam(BattleRealEnemyController enemyController, int bulletIndex, Vector3 position, float velocityRad, float speed, float dTime)
    {
        m_EnemyController = enemyController;
        m_BulletIndex = bulletIndex;
        m_Position = position;
        m_VelocityRad = velocityRad;
        m_Speed = speed;
        m_DTime = dTime;
    }


    public CVLMShotParam(CVLMShotParam cVLMShotParam)
        : this(cVLMShotParam.m_EnemyController, cVLMShotParam.m_BulletIndex, cVLMShotParam.m_Position, cVLMShotParam.m_VelocityRad, cVLMShotParam.m_Speed, cVLMShotParam.m_DTime)
    {

    }


    /// <summary>
    /// 発射位置に引数の値を足す。（相対的なsetPositionみたいなもの？）
    /// </summary>
    public void PlusPosition(Vector3 position)
    {
        m_Position += position;
    }


    /// <summary>
    /// 弾を撃つ。（引数なし）（{}でなく=>でいける？）
    /// </summary>
    public virtual void Shoot()
    {
        Shoot(m_EnemyController,m_BulletIndex,m_Position,m_VelocityRad,m_Speed,m_DTime);
    }


    /// <summary>
    /// 弾を撃つ。（引数最大）
    /// </summary>
    public void Shoot(BattleRealEnemyController enemyController, int bulletIndex, Vector3 position, float velocityRad, float speed, float dTime)
    {
        Vector3 realPosition = position + speed * dTime * new Vector3(Mathf.Cos(velocityRad), 0, Mathf.Sin(velocityRad));

        Vector3 eulerAngles = Calc.CalcEulerAngles(enemyController.transform.eulerAngles, velocityRad);

        // 弾の大きさを変えている。
        BulletShotParam bulletShotParam = new BulletShotParam(enemyController, bulletIndex, Mathf.RoundToInt(speed * 10 - 1), 0, realPosition, eulerAngles, Vector3.one * 0.03f);
        BulletController.ShotBullet(bulletShotParam);
    }

    /// <summary>
    /// 弾を撃つ。（位置と角度と速さをいじる）
    /// </summary>
    public void ShootDpoDraDsp(Vector3 dPosition, float dVelocityRad, float dSpeed)
    {
        Shoot(
            m_EnemyController,
            m_BulletIndex,
            m_Position + dPosition,
            m_VelocityRad + dVelocityRad,
            m_Speed + dSpeed,
            m_DTime
            );
    }


    /// <summary>
    /// 弾を撃つ。（角度をいじる）
    /// </summary>
    public void ShootDra(float dVelocityRad)
    {
        Shoot(
            m_EnemyController,
            m_BulletIndex,
            m_Position,
            m_VelocityRad + dVelocityRad,
            m_Speed,
            m_DTime
            );
    }
}




//Vector3 realPosition = m_Position + m_Speed * m_DTime * new Vector3(Mathf.Cos(m_VelocityRad), 0, Mathf.Sin(m_VelocityRad));

//Vector3 eulerAngles = Calc.CalcEulerAngles(m_EnemyController.transform.eulerAngles, m_VelocityRad);

//// 弾の大きさを変えている。
//BulletShotParam bulletShotParam = new BulletShotParam(m_EnemyController, m_BulletIndex, Mathf.RoundToInt(m_Speed * 10 - 1), 0, realPosition, eulerAngles, Vector3.one * 0.03f);
//BulletController.ShotBullet(bulletShotParam);
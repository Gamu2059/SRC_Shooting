using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等速直線運動する弾を発射するときのクラス。
/// </summary>
public class CVLM : object
{

    /// <summary>
    /// 敵本体。
    /// </summary>
    protected BattleHackingBossBehavior m_EnemyController;

    /// <summary>
    /// 弾の外見のインデックス。
    /// </summary>
    protected int m_BulletIndex;

    /// <summary>
    /// 発射する位置。
    /// </summary>
    protected Vector3 m_Position;
    public Vector3 Position => m_Position;

    /// <summary>
    /// 発射する速度の向き。
    /// </summary>
    protected float m_VelocityRad;

    /// <summary>
    /// 発射する速さ。
    /// </summary>
    protected float m_Speed;

    /// <summary>
    /// 発射されてからの時間。
    /// </summary>
    protected float m_DTime;


    /// <summary>
    /// 引数最多のコンストラクタ。
    /// </summary>
    public CVLM(BattleHackingBossBehavior enemyController, int bulletIndex, Vector3 position, float velocityRad, float speed, float dTime)
    {
        m_EnemyController = enemyController;
        m_BulletIndex = bulletIndex;
        m_Position = position;
        m_VelocityRad = velocityRad;
        m_Speed = speed;
        m_DTime = dTime;
    }


    /// <summary>
    /// 引数が自分だけのコンストラクタ。（継承先用）
    /// </summary>
    public CVLM(CVLM cVLMShotParam)
        : this(cVLMShotParam.m_EnemyController, cVLMShotParam.m_BulletIndex, cVLMShotParam.m_Position, cVLMShotParam.m_VelocityRad, cVLMShotParam.m_Speed, cVLMShotParam.m_DTime)
    {

    }


    /// <summary>
    /// 発射位置に引数の値を足す。
    /// </summary>
    public void PlusPosition(Vector3 position)
    {
        m_Position += position;
    }


    /// <summary>
    /// 発射角度に引数の値を足す。
    /// </summary>
    public void PlusAngle(float angle)
    {
        m_VelocityRad += angle;
    }


    /// <summary>
    /// 弾を撃つ。（引数なし）
    /// </summary>
    public virtual void Shoot()
        =>
        Shoot(m_EnemyController,m_BulletIndex,m_Position,m_VelocityRad,m_Speed,m_DTime);


    /// <summary>
    /// 弾を撃つ。（引数最多）
    /// </summary>
    public void Shoot(BattleHackingBossBehavior enemyController, int bulletIndex, Vector3 position, float velocityRad, float speed, float dTime)
    {
        Vector3 realPosition = position + speed * dTime * new Vector3(Mathf.Cos(velocityRad), 0, Mathf.Sin(velocityRad));

        Vector3 eulerAngles = Calc.CalcEulerAngles(enemyController.GetEnemy().transform.eulerAngles, velocityRad);

        // 弾の大きさを変えている。
        CommandBulletShotParam bulletShotParam = new CommandBulletShotParam(enemyController.GetEnemy(), bulletIndex, Mathf.RoundToInt(speed * 5 - 1), 0, realPosition, eulerAngles, Vector3.one * 0.8f);
        var bullet = enemyController.Shot(bulletShotParam);
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


    /// <summary>
    /// 弾を撃つ。（位置と角度をいじる）
    /// </summary>
    public void ShootDpoDra(Vector3 dPosition, float dVelocityRad)
    {
        Shoot(
            m_EnemyController,
            m_BulletIndex,
            m_Position + dPosition,
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
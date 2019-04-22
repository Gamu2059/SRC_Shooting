using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントの通常敵。
/// </summary>
public class CommandEnemyNormal : CommandEnemyController
{
    /// <summary>
    /// 発射間隔
    /// </summary>
    [SerializeField]
    private float m_ShotInterval;

    private float m_ShotTimeCount;

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_ShotTimeCount = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_ShotTimeCount += Time.deltaTime;
        if (m_ShotTimeCount >= m_ShotInterval)
        {
            m_ShotTimeCount = 0;
            CommandBulletController.ShotBullet(this);
        }
    }
}

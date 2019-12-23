using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NWayPointMoveEnemy : PointMoveEnemy
{
    protected int m_WayNum;
    protected float m_WayRadius;
    protected float m_WayAngleOffset;

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();

        if(BehaviorParamSet is NWayPointMoveEnemyParamSet paramSet)
        {
            m_WayNum = paramSet.Num;
            m_WayRadius = paramSet.Radius;
            m_WayAngleOffset = paramSet.AngleOffset;
        }
    }

    protected override void OnShot(EnemyShotParam param)
    {
        if (m_WayNum < 1)
        {
            return;
        }

        var spreadAngles = GetBulletSpreadAngles(param.Num, param.BetweenBulletsAngle, param.ShotAngle);
        var shotParam = new BulletShotParam(this);

        for (int i = 0; i < m_WayNum; i++)
        {
            // transform.eulerAnglesは座標系が逆なので負の数にする
            var posAngle = (360 / m_WayNum) * i + m_WayAngleOffset - transform.eulerAngles.y;
            // 敵の正面を0°にする
            posAngle += 90;

            var x = Mathf.Cos(posAngle * Mathf.Deg2Rad) * m_WayRadius;
            var z = Mathf.Sin(posAngle * Mathf.Deg2Rad) * m_WayRadius;
            shotParam.Position = transform.position + new Vector3(x, 0, z);

            var shotAngle = 90 - posAngle;

            for (int j = 0; j < param.Num; j++)
            {
                var bullet = BulletController.ShotBullet(shotParam);
                bullet.SetRotation(new Vector3(0, spreadAngles[j] + shotAngle, 0));
            }
        }

        AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot01Se);
    }
}

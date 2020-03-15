#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// アニメーションカーブによって動く敵のNway版。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/NwayAnimationCurve", fileName = "param.battle_real_enemy_behavior.asset")]
public class NwayAnimationCurveBehavior : AnimationCurveBehavior
{
    #region Field Inspector

    [Header("N way Param")]

    [SerializeField]
    private int m_Num = default;
    public int Num => m_Num;

    [SerializeField]
    private float m_Radius = default;
    public float Radius => m_Radius;

    [SerializeField]
    private float m_AngleOffset = default;
    public float AngleOffset => m_AngleOffset;

    #endregion

    #region Field

    protected int m_WayNum;
    protected float m_WayRadius;
    protected float m_WayAngleOffset;

    #endregion

    protected override void OnShot(EnemyShotParam param)
    {
        if (m_WayNum < 1)
        {
            return;
        }

        var transform = m_Enemy.transform;
        var spreadAngles = BattleRealCharaController.GetBulletSpreadAngles(param.Num, param.Angle);
        var shotParam = new BulletShotParam(m_Enemy);

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

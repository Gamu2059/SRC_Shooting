using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingFreeTrajectoryBulletController : BattleHackingBulletController
{

    public override void OnUpdate()
    {
        transform.localPosition += Time.deltaTime * new Vector3(1,0,0);
    }
}




///// <summary>
///// 発射したキャラの何番目の弾か。
///// </summary>
//public int m_BulletIndex;

///// <summary>
///// 発射時の敵本体の速度
///// </summary>
//private Vector3 m_BaseVelocity;

///// <summary>
///// 発射時の敵本体の加速度
///// </summary>
//private Vector3 m_BaseAcceleration;


///// <summary>
///// 発射してからの経過時間
///// </summary>
//public float m_Time;

///// <summary>
///// 発射時の敵本体の位置
///// </summary>
//public Vector3 m_BasePosition;

///// <summary>
///// 発射角度（軌道別パラメータだけど今だけここに置く）
///// </summary>
//public float m_ShotAngle;

///// <summary>
///// 弾速（これは軌道別パラメータ？）
///// </summary>
//public float m_Speed;


//transform.localPosition = m_BasePosition + m_Time * m_Speed * new Vector3(Mathf.Cos(m_ShotAngle),0, Mathf.Sin(m_ShotAngle));
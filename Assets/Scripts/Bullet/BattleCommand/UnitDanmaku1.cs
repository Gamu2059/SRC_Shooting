using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単位弾幕を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/UnitDanmaku", fileName = "UnitDanmaku", order = 0)]
[System.Serializable]
public class UnitDanmaku1 : ScriptableObject
{

    [SerializeField, Tooltip("発射パラメータ操作の配列（単数→単数）")]
    private ShotController[] m_ShotControllerArray;

    [SerializeField, Tooltip("発射操作の配列（リスト→リスト）")]
    private ShotsController[] m_ShotsControllerArray;

    [SerializeField, Tooltip("発射パラメータの初期値")]
    private ShotParam m_ShotParam;

    [SerializeField, Tooltip("発射タイミングオブジェクト")]
    private ShotTimer m_ShotTimer;

    [SerializeField, Tooltip("軌道の決め方のオブジェクト")]
    private TrajectoryBase m_Trajectory;


    public void OnStarts()
    {
        // 発射タイミングオブジェクトの初期の処理をする
        m_ShotTimer.OnStarts();

        // 発射パラメータ初期値の位置を決める（ボクシングされているので）
        m_ShotParam.Position = new Boxing1<Vector2>(new Vector2(0, 0.5f));
    }


    public void OnUpdates(BattleHackingBossBehavior boss, HackingBossPhaseState1 state)
    {
        m_ShotTimer.OnUpdates();

        while(m_ShotTimer.HasNextAndNext())
        {
            ShotParam sP = new ShotParam(m_ShotParam);
            for (int i = 0; i < m_ShotControllerArray.Length; i++)
            {
                m_ShotControllerArray[i].GetshotParam(sP, m_ShotTimer, state);
            }

            List<ShotParam> shotParamList = new List<ShotParam>() { sP };
            for (int i = 0; i < m_ShotsControllerArray.Length; i++)
            {
                m_ShotsControllerArray[i].GetshotsParam(shotParamList, m_ShotTimer, state);
            }

            foreach (ShotParam shotParam in shotParamList)
            {
                // 弾を撃つ
                CommandBulletShotParam bulletShotParam = new CommandBulletShotParam(boss.GetEnemy(), shotParam.BulletIndex, 0, 0, Vector3.zero, Vector3.zero, Vector3.zero);
                BattleHackingFreeTrajectoryBulletController.ShotBullet(
                    bulletShotParam,
                    new SimpleTrajectory(
                        shotParam
                        ),
                    m_ShotTimer.GetDTime(),
                    new TrajectoryBasis(
                        new TransformSimple(
                            shotParam.Position.m_Value,
                            shotParam.Angle,
                            0.8F),
                        shotParam.Speed
                        ),
                    m_Trajectory,
                    false
                    );
            }

            AudioManager.Instance.Play(BattleHackingEnemyManager.Instance.ParamSet.MediumShot02Se);
        }
    }
}




//[SerializeField, Tooltip("（合計の）発射回数")]
//private float m_ShotSum;


//// この行って、等速直線運動前提だよね？でもrealPosition使ってないから問題ないか。（いらない）
//Vector3 realPosition = shotParam.Position + shotParam.Speed * dTime * new Vector3(Mathf.Cos(shotParam.Angle), 0, Mathf.Sin(shotParam.Angle));

//Vector3 eulerAngles = Calc.CalcEulerAngles(boss.GetEnemy().transform.eulerAngles, shotParam.Angle);


//[SerializeField, Tooltip("発射間隔")]
//private float m_ShotInterval2;

////[SerializeField, Tooltip("現在の形態の開始からの時刻")]
//private float m_Time2;


////[SerializeField, Tooltip("実際に発射された回数")]
//private int m_RealShotNum2;

//[SerializeField, Tooltip("発射操作の配列")]
//private ShotController[] m_ShotControllerArray2;

//[SerializeField, Tooltip("発射操作の配列")]
//private ShotsController[] m_ShotsControllerArray2;

//[SerializeField, Tooltip("")]
//private ShotParam m_ShotParam2;


//public void OnUpdatesPhases(BattleHackingBossBehavior boss)
//{

//}


////時間を進める
//m_Time += Time.deltaTime;

//// まだ何もしない時間なら
//if (!m_IsMainPhase)
//{
//    // この最初の形態を抜けていたら
//    if (m_InitialTime <= m_Time)
//    {
//        m_IsMainPhase = true;

//        m_Time -= m_InitialTime;
//    }
//    // 今もまだ最初の形態なら
//    else
//    {
//        return;
//    }
//}

//// 現在のあるべき発射回数
//int properShotNum = Mathf.FloorToInt(m_Time / m_ShotInterval);

//// 発射されるべき回数分、弾を発射する
//while (m_RealShotNum < properShotNum)
//{
//    // 発射する弾の番号にする
//    m_RealShotNum++;

//    // 発射時刻
//    float launchTime = m_RealShotNum * m_ShotInterval;

//    // 発射からの経過時間
//    float dTime = m_Time - launchTime;
//}


////[SerializeField, Tooltip("メインの形態であるかどうか")]
//private bool m_IsMainPhase;

//[SerializeField, Tooltip("最初のなにもしない時間の長さ")]
//private float m_InitialTime;

//[SerializeField, Tooltip("発射間隔")]
//private float m_ShotInterval;

////[SerializeField, Tooltip("現在の形態の開始からの時刻")]
//private float m_Time;


////[SerializeField, Tooltip("実際に発射された回数")]
//private int m_RealShotNum;


//List<(float realShotNum, float launchTime, float dTime)> launchDTimeList = shotTimer.OnUpdates();


//[SerializeField, Tooltip("軌道の決め方のオブジェクト")]
//private SimpleTrajectory m_Trajectory;

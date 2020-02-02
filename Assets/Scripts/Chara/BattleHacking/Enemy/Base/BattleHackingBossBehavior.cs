using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingBossBehavior : ControllableObject
{
    protected BattleHackingEnemyController Enemy { get; private set; }
    protected BattleHackingBossBehaviorUnitParamSet BehaviorParamSet { get; private set; }

    public BattleHackingBossBehavior(BattleHackingEnemyController enemy, BattleHackingBossBehaviorUnitParamSet paramSet)
    {
        Enemy = enemy;
        BehaviorParamSet = paramSet;
    }

    /// <summary>
    /// この行動パターンから他のパターンになった時に呼び出される
    /// （オーバーライドがなくなったのでここに説明を書いている）
    /// </summary>
    public virtual void OnEnd()
    {

    }

    /// <summary>
    /// ボスのワールド座標を設定する。
    /// </summary>
    public void SetPosition(Vector3 pos)
    {
        if (Enemy == null)
        {
            return;
        }

        pos.y = Enemy.transform.position.y;
        Enemy.transform.position = pos;
    }

    /// <summary>
    /// ボスのワールド回転を設定する。
    /// </summary>
    public void SetRotation(float angle)
    {
        if (Enemy == null)
        {
            return;
        }

        var angles = Enemy.transform.eulerAngles;
        angles.y = angle;
        Enemy.transform.eulerAngles = angles;
    }

    // 使われていないので、コメントアウトした。
    //public BattleHackingBulletController Shot()
    //{
    //    return BattleHackingBulletController.ShotBullet(Enemy);
    //}

    public BattleHackingFreeTrajectoryBulletController Shot(CommandBulletShotParam p,SimpleTrajectory trajectoryBase, Vector3 position,float dTime)
    {
        p.BulletOwner = Enemy;
        //return BattleHackingFreeTrajectoryBulletController.ShotBullet(p, trajectoryBase, dTime);
        return null;
    }

    // ボスや攻撃に関わらず共通に使いそうなので、以下に書いておく。

    public BattleHackingEnemyController GetEnemy()
    {
        return Enemy;
    }


    [SerializeField, Tooltip("")]
    public HackingBossPhase m_HackingBossPhase;


    protected InfC761Hacker1Phase1ParamSet m_ParamSet;


    // オーバーライド前提なのでひとまずnull
    public virtual BattleHackingBossBehaviorUnitParamSet GetParamSet()
    {
        return null;
    }

    /// <summary>
    /// この行動パターンに入った瞬間に呼び出される
    /// </summary>
    public override void OnStart()
    {
        var paramSet = GetParamSet();
        if (paramSet == null)
        {
            return;
        }

        m_HackingBossPhase = m_ParamSet.m_HackingBossPhase;

        m_HackingBossPhase.OnStarts();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // なんで敵が倒れる時、このフィールドがnullになってしまうんだろう。
        if (m_HackingBossPhase != null) {
            TransformSimple transform = m_HackingBossPhase.OnUpdates(this);
            GetEnemy().transform.localPosition = new Vector3(transform.m_Position.x, 0, transform.m_Position.y);
            GetEnemy().transform.localEulerAngles = new Vector3(0, transform.m_Angle * Mathf.Rad2Deg, 0);
            GetEnemy().transform.localScale = Vector3.one * transform.m_Scale;
        }
    }
}





//SCWay sCWay = new SCWay();
//sCWay.m_Way = 20;
//SCDsp sCDsp = new SCDsp();
//sCDsp.m_SpeedNum = 3;
//sCDsp.m_DSpeed = 0.3f;

//shotParamArray = sCWay.GetshotParam(launchTime, shotParamArray);
//shotParamArray = sCDsp.GetshotParam(launchTime, shotParamArray);


//public List<ShotParam> GetshotParamWay(float time, List<ShotParam> array)
//{
//    int arraySize = array.Count;
//    int way = 20;

//    for (int i = 0; i < arraySize; i++)
//    {
//        ShotParam shotParam = array[0];
//        array.RemoveAt(0);

//        for (int wayIndex = 0; wayIndex < way; wayIndex++)
//        {
//            float newAngle = shotParam.Angle + Calc.TWO_PI * wayIndex / way;

//            array.Add(new ShotParam(shotParam.Position, newAngle, shotParam.Speed));
//        }
//    }

//    return array;
//}


//public List<ShotParam> GetshotParamSpeed(float time, List<ShotParam> array)
//{
//    int speedNum = 3;
//    float dSpeed = 0.3f;
//    int arraySize = array.Count;

//    for (int i = 0; i < arraySize; i++)
//    {
//        ShotParam shotParam = array[0];
//        array.RemoveAt(0);

//        for (int speedIndex = -(speedNum - 1); speedIndex <= speedNum - 1; speedIndex += 2)
//        {
//            float newSpeed = shotParam.Speed + speedIndex * dSpeed / 2;

//            array.Add(new ShotParam(shotParam.Position, shotParam.Angle, newSpeed));
//        }
//    }

//    return array;
//}


//m_ShotControllerArray = new ShotController[] {
//    new SCWay(){m_Way = 20 },
//    new SCDsp(){m_SpeedNum = 3, m_DSpeed = 0.3f}
//};


//for (int i = 0;i < m_ShotControllerArray.Length;i++)
//{
//    switch (m_ShotControllerArray[i])
//    {
//        case SCSwr sCSwr:
//            m_ShotControllerArray[i] = sCSwr;
//            break;

//        default:
//            Debug.Log("");
//            break;
//    }
//}

//for (int i = 0; i < m_ShotsControllerArray.Length; i++)
//{
//    switch (m_ShotsControllerArray[i])
//    {
//        case SCWay sCWay:
//            m_ShotsControllerArray[i] = sCWay;
//            break;

//        case SCDsp sCDsp:
//            m_ShotsControllerArray[i] = sCDsp;
//            break;

//        default:
//            Debug.Log("");
//            break;
//    }
//}


//m_Time2 += Time.deltaTime;

//// 現在のあるべき発射回数
//int properShotNum = Mathf.FloorToInt(GetIdealShotNum(m_Time2));

//// 発射されるべき回数分、弾を発射する
//while (m_RealShotNum < properShotNum)
//{
//    // 発射する弾の番号にする
//    m_RealShotNum++;

//    // 発射時刻
//    float launchTime = CalcLaunchTime();

//    // 発射からの経過時間
//    float dTime = m_Time2 - launchTime;


//    ShotParam sP = new ShotParam(m_ShotParam);
//    for (int i = 0; i < m_ShotControllerArray.Length; i++)
//    {
//        sP = m_ShotControllerArray[i].GetshotParam(launchTime, sP);
//    }

//    List<ShotParam> shotParamArray = new List<ShotParam>() {sP};
//    for (int i = 0; i < m_ShotsControllerArray.Length; i++)
//    {
//        shotParamArray = m_ShotsControllerArray[i].GetshotsParam(launchTime, shotParamArray);
//    }

//    foreach (ShotParam shotParam in shotParamArray)
//    {
//        // この行って、等速直線運動前提だよね？でもrealPosition使ってないから問題ないか。（いらない）
//        Vector3 realPosition = shotParam.Position + shotParam.Speed * dTime * new Vector3(Mathf.Cos(shotParam.Angle), 0, Mathf.Sin(shotParam.Angle));

//        Vector3 eulerAngles = Calc.CalcEulerAngles(GetEnemy().transform.eulerAngles, shotParam.Angle);

//        // 弾を撃つ
//        CommandBulletShotParam bulletShotParam = new CommandBulletShotParam(GetEnemy(), 0, 0, 0, Vector3.zero, Vector3.zero, Vector3.zero);
//        BattleHackingBulletController.ShotBullet(
//            bulletShotParam,
//            new SimpleTrajectory(
//                new TransformSimple(shotParam.Position, shotParam.Angle, 0.8f),
//                shotParam.Angle,
//                shotParam.Speed),
//            dTime);
//    }

//    AudioManager.Instance.Play(BattleHackingEnemyManager.Instance.ParamSet.MediumShot02Se);
//}


//public float GetIdealShotNum(float time)
//{
//    return time * 20;
//}


//public float CalcLaunchTime()
//{
//    return m_RealShotNum / 20.0f;
//}


//public override void OnFixedUpdate()
//{
//    base.OnFixedUpdate();
//}


//// 敵本体の位置を更新する
//private void BezierPositionMovingPV()
//{
//    // 時刻の進行度(0～1の値)
//    float normalizedTime = m_MoveTime / m_Bezier3Points[m_NowPhase].m_Time;

//    if (m_NowPhase == 0)
//    {
//        Enemy.transform.position = BezierMovePV(
//            m_InitPos,
//            Vector3.zero,
//            m_Bezier3Points[0].m_Time,
//            m_Bezier3Points[0].m_AnchorPosition,
//            m_Bezier3Points[0].m_AnchorVelocity,
//            normalizedTime
//            );
//    }
//    else if (m_IsLooping)
//    {
//        Enemy.transform.position = BezierMovePV(
//            m_Bezier3Points[m_Bezier3Points.Length - 1].m_AnchorPosition,
//            m_Bezier3Points[m_Bezier3Points.Length - 1].m_AnchorVelocity,
//            m_Bezier3Points[m_NowPhase].m_Time,
//            m_Bezier3Points[m_NowPhase].m_AnchorPosition,
//            m_Bezier3Points[m_NowPhase].m_AnchorVelocity,
//            normalizedTime
//            );
//    }
//    else
//    {
//        Enemy.transform.position = BezierMovePV(
//            m_Bezier3Points[m_NowPhase - 1].m_AnchorPosition,
//            m_Bezier3Points[m_NowPhase - 1].m_AnchorVelocity,
//            m_Bezier3Points[m_NowPhase].m_Time,
//            m_Bezier3Points[m_NowPhase].m_AnchorPosition,
//            m_Bezier3Points[m_NowPhase].m_AnchorVelocity,
//            normalizedTime
//            );
//    }
//}


//// 4点と媒介変数から、ベジェ曲線上の1点を返す(tは0～1の値)
//private Vector3 BezierMoving(Vector3 vec0, Vector3 vec1, Vector3 vec2, Vector3 vec3, float t)
//{
//    return (1 - t) * (1 - t) * (1 - t) * vec0 + 3 * (1 - t) * (1 - t) * t * vec1 + 3 * (1 - t) * t * t * vec2 + t * t * t * vec3;
//}


//// 始点と終点の位置と速度ベクトルと所要時間から、ベジェ曲線上の1点を返す(tは0～1の値)
//private Vector3 BezierMovePV(Vector3 position1, Vector3 velocity1, float time, Vector3 position2, Vector3 velocity2, float t)
//{
//    return BezierMoving(
//        position1,
//        position1 + velocity1 * time,
//        position2 + velocity2 * time,
//        position2,
//        t
//        );
//}


//[SerializeField, Tooltip("この攻撃の開始からの経過時間")]
//private float m_Time;

//[SerializeField, Tooltip("アセット用単位弾幕パラメータの配列")]
//private AllUDFieldArray m_AllUDFieldArray;

//[SerializeField, Tooltip("弾幕の抽象クラスの配列")]
//private DanmakuCountAbstract2[] m_DanmakuCountAbstractArray;

//[SerializeField, Tooltip("初期位置")]
//private Vector3 m_InitPos;

//[SerializeField, Tooltip("この形態になってからの時間")]
//private float m_MoveTime;

//[SerializeField, Tooltip("現在は第何形態か")]
//private int m_NowPhase;

//[SerializeField, Tooltip("ベジェ曲線の配列")]
//private Bezier1Point[] m_Bezier3Points;

//[SerializeField, Tooltip("ループの開始が第何形態目からか")]
//private int m_LoopBeginPhase;

//[SerializeField, Tooltip("形態がループしている最中かどうか")]
//private bool m_IsLooping = false;


//[SerializeField, Tooltip("開始からの時刻")]
//private float m_Time2 = 0;

//[SerializeField, Tooltip("実際に発射された回数")]
//private int m_RealShotNum = 0;

//[SerializeField, Tooltip("発射操作の配列")]
//public ShotParamControllerBase[] m_ShotControllerArray;

//[SerializeField, Tooltip("発射操作の配列")]
//public ShotParamListControllerBase[] m_ShotsControllerArray;

//[SerializeField, Tooltip("")]
//public ShotParam m_ShotParam;


//m_Time = 0;

//m_AllUDFieldArray = paramSet.AllUDFieldArray;
//m_DanmakuCountAbstractArray = paramSet.DanmakuCountAbstractArray;

//m_InitPos = Enemy.transform.position;//Debug.Log(m_InitPos);
//m_MoveTime = 0;
//m_NowPhase = 0;
//m_Bezier3Points = paramSet.Bezier3Points;
//m_LoopBeginPhase = paramSet.LoopBeginPhase;

//UDParams[] uDParamsArray = m_AllUDFieldArray.GetAllUDParams();

//m_DanmakuCountAbstractArray = new DanmakuCountAbstract2[uDParamsArray.Length];

//for (int i = 0; i < uDParamsArray.Length; i++)
//{
//    m_DanmakuCountAbstractArray[i] = EUDS.EUDToUDObject(uDParamsArray[i].GetEUD());

//    m_DanmakuCountAbstractArray[i].Awakes(uDParamsArray[i]);
//}


//m_ShotParam = m_ParamSet.m_ShotParam;

//m_ShotControllerArray = m_ParamSet.m_ShotControllerArray;
//m_ShotsControllerArray = m_ParamSet.m_ShotsControllerArray;


//時間を進める
//m_Time += Time.deltaTime;
//m_MoveTime += Time.deltaTime;

//if (m_Bezier3Points != null)
//{
//    if (m_Bezier3Points[m_NowPhase].m_Time < m_MoveTime)
//    {

//        // 経過時間を正しくする
//        m_MoveTime -= m_Bezier3Points[m_NowPhase].m_Time;

//        // 形態を次のものにする
//        m_NowPhase++;

//        m_IsLooping = false;

//        // 形態が最後まで行っているか
//        if (m_NowPhase == m_Bezier3Points.Length)
//        {
//            m_IsLooping = true;
//            m_NowPhase = m_LoopBeginPhase;
//        }
//    }

//    // 敵本体の状態を更新する
//    BezierPositionMovingPV();
//}

//if (m_DanmakuCountAbstractArray != null)
//{
//    foreach (DanmakuCountAbstract2 danmakuCountAbstract in m_DanmakuCountAbstractArray)
//    {
//        //danmakuCountAbstract.Updates(this, m_Time);
//    }
//}

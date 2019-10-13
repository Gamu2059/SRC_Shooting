using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// あるスマッシャーのボスのクラス。
public class BossTest : BattleRealEnemyController
{
    // 時刻を入力すると位置を返すデリゲート
    public delegate Vector3 PositionDelegate(float t);


    //[SerializeField, Tooltip("この形態になってからの経過時間")]
    private float m_Time;

    //[SerializeField, Tooltip("現在は何形態目か")]
    private int m_NowPhase;

    //[SerializeField, Tooltip("初期位置")]
    private Vector3 m_InitialPosition;

    //[SerializeField, Tooltip("ベジェ曲線の配列")]
    private Bezier3Points[] m_Bezier3Points;

    //[SerializeField, Tooltip("ベジェ曲線の配列")]
    //private Bezier1Point[] m_Bezier1Points;

    [SerializeField, Tooltip("弾幕")]
    private Smasher1Boss1 m_Danmaku;

    // ループする攻撃用時刻
    private float m_AttackTime;


    protected override void OnAwake()
    {
        // 時刻をゼロにする
        m_Time = 0;

        // 現在は0形態目
        m_NowPhase = 0;

        // 初期位置を代入する
        m_InitialPosition = transform.localPosition;
        //m_InitialPosition = new Vector3(10,0,0);

        m_Danmaku.Awakes();
    }


    public override void OnUpdate()
    {
        // 経過時間を進める
        m_Time += Time.deltaTime;

        //if (m_Bezier3Points[m_NowPhase].m_Time < m_Time)
        //{

        //    // 経過時間を正しくする
        //    m_Time -= m_Bezier3Points[m_NowPhase].m_Time;

        //    // 形態を次のものにする
        //    m_NowPhase++;

        //    // 形態が最後まで行っているか
        //    if (m_NowPhase == m_Bezier3Points.Length)
        //    {
        //        m_InitialPosition = m_Bezier3Points[m_NowPhase - 1].m_EndPoint;
        //        //m_InitialPosition = new Vector3(10, 0, 0);

        //        m_NowPhase = 0;
        //    }
        //}

        //// 敵本体の状態を更新する
        //BezierPositionMoving();

        m_Danmaku.Updates(this);
    }


    // 敵本体の位置を更新する
    private void BezierPositionMoving()
    {
        // 時刻の進行度(0～1の値)
        float normalizedTime = m_Time / m_Bezier3Points[m_NowPhase].m_Time;

        if (m_NowPhase == 0)
        {
            transform.localPosition = BezierMoving(m_InitialPosition,
                            m_Bezier3Points[0].m_ControlPoint1, m_Bezier3Points[0].m_ControlPoint2, m_Bezier3Points[0].m_EndPoint, normalizedTime);
        }
        else
        {
            transform.localPosition = BezierMoving(m_Bezier3Points[m_NowPhase - 1].m_EndPoint,
                m_Bezier3Points[m_NowPhase].m_ControlPoint1, m_Bezier3Points[m_NowPhase].m_ControlPoint2, m_Bezier3Points[m_NowPhase].m_EndPoint, normalizedTime);
        }
    }


    // 4点と媒介変数から、ベジェ曲線上の1点を返す(tは0～1の値)
    private Vector3 BezierMoving(Vector3 vec0, Vector3 vec1, Vector3 vec2, Vector3 vec3, float t)
    {
        return (1 - t) * (1 - t) * (1 - t) * vec0 + 3 * (1 - t) * (1 - t) * t * vec1 + 3 * (1 - t) * t * t * vec2 + t * t * t * vec3;
    }


    // 始点と終点の位置と速度ベクトルと所要時間から、ベジェ曲線上の1点を返す(tは0～1の値)
    private Vector3 BezierVT(Vector3 posiotion1,Vector3 velocity1, Vector3 posiotion2, Vector3 velocity2,float t)
    {
        return BezierMoving(posiotion1,posiotion1 + velocity1 * t, posiotion2 - velocity2 * t,posiotion2,t);
    }


    //// 始点と終点の位置と速度ベクトルと所要時間から、ベジェ曲線上の1点を返す(tは0～1の値)
    //private Vector3 BezierArrayMove(float[] t,Vector3[] position,Vector3[] velocity)
    //{
    //    for (int i = 0;i < t.Length;i++)
    //    {

    //    }
    //}
}





//// ベジェ曲線の配列の数を決める
//m_Bezier3Points = new Bezier3Points[1];

//m_Bezier3Points[0] = new Bezier3Points();

//// ランダムな位置を決める
//Vector2 randomPositionVector2 = 10 * Random.insideUnitCircle;
//Vector3 randomPositionVector3 = new Vector3(randomPositionVector2.x,0, randomPositionVector2.y);

//// ベジェ曲線の点に代入する
//m_Bezier3Points[0].m_ControlPoint1 = randomPositionVector3;
//m_Bezier3Points[0].m_ControlPoint2 = randomPositionVector3;
//m_Bezier3Points[0].m_EndPoint = randomPositionVector3;

//// ベジェ曲線の時間を決める
//m_Bezier3Points[0].m_Time = 3;




//// ランダムな位置を決める
//Vector2 randomPositionVector2 = 10 * Random.insideUnitCircle;
//Vector3 randomPositionVector3 = new Vector3(randomPositionVector2.x, 0, randomPositionVector2.y);

//// ベジェ曲線の点に代入する
//m_Bezier3Points[0].m_ControlPoint1 = randomPositionVector3;
//m_Bezier3Points[0].m_ControlPoint2 = randomPositionVector3;
//m_Bezier3Points[0].m_EndPoint = randomPositionVector3;




//transform.localPosition = m_InitialPosition + BezierMoving(m_InitialPosition,
//                m_Bezier3Points[0].m_ControlPoint1, m_Bezier3Points[0].m_ControlPoint2, m_Bezier3Points[0].m_EndPoint, normalizedTime);



//transform.localPosition = m_InitialPosition + BezierMoving(m_Bezier3Points[m_NowPhase-1].m_EndPoint, 
//    m_Bezier3Points[m_NowPhase].m_ControlPoint1, m_Bezier3Points[m_NowPhase].m_ControlPoint2, m_Bezier3Points[m_NowPhase].m_EndPoint, normalizedTime);
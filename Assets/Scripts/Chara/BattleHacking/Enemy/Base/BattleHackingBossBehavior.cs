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

    public CommandBulletController Shot()
    {
        return CommandBulletController.ShotBullet(Enemy);
    }

    public CommandBulletController Shot(CommandBulletShotParam p)
    {
        p.BulletOwner = Enemy;
        return CommandBulletController.ShotBullet(p);
    }

    // ボスや攻撃に関わらず共通に使いそうなので、以下に書いておく。

    public BattleHackingEnemyController GetEnemy()
    {
        return Enemy;
    }


    [SerializeField, Tooltip("この攻撃の開始からの経過時間")]
    private float m_Time;

    [SerializeField, Tooltip("アセット用単位弾幕パラメータの配列")]
    private AllUDFieldArray m_AllUDFieldArray;

    [SerializeField, Tooltip("弾幕の抽象クラスの配列")]
    private DanmakuCountAbstract2[] m_DanmakuCountAbstractArray;

    [SerializeField, Tooltip("初期位置")]
    private Vector3 m_InitPos;

    [SerializeField, Tooltip("この形態になってからの時間")]
    private float m_MoveTime;

    [SerializeField, Tooltip("現在は第何形態か")]
    private int m_NowPhase;

    [SerializeField, Tooltip("ベジェ曲線の配列")]
    private Bezier1Point[] m_Bezier3Points;

    [SerializeField, Tooltip("ループの開始が第何形態目からか")]
    private int m_LoopBeginPhase;

    [SerializeField, Tooltip("形態がループしている最中かどうか")]
    private bool m_IsLooping = false;


    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
    }

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
        base.OnStart();

        var paramSet = GetParamSet();
        if (paramSet == null)
        {
            return;
        }

        m_Time = 0;

        m_AllUDFieldArray = paramSet.AllUDFieldArray;
        m_DanmakuCountAbstractArray = paramSet.DanmakuCountAbstractArray;

        m_InitPos = Enemy.transform.position;
        m_MoveTime = 0;
        m_NowPhase = 0;
        m_Bezier3Points = paramSet.Bezier3Points;
        m_LoopBeginPhase = paramSet.LoopBeginPhase;

        UDParams[] uDParamsArray = m_AllUDFieldArray.GetAllUDParams();

        m_DanmakuCountAbstractArray = new DanmakuCountAbstract2[uDParamsArray.Length];

        for (int i = 0; i < uDParamsArray.Length; i++)
        {
            m_DanmakuCountAbstractArray[i] = EUDS.EUDToUDObject(uDParamsArray[i].GetEUD());

            m_DanmakuCountAbstractArray[i].Awakes(uDParamsArray[i]);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();


        //時間を進める
        m_Time += Time.deltaTime;
        m_MoveTime += Time.deltaTime;

        if (m_Bezier3Points != null)
        {
            if (m_Bezier3Points[m_NowPhase].m_Time < m_MoveTime)
            {

                // 経過時間を正しくする
                m_MoveTime -= m_Bezier3Points[m_NowPhase].m_Time;

                // 形態を次のものにする
                m_NowPhase++;

                m_IsLooping = false;

                // 形態が最後まで行っているか
                if (m_NowPhase == m_Bezier3Points.Length)
                {
                    m_IsLooping = true;
                    m_NowPhase = m_LoopBeginPhase;
                }
            }

            // 敵本体の状態を更新する
            BezierPositionMovingPV();
        }

        if (m_DanmakuCountAbstractArray != null)
        {
            foreach (DanmakuCountAbstract2 danmakuCountAbstract in m_DanmakuCountAbstractArray)
            {
                danmakuCountAbstract.Updates(this, m_Time);
            }
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    // 敵本体の位置を更新する
    private void BezierPositionMovingPV()
    {
        // 時刻の進行度(0～1の値)
        float normalizedTime = m_MoveTime / m_Bezier3Points[m_NowPhase].m_Time;

        if (m_NowPhase == 0)
        {
            Enemy.transform.position = BezierMovePV(
                m_InitPos,
                Vector3.zero,
                m_Bezier3Points[0].m_Time,
                m_Bezier3Points[0].m_AnchorPosition,
                m_Bezier3Points[0].m_AnchorVelocity,
                normalizedTime
                );
        }
        else if (m_IsLooping)
        {
            Enemy.transform.position = BezierMovePV(
                m_Bezier3Points[m_Bezier3Points.Length - 1].m_AnchorPosition,
                m_Bezier3Points[m_Bezier3Points.Length - 1].m_AnchorVelocity,
                m_Bezier3Points[m_NowPhase].m_Time,
                m_Bezier3Points[m_NowPhase].m_AnchorPosition,
                m_Bezier3Points[m_NowPhase].m_AnchorVelocity,
                normalizedTime
                );
        }
        else
        {
            Enemy.transform.position = BezierMovePV(
                m_Bezier3Points[m_NowPhase - 1].m_AnchorPosition,
                m_Bezier3Points[m_NowPhase - 1].m_AnchorVelocity,
                m_Bezier3Points[m_NowPhase].m_Time,
                m_Bezier3Points[m_NowPhase].m_AnchorPosition,
                m_Bezier3Points[m_NowPhase].m_AnchorVelocity,
                normalizedTime
                );
        }
    }


    // 4点と媒介変数から、ベジェ曲線上の1点を返す(tは0～1の値)
    private Vector3 BezierMoving(Vector3 vec0, Vector3 vec1, Vector3 vec2, Vector3 vec3, float t)
    {
        return (1 - t) * (1 - t) * (1 - t) * vec0 + 3 * (1 - t) * (1 - t) * t * vec1 + 3 * (1 - t) * t * t * vec2 + t * t * t * vec3;
    }


    // 始点と終点の位置と速度ベクトルと所要時間から、ベジェ曲線上の1点を返す(tは0～1の値)
    private Vector3 BezierMovePV(Vector3 position1, Vector3 velocity1, float time, Vector3 position2, Vector3 velocity2, float t)
    {
        return BezierMoving(
            position1,
            position1 + velocity1 * time,
            position2 + velocity2 * time,
            position2,
            t
            );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 単純な軌道の弾幕の抽象クラス。
// 発射回数をひたすら数えて、発射するかどうかを判定する。
[System.Serializable]
public abstract class DanmakuCountAbstract2 : System.Object
{

    [SerializeField, Tooltip("何番目の弾か")]
    protected int[] m_BulletIndex;

    [SerializeField, Tooltip("何番目の弾パラメータか")]
    protected int[] m_BulletParamIndex;

    // 実際の今までの発射回数
    protected int m_RealShotNum = 0;


    public Dictionary<string, int> m_IntParams = new Dictionary<string, int>();
    public Dictionary<string, float> m_FloatParams = new Dictionary<string, float>();
    public Dictionary<string, Vector3> m_Vector3Params = new Dictionary<string, Vector3>();

    public int[] m_Int;
    public float[] m_Float;
    public Vector3[] m_Vector3;
    public bool[] m_Bool;


    /// <summary>
    /// 単位弾幕パラメータを代入する。
    /// </summary>
    public virtual void Awakes(UDParams uDParams)
    {
        m_Bool = uDParams.GetBoolParams();
        m_Int = uDParams.GetIntParams();
        m_Float = uDParams.GetFloatParams();
        m_Vector3 = uDParams.GetVector3Params();

        //Debug.Log("DanmakuCountAbstract2.Awakes");
    }


    // Update is called once per frame
    public void Updates(BattleHackingBossBehavior enemyController, float time)
    {

        time -= 1;


        // 本体の位置とオイラー角を更新する
        //UpdateSelf();

        // 現在のあるべき発射回数
        int properShotNum = Mathf.FloorToInt(CalcNowShotNum(time));

        // 発射されるべき回数分、弾を発射する
        while (m_RealShotNum < properShotNum)
        {
            // 発射する弾の番号にする
            m_RealShotNum++;

            // 発射時刻
            float launchTime = CalcLaunchTime();

            // 発射からの経過時間
            float dTime = time - launchTime;

            // 弾を撃つ
            ShotBullets(enemyController, launchTime, dTime);

            //Debug.Log("DanmakuCountAbstract2.Updates while");
        }

        //Debug.Log("DanmakuCountAbstract2.Updates");
    }


    public Vector3 RThetaToVector3(float radius, float rad)
    {
        return new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));
    }


    public float V3ToRelativeAngle(Vector3 v1, Vector3 v2)
    {
        Vector3 relativePosition = v2 - v1;
        return Mathf.Atan2(relativePosition.z, relativePosition.x);
    }


    public Vector3 RandomCircleInsideToV3(float radius)
    {
        Vector2 randomPos = Random.insideUnitCircle * radius;
        return new Vector3(randomPos.x, 0, randomPos.y);
    }


    public Vector3 RandomCircleInsideToV3AndZero(float radius)
    {
        if (radius != 0)
        {
            return RandomCircleInsideToV3(radius);
        }
        else
        {
            return Vector3.zero;
        }
    }


    public void ShotTouchokuBullet(BattleHackingBossBehavior enemyController, int bulletIndex, Vector3 position, float velocityRad, float speed, float dTime)
    {
        Vector3 realPosition = position + speed * dTime * new Vector3(Mathf.Cos(velocityRad), 0, Mathf.Sin(velocityRad));

        Vector3 eulerAngles = CalcEulerAngles(enemyController.GetEnemy().transform.eulerAngles, velocityRad);

        // 弾の大きさを変えている。
        CommandBulletShotParam bulletShotParam = new CommandBulletShotParam(enemyController.GetEnemy(), bulletIndex, Mathf.RoundToInt(speed * 5 - 1), 0, realPosition, eulerAngles, Vector3.one * 0.8f);
        // とりあえずパラメータは適当に
        BattleHackingBulletController.ShotBullet(bulletShotParam,new ShunLinearTrajectory(velocityRad, speed * 2, speed, 0.2f, 0.1f),
            position, dTime);
    }


    public void ShotTouchokuWayBullet(BattleHackingBossBehavior enemyController, int bulletIndex, Vector3 position, float velocityRad, float speed, float dTime, int way)
    {
        for (int i = 0; i < way; i++)
        {
            // wayによる角度差
            float wayRad = Mathf.PI * 2 * i / m_Int[(int)Omn.INT.way];

            ShotTouchokuBullet(enemyController, bulletIndex, position, velocityRad + wayRad, speed, dTime);
        }
    }


    public void ShotTouchokuWayRadiusBullet(BattleHackingBossBehavior enemyController, int bulletIndex, Vector3 position, float velocityRad, float speed, float dTime, int way, float radius)
    {
        for (int i = 0; i < way; i++)
        {
            // wayによる角度差
            float wayRad = Mathf.PI * 2 * i / m_Int[(int)Omn.INT.way];

            Vector3 wayPos = RThetaToVector3(m_Float[(int)Omn.FLOAT.bulletSourceRadius], velocityRad + wayRad);

            ShotTouchokuBullet(enemyController, bulletIndex, position + wayPos, velocityRad + wayRad, speed, dTime);
        }
    }


    // 本体の位置とオイラー角を更新する
    //public abstract void UpdateSelf();

    // 現在のあるべき発射回数を計算する(小数)
    public abstract float CalcNowShotNum(float time);

    // 発射時刻を計算する
    public abstract float CalcLaunchTime();

    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public abstract void ShotBullets(BattleHackingBossBehavior enemyController, float launchTime, float dTime);


    // 角度からオイラー角を計算する
    public Vector3 CalcEulerAngles(Vector3 eulerAngles, float rad)
    {
        Vector3 angle = eulerAngles;
        angle.y = -(rad * Mathf.Rad2Deg) + 90;
        //angle.y = - rad * Mathf.Rad2Deg;
        return angle;
    }


    // 2πで割った余りにする
    public float Modulo2PI(float rad)
    {
        rad %= Mathf.PI * 2;
        return rad;
    }


    // オイラー角から角度を計算する
    public float CalcRad(Vector3 eulerAngles)
    {
        Vector3 angle = eulerAngles;
        return (90 - angle.y) * Mathf.Deg2Rad;
    }
}




//これいらなさそう
//[SerializeField, Tooltip("その弾幕の開始からの経過時間")]
//protected float m_Time;


//m_Bool = new bool[uDParams.GetNumBoolParams()];
//uDParams.SetBoolParamsArray(m_Bool);

//m_Int = new int[uDParams.GetNumIntParams()];
//uDParams.SetIntParamsArray(m_Int);

//m_Float = new float[uDParams.GetNumFloatParams()];
//uDParams.SetFloatParamsArray(m_Float);

//m_Vector3 = new Vector3[uDParams.GetNumVector3Params()];
//uDParams.SetVector3ParamsArray(m_Vector3);


//public virtual void Awakes(UDField uDField)
//{

//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDOmn : DanmakuCountAbstract
{

    //private enum BOOL
    //{
    //    発射平均位置を指定するかどうか,
    //    発射中心位置を円内にブレさせるかどうか,
    //    発射角度を直角に曲げるかどうか,
    //    発射角度を指定するかどうか,
    //    発射角度が自機依存かどうか,
    //    発射角度が自機狙いかどうか,
    //}


    //private enum INT
    //{
    //    何番目の弾か,
    //    way数,
    //}


    //private enum FLOAT
    //{
    //    発射間隔,
    //    弾速,
    //    弾源円半径,
    //    発射位置のブレ範囲の円の半径,// 「発射位置を円内にブレさせるかどうか」がfalseのなら、0になる。
    //    発射角度,// 「発射角度を指定するかどうか」がfalseなら、ランダムな角度になる。
    //}


    //private enum VECTOR3
    //{
    //    発射平均位置,// 「発射平均位置を指定するかどうか」がfalseのなら、ゼロベクトルになる。
    //}


    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum(float time)
    {
        return time / m_Float[(int)Omn.FLOAT.発射間隔];
    }


    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return m_Float[(int)Omn.FLOAT.発射間隔] * m_RealShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(BattleRealEnemyController enemyController, float launchTime, float dTime)
    {

        Vector3 avePosition;

        if (m_Bool[(int)Omn.BOOL.発射平均位置を指定するかどうか])
        {
            avePosition = m_Vector3[(int)Omn.VECTOR3.発射平均位置];
        }
        else
        {
            avePosition = Vector3.zero;
        }


        Vector3 posRandomZure;

        if (m_Bool[(int)Omn.BOOL.発射中心位置を円内にブレさせるかどうか])
        {
            posRandomZure = RandomCircleInsideToV3(m_Float[(int)Omn.FLOAT.発射位置のブレ範囲の円の半径]);
        }
        else
        {
            posRandomZure = Vector3.zero;
        }


        float posVeloRad;

        if (m_Bool[(int)Omn.BOOL.発射角度を直角に曲げるかどうか])
        {
            posVeloRad = Mathf.PI / 2;
        }
        else
        {
            posVeloRad = 0;
        }


        float rad0;

        if (m_Bool[(int)Omn.BOOL.発射角度を指定するかどうか])
        {
            rad0 = Mathf.PI * 2 * m_Float[(int)Omn.FLOAT.発射角度];
        }
        else
        {
            if (m_Bool[(int)Omn.BOOL.発射角度が自機依存かどうか])
            {
                var player = BattleRealPlayerManager.Instance.Player;
                rad0 = V3ToRelativeRad(enemyController.transform.position + m_Vector3[(int)Omn.VECTOR3.発射平均位置] + posRandomZure,
                    player.transform.position);

                if (m_Bool[(int)Omn.BOOL.発射角度が自機狙いかどうか])
                {
                    
                }
                else
                {
                    rad0 += Mathf.PI * 2 / m_Int[(int)Omn.INT.way数] / 2;
                }
            }
            else
            {
                rad0 = Random.Range(0, Mathf.PI * 2);
            }
        }


        // これ以降で使うフィールドは、bool値によらず必ず使うものだけを使う。

        for (int i = 0; i < m_Int[(int)Omn.INT.way数]; i++)
        {
            // 1つの弾の角度
            float rad = rad0 + Mathf.PI * 2 * i / m_Int[(int)Omn.INT.way数];

            // 発射された弾の現在の位置
            Vector3 pos;
            pos = enemyController.transform.position;
            pos += avePosition;
            pos += posRandomZure;
            pos += RThetaToVector3(m_Float[(int)Omn.FLOAT.弾源円半径], rad);

            ShotTouchokuBullet(enemyController, m_Int[(int)Omn.INT.何番目の弾か], pos, rad + posVeloRad, m_Float[(int)Omn.FLOAT.弾速], dTime);
        }
    }
}



//[SerializeField, Tooltip("パラメータ")]
//public UDParameters m_Parameters;


//[SerializeField, Tooltip("発射間隔")]
//private float m_ShotInterval;

//[SerializeField, Tooltip("発射位置")]
//public Vector3 m_LaunchPosition;

//[SerializeField, Tooltip("way数")]
//private int m_Way;

//[SerializeField, Tooltip("弾源円半径")]
//private float m_CircleRadius;

//[SerializeField, Tooltip("弾速")]
//private float m_BulletSpeed = 10;


//foreach (E_INT eInt in System.Enum.GetValues(typeof(E_INT)))
//{
//    m_IntParamsArray[(int)eInt] = m_IntParamsArray[(int)eInt];
//}


//uDParams.SetIntParams(m_IntParams, "何番目の弾か", "何番目の弾パラメータか", "way数");
//uDParams.SetFloatParams(m_FloatParams, "発射間隔", "弾源円半径", "弾速");
//uDParams.SetVector3Params(m_Vector3Params, "発射位置");


//// パラメータを代入する
//public override void Awakes(UDParams uDParams)
//{
//    m_Int = new int[uDParams.GetNumIntParams()];
//    uDParams.SetIntParamsArray(m_Int);

//    m_Float = new float[uDParams.GetNumFloatParams()];
//    uDParams.SetFloatParamsArray(m_Float);

//    m_Vector3 = new Vector3[uDParams.GetNumVector3Params()];
//    uDParams.SetVector3ParamsArray(m_Vector3);
//}


//public void SetIntParams(UDParams uDParams, params string[] paramNames)
//{
//    uDParams.SetIntParams(m_IntParams, paramNames);
//}


//public void SetIntParameters(int[] intParameters)
//{

//}


//m_Way = uDParameters.m_IntParameters[0];
//m_CircleRadius = uDParameters.m_FloatParameters[1];
//m_BulletSpeed = uDParameters.m_FloatParameters[2];
//m_LaunchPosition = uDParameters.m_Vector3Parameters[0];

//m_ShotInterval = m_FloatParams["発射間隔"];

//m_ShotInterval = uDParameters.m_FloatParameters[0];


//float distance = m_Float[(int)FLOAT.弾速] * dTime;

//pos += new Vector3(distance * Mathf.Cos(rad + Mathf.PI / 2), 0, distance * Mathf.Sin(rad + Mathf.PI / 2));

// その弾の発射角度
//Vector3 eulerAngles;
//eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles, rad + Mathf.PI / 2);

// 弾を撃つ
//BulletShotParam bulletShotParam = new BulletShotParam(enemyController, m_Int[(int)INT.何番目の弾か], m_Int[(int)INT.何番目の弾パラメータか], 0, pos, eulerAngles, enemyController.transform.localScale);
//BulletController.ShotBullet(bulletShotParam);

//pos += new Vector3(distance * Mathf.Cos(rad - Mathf.PI / 2), 0, distance * Mathf.Sin(rad - Mathf.PI / 2));

// その弾の発射角度
//eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles, rad - Mathf.PI / 2);

// 弾を撃つ
//bulletShotParam = new BulletShotParam(enemyController, m_Int[(int)INT.何番目の弾か], m_Int[(int)INT.何番目の弾パラメータか], 0, pos, eulerAngles, enemyController.transform.localScale);
//BulletController.ShotBullet(bulletShotParam);

// 発射された弾の現在の位置
//pos = enemyController.transform.position;
//pos += m_Vector3[(int)VECTOR3.発射位置];
//pos += RThetaToVector3(m_Float[(int)FLOAT.弾源円半径],rad);


//// ランダムな角度
//float rad0 = Random.Range(0, Mathf.PI * 2);

//for (int i = 0; i < m_Int[(int)INT.way数]; i++)
//{
//    // 1つの弾の角度
//    float rad1 = rad0 + Mathf.PI * 2 / m_Int[(int)INT.way数] * i;
//    float rad2 = rad0 + Mathf.PI * 2 / m_Int[(int)INT.way数] * (i + m_Float[(int)FLOAT.左右の角度のズレ]);

//    // 発射された弾の現在の位置
//    Vector3 pos;
//    pos = enemyController.transform.position;
//    pos += m_Vector3[(int)VECTOR3.発射位置];

//    Vector3 pos1;
//    pos1 = pos + RThetaToVector3(m_Float[(int)FLOAT.弾源円半径], rad1);

//    Vector3 pos2;
//    pos2 = pos + RThetaToVector3(m_Float[(int)FLOAT.弾源円半径], rad2);

//    ShotTouchokuBullet(enemyController, m_Int[(int)INT.何番目の弾か], pos1, rad1 + Mathf.PI / 2, m_Float[(int)FLOAT.弾速], dTime);
//    ShotTouchokuBullet(enemyController, m_Int[(int)INT.何番目の弾か], pos2, rad2 - Mathf.PI / 2, m_Float[(int)FLOAT.弾速], dTime);
//}

//// ランダムな角度
//float rad0 = Random.Range(0, Mathf.PI * 2);

//for (int i = 0; i < m_Int[(int)INT.way数]; i++)
//{
//    // 1つの弾の角度
//    float rad = rad0 + Mathf.PI * 2 * i / m_Int[(int)INT.way数];

//    // 発射された弾の現在の位置
//    Vector3 pos;
//    pos = enemyController.transform.position;
//    pos += m_Vector3[(int)VECTOR3.発射位置];
//    pos += RThetaToVector3(m_Float[(int)FLOAT.弾源円半径], rad);

//ShotTouchokuBullet(enemyController, m_Int[(int)INT.何番目の弾か], pos, rad + Mathf.PI / 2, m_Float[(int)FLOAT.弾速], dTime);

//ShotTouchokuBullet(enemyController, m_Int[(int)INT.何番目の弾か], pos, rad - Mathf.PI / 2, m_Float[(int)FLOAT.弾速], dTime);

//}


//// ランダムな角度
//float rad0 = Random.Range(0, Mathf.PI * 2);


//if (m_Bool[(int)BOOL.交差かどうか])
//{
//    // ランダムな角度
//    float rad0 = Random.Range(0, Mathf.PI * 2);

//    Do(enemyController, launchTime, dTime, rad0, 0, Mathf.PI / 2);
//    Do(enemyController, launchTime, dTime, rad0, m_Float[(int)FLOAT.左右の角度のズレ], - Mathf.PI / 2);
//}
//else
//// 交差でない時
//{

//}


//左右の角度のズレ,// 0～1の値


//Do(enemyController, launchTime, dTime, rad0, posRandomZure, posVeloRad);


//// この中で使うフィールドは、bool値によらず必ず使うものだけを使う。
//public void Do(EnemyController enemyController, float launchTime, float dTime,float rad0,Vector3 posRandomZure,float posVeloRad)
//{

//    for (int i = 0; i < m_Int[(int)INT.way数]; i++)
//    {
//        // 1つの弾の角度
//        float rad = rad0 + Mathf.PI * 2 * i / m_Int[(int)INT.way数];

//        // 発射された弾の現在の位置
//        Vector3 pos;
//        pos = enemyController.transform.position;
//        pos += m_Vector3[(int)VECTOR3.発射位置];
//        pos += posRandomZure;
//        pos += RThetaToVector3(m_Float[(int)FLOAT.弾源円半径], rad);

//        ShotTouchokuBullet(enemyController, m_Int[(int)INT.何番目の弾か], pos, rad + posVeloRad, m_Float[(int)FLOAT.弾速], dTime);
//    }
//}


//Vector3 relativePosition = PlayerCharaManager.Instance.GetCurrentController().transform.position
//    - (enemyController.transform.position + m_Vector3[(int)VECTOR3.発射平均位置] + posRandomZure);
//relativePosition.Normalize();
//rad0 = Mathf.Atan2(relativePosition.z, relativePosition.x);


//Vector2 randomPos = Random.insideUnitCircle * m_Float[(int)FLOAT.発射位置のブレ範囲の円の半径];
//posRandomZure = new Vector3(randomPos.x, 0, randomPos.y);
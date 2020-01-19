using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// よく使う計算をまとめたクラス。
/// </summary>
public static class Calc : object
{

    /// <summary>
    /// π/2
    /// </summary>
    public static readonly float HALF_PI = Mathf.PI / 2;


    /// <summary>
    /// 2π
    /// </summary>
    public static readonly float TWO_PI = Mathf.PI * 2;


    /// <summary>
    /// 黄金角（ラジアン）
    /// </summary>
    public static readonly float GOLDEN_ANGLE = 2.399963229728658023F;


    /// <summary>
    /// 黄金の割合
    /// </summary>
    public static readonly float GOLDEN_RATIO = 0.3819660112501059F;


    /// <summary>
    /// 角度からオイラー角を計算する。
    /// </summary>
    public static Vector3 CalcEulerAngles(Vector3 eulerAngles, float rad)
    {
        Vector3 angle = eulerAngles;
        angle.y = -(rad * Mathf.Rad2Deg) + 90;
        //angle.y = - rad * Mathf.Rad2Deg;
        return angle;
    }


    /// <summary>
    /// 角度にプラスして、一周余り処理をする。
    /// </summary>
    public static float PlusRad(float rad,float plusRad)
    {
        rad += plusRad;
        rad %= Mathf.PI * 2;
        return rad;
    }


    /// <summary>
    /// 半径と偏角から、XZ平面上の3次元ベクトルを取得する。
    /// </summary>
    public static Vector3 RThetaToVec3(float radius, float rad)
    {
        return new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));
    }


    /// <summary>
    /// 半径と偏角から、2次元ベクトルを取得する。
    /// </summary>
    public static Vector2 RThetaToVec2(float radius, float rad)
    {
        return new Vector2(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad));
    }


    /// <summary>
    /// XZ平面上の3次元ベクトルの差から、v1からv2への相対角度を取得する。
    /// </summary>
    public static float Vec3ToRelativeRad(Vector3 v1, Vector3 v2)
    {
        Vector3 relativePosition = v2 - v1;
        return Mathf.Atan2(relativePosition.z, relativePosition.x);
    }


    /// <summary>
    /// 半径から、XZ平面上の円内のランダムな3次元ベクトルを取得する。半径がゼロなら、ランダム処理をしない。
    /// </summary>
    public static Vector3 RandomCircleInsideToV3AndZero(float radius)
    {
        if (radius != 0)
        {
            Vector2 randomVec2 = Random.insideUnitCircle * radius;
            return new Vector3(randomVec2.x, 0, randomVec2.y);
        }
        else
        {
            return Vector3.zero;
        }
    }


    ///// <summary>
    ///// 半径から、円内のランダムな2次元ベクトルを取得する。半径がゼロなら、ランダム処理をしない。
    ///// </summary>
    //public static Vector2 RandomCircleInsideToV2(float radius)
    //{
    //    if (radius != 0)
    //    {
    //        Vector2 randomVec2 = Random.insideUnitCircle * radius;
    //        return new Vector2(randomVec2.x, 0, randomVec2.y);
    //    }
    //    else
    //    {
    //        return Vector2.zero;
    //    }
    //}


    /// <summary>
    /// プレイヤーの位置を取得する。
    /// </summary>
    public static Vector3 GetPlayerPosition()
    {
        return BattleHackingPlayerManager.Instance.Player.transform.position;
    }


    /// <summary>
    /// 与えられた位置から画面端までの最長距離を取得する。（未使用）（弾の大きさを考えていない）
    /// </summary>
    public static float GetLongestDistance(Vector3 position)
    {
        float minPositionX = -1;
        float maxPositionX = 1;
        float minPositionZ = -1;
        float maxPositionZ = 1;

        if (position.z >= 0)
        {
            // 第一象限なら
            if (position.x >= 0)
            {
                return Vector3.Distance(position, new Vector3(minPositionX,minPositionZ));
            }
            // 第二象限なら
            else
            {
                return Vector3.Distance(position, new Vector3(maxPositionX, minPositionZ));
            }
        }
        else
        {
            // 第四象限なら
            if (position.x >= 0)
            {
                return Vector3.Distance(position, new Vector3(minPositionX, maxPositionZ));
            }
            // 第三象限なら
            else
            {
                return Vector3.Distance(position, new Vector3(maxPositionX, maxPositionZ));
            }
        }
    }


    /// <summary>
    /// Vector2をVector3に、xyがxzになるように変換する。
    /// </summary>
    /// <param name="vector2">変換元のVector2</param>
    /// <returns>変換されたVector3</returns>
    public static Vector3 V2ToV3XZ(Vector2 vector2)
    {
        return new Vector3(vector2.x, 0, vector2.y);
    }
}




///// <summary>
///// 2πで割った余りにする。
///// </summary>
//public static float Modulo2PI(float rad)
//{
//    rad %= Mathf.PI * 2;
//    return rad;
//}


// なんだこれ(いらない)

///// <summary>
///// オイラー角から角度を計算する。
///// </summary>
//public static float CalcRad(Vector3 eulerAngles)
//{
//    Vector3 angle = eulerAngles;
//    return (90 - angle.y) * Mathf.Deg2Rad;
//}


///// <summary>
///// 半径から、XZ平面上の円内のランダムな3次元ベクトルを取得する。
///// </summary>
//public static Vector3 RandomCircleInsideToV3(float radius)
//{
//    Vector2 randomVec2 = Random.insideUnitCircle * radius;
//    return new Vector3(randomVec2.x, 0, randomVec2.y);
//}


///// <summary>
///// if文。float型の値を返す。
///// </summary>
//public static float floatIf(bool b, float t, float f)
//{
//    if (b) return t; else return f;
//}
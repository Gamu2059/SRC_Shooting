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
    /// 2πで割った余りにする。
    /// </summary>
    public static float Modulo2PI(float rad)
    {
        rad %= Mathf.PI * 2;
        return rad;
    }


    /// <summary>
    /// オイラー角から角度を計算する。
    /// </summary>
    public static float CalcRad(Vector3 eulerAngles)
    {
        Vector3 angle = eulerAngles;
        return (90 - angle.y) * Mathf.Deg2Rad;
    }


    /// <summary>
    /// 半径と偏角から、XZ平面上の3次元ベクトルを取得する。
    /// </summary>
    public static Vector3 RThetaToVector3(float radius, float rad)
    {
        return new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));
    }


    /// <summary>
    /// XZ平面上の3次元ベクトルの差から、相対角度を取得する。
    /// </summary>
    public static float V3ToRelativeRad(Vector3 v1, Vector3 v2)
    {
        Vector3 relativePosition = v2 - v1;
        return Mathf.Atan2(relativePosition.z, relativePosition.x);
    }


    /// <summary>
    /// 半径から、XZ平面上の円内のランダムな3次元ベクトルを取得する。
    /// </summary>
    public static Vector3 RandomCircleInsideToV3(float radius)
    {
        Vector2 randomPos = Random.insideUnitCircle * radius;
        return new Vector3(randomPos.x, 0, randomPos.y);
    }


    /// <summary>
    /// 半径から、XZ平面上の円内のランダムな3次元ベクトルを取得する。半径がゼロなら、ランダム処理をしない。
    /// </summary>
    public static Vector3 RandomCircleInsideToV3AndZero(float radius)
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

    /// <summary>
    /// if文。float型の値を返す。
    /// </summary>
    public static float floatIf(bool b,float t,float f)
    {
        if (b) return t; else return f;
    }
}

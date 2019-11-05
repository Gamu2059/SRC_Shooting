using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// よく使う計算をまとめたクラス。
/// </summary>
public static class Calc : object
{
    // 角度からオイラー角を計算する
    public static Vector3 CalcEulerAngles(Vector3 eulerAngles, float rad)
    {
        Vector3 angle = eulerAngles;
        angle.y = -(rad * Mathf.Rad2Deg) + 90;
        //angle.y = - rad * Mathf.Rad2Deg;
        return angle;
    }


    // 2πで割った余りにする
    public static float Modulo2PI(float rad)
    {
        rad %= Mathf.PI * 2;
        return rad;
    }


    // オイラー角から角度を計算する
    public static float CalcRad(Vector3 eulerAngles)
    {
        Vector3 angle = eulerAngles;
        return (90 - angle.y) * Mathf.Deg2Rad;
    }


    public static Vector3 RThetaToVector3(float radius, float rad)
    {
        return new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));
    }


    public static float V3ToRelativeRad(Vector3 v1, Vector3 v2)
    {
        Vector3 relativePosition = v2 - v1;
        return Mathf.Atan2(relativePosition.z, relativePosition.x);
    }


    public static Vector3 RandomCircleInsideToV3(float radius)
    {
        Vector2 randomPos = Random.insideUnitCircle * radius;
        return new Vector3(randomPos.x, 0, randomPos.y);
    }


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
}

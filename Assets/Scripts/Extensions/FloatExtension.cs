using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の拡張メソッド
/// </summary>
public static class FloatExtension
{
    /// <summary>
    /// value * Mathf.Deg2Rad を返す。
    /// </summary>
    public static float DegToRad(this float value)
    {
        return value * Mathf.Deg2Rad;
    }

    /// <summary>
    /// value * Mathf.Rad2Deg を返す。
    /// </summary>
    public static float RadToDeg(this float value)
    {
        return value * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 数学のデカルト座標系の角度から、Unityのオブジェクトの座標系の角度へと変換する。<br/>
    /// 数学では0度が右、90度が上、180度が左、270度が下である。<br/>
    /// これを、0度が上、90度が右、180度が下、270度が左に変換する。
    /// </summary>
    public static float MathAngleToUnityObjectAngle(this float degreeAngle)
    {
        return -(degreeAngle - 90);
    }

    /// <summary>
    /// Unityのオブジェクトの座標系の角度から、数学のデカルト座標系の角度へと変換する。<br/>
    /// Unityのオブジェクトでは0度が上、90度が左、180度が下、270度が左である。<br/>
    /// これを、0度が右、90度が上、180度が左、270度が下に変換する。
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float UnityObjectAngleToMathAngle(this float degreeAngle)
    {
        return -degreeAngle + 90;
    }
}

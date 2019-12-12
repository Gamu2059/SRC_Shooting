using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationCurveExtension
{
    /// <summary>
    /// AnimationCurveの最初から最後までの時間の長さを取得する。
    /// </summary>
    public static float Duration(this UnityEngine.AnimationCurve animation)
    {
        if (animation == null)
        {
            return 0;
        }

        var keys = animation.keys;
        if (keys == null || keys.Length < 1)
        {
            return 0;
        }

        var max = keys[keys.Length - 1].time;
        var min = keys[0].time;

        return max - min;
    }
}

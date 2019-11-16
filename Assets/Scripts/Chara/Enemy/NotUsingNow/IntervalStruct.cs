using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct IntervalStruct
{
    // 時間間隔
    private float interval;

    // 次までの時間
    private float time;


    // 代入する
    public void set(float interval, float time)
    {
        this.interval = interval;
        this.time = time;
    }

    // 進める
    public void proceed()
    {
        time -= Time.deltaTime;
    }

    // 次までの時間がマイナスかどうか
    public bool timeIsMinus()
    {
        return time < 0;
    }

    public float getMinusTime()
    {
        return - time;
    }

    // 時間をインターバル分補充する
    public void reload()
    {
        time += interval;
    }

}

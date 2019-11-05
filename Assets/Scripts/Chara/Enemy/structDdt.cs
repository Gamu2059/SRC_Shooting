using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct structDdt
{
    // そのもの
    public float it;

    // 時間微分値
    public float ddt;


    // 代入する
    public void set(float it,float ddt)
    {
        this.it = it;
        this.ddt = ddt;
    }

    // 進める
    public void plus()
    {
        it += ddt * Time.deltaTime;
    }

    // そのものを2πで割った余りにする
    public void modulo2PI()
    {
        it %= Mathf.PI * 2;
    }

    // そのものを与えられたオイラー角に反映する
    public void reflectEulerAngles(Vector3 eulerAngles)
    {
        Vector3 tempAngles = eulerAngles;
        tempAngles.y = -(it * Mathf.Rad2Deg) + 90;
        eulerAngles = tempAngles;
    }

    // 与えられた時間だけ戻ったそのものを返す
    public float getGoBack(float dTime)
    {
        return it - ddt * dTime;
    }
}

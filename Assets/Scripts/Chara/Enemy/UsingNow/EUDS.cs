using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単位弾幕を追加する際に変えるメソッドを集めた静的なクラス。
/// </summary>
public static class EUDS : object
{
    /// <summary>
    /// 単位弾幕の種類の列挙型から、単位弾幕のオブジェクトそのものを取得する。
    /// </summary>
    public static DanmakuCountAbstract EUDToUDObject(E_U_D eUD)
    {
        switch (eUD)
        {
            case E_U_D.OMN:
                return new UDOmn();

            case E_U_D.WAY:
                return new UDWay();

            case E_U_D.SWR:
                return new UDSwr();

            case E_U_D.WAP:
                return new UDWap();

            case E_U_D.ALS:
                return new UDAls();

            // 追加しやすいようにしている（この部分は変えない）
            default:
                return new UDOmn();
        }
    }


    /// <summary>
    /// 単位弾幕の種類の列挙型から、単位弾幕の意味のオブジェクトそのものを取得する。
    /// </summary>
    public static ExpAbstract EUDToExpObject(E_U_D eUD)
    {
        switch (eUD)
        {
            case E_U_D.OMN:
                return new Omn();

            case E_U_D.WAY:
                return new Way();

            case E_U_D.WAP:
                return new Wap();

            case E_U_D.ALS:
                return new Als();

            // 追加しやすいようにしている（この部分は変えない）
            default:
                return new Omn();
        }
    }
}

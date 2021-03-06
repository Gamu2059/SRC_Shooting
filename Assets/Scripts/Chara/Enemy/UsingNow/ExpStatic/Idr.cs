﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単位弾幕IDRの意味（名前と説明）を管理する。
/// </summary>
public class Idr : ExpAbstract
{

    /// <summary>
    /// 単位弾幕全体の説明を取得する。
    /// </summary>
    public override string GetExp()
    {
        return "イドラディアボルスのような弾幕。";
    }


    /// <summary>
    /// bool型のパラメータ名。
    /// </summary>
    public enum BOOL
    {
        特になし,
    }


    /// <summary>
    /// bool型のパラメータの説明。
    /// </summary>
    public override string[] GetBoolExps2()
    {
        return new string[] {
            "特になし。"
        };
    }


    public enum INT
    {
        bulletIndex,
        way,
    }


    public override string[] GetIntExps2()
    {
        return new string[] {
            "弾の外見のインデックス。（今は1種類しかないので0にする）"
            ,
            "一度に発射する弾の数。"
        };
    }


    public enum FLOAT
    {
        shotInterval,

        /// <summary>
        /// 弾速。
        /// </summary>
        bulletSpeed,

        bulletSourceRadius,
        sourceAngle,
        sourceAngleSpeed,
        angle,
        angleSpeed,
    }


    public override string[] GetFloatExps2()
    {
        return new string[] {
            "前に発射してから次に発射するまでの時間。単位は秒。"
            ,
            "弾の速さ。"
            ,
            "弾源の円の半径"
            ,
            "弾源の角度の初期値"
            ,
            "弾源の角速度"
            ,
            "発射角度の初期値"
            ,
            "発射角度の角速度"
            ,
            "弾の大きさ"
        };
    }


    public enum VECTOR3
    {
        shotAvePosition,
    }


    public override string[] GetVector3Exps2()
    {
        return new string[] {
            "弾源の平均の位置。"
        };
    }


    /// <summary>
    /// bool型のパラメータ名の列挙型を取得する。
    /// </summary>
    public override System.Type GetEnumBool() { return typeof(BOOL); }
    public override System.Type GetEnumInt() { return typeof(INT); }
    public override System.Type GetEnumFloat() { return typeof(FLOAT); }
    public override System.Type GetEnumVector3() { return typeof(VECTOR3); }
}
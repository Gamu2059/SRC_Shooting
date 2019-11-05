using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単位弾幕POLの意味（名前と説明）を管理する。（弾速を自由に変えられないため、保留）
/// </summary>
public class Pol : ExpAbstract
{

    /// <summary>
    /// 単位弾幕全体の説明を取得する。
    /// </summary>
    public override string GetExp()
    {
        return "全体が正多角形の形をしている全方位弾。";
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
        numVertex,
    }


    public override string[] GetIntExps2()
    {
        return new string[] {
            "弾の外見のインデックス。（今は1種類しかないので0にする）"
            ,
            "一度に発射する弾の数。"
            ,
            "全方位弾全体の形が正何角形か"
        };
    }


    public enum FLOAT
    {
        shotInterval,
        bulletSpeed,
        angleSpeed,
        bulletSourceRadius,
        shotBlurRadius,
        angle,
    }


    public override string[] GetFloatExps2()
    {
        return new string[] {
            "前に発射してから次に発射するまでの時間。単位は秒。"
            ,
            "弾の速さ。"
            ,
            "弾源の回る角速度"
            ,
            "「発射位置」で指定した位置から、これだけ離れた位置から発射する。"
            ,
            "発射位置を、この半径の円の中にランダムでブレさせる。"
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
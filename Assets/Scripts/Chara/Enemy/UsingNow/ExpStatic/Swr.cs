using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単位弾幕SWRの意味（名前と説明）を管理する。
/// </summary>
public class Swr : ExpAbstract
{

    /// <summary>
    /// 単位弾幕全体の説明を取得する。
    /// </summary>
    public override string GetExp()
    {
        return "渦巻状に弾を発射する。";
    }


    /// <summary>
    /// bool型のパラメータ名。
    /// </summary>
    public enum BOOL
    {
        発射平均位置を指定するかどうか,
        発射中心位置を円内にブレさせるかどうか,
    }


    /// <summary>
    /// bool型のパラメータの説明。
    /// </summary>
    public override string[] GetBoolExps2()
    {
        return new string[] {
            "falseなら、「発射平均位置」をゼロベクトルに固定する。"
            ,
            "falseなら「発射位置のずれの半径」を0に固定する。"
        };
    }


    public enum INT
    {
        何番目の弾か,
        way数,
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
        発射間隔,
        弾速,
        角速度,
        弾源円半径,
        発射位置のブレ範囲の円の半径,
    }


    public override string[] GetFloatExps2()
    {
        return new string[] {
            "前に発射してから次に発射するまでの時間。単位は秒。"
            ,
            "弾の速さ。（今は2以上20以下の偶数にする）"
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
        発射平均位置,
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
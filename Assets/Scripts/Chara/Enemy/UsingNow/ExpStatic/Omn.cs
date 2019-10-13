using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単位弾幕OMNの意味（名前と説明）を管理する。
/// </summary>
public class Omn : ExpAbstract
{

    /// <summary>
    /// 単位弾幕全体の説明を取得する。
    /// </summary>
    public override string GetExp()
    {
        return "全方位に弾を発射する。隣り合う角度は等しく、対称的である。";
    }


    /// <summary>
    /// bool型のパラメータ名。
    /// </summary>
    public enum BOOL
    {
        発射平均位置を指定するかどうか,
        発射中心位置を円内にブレさせるかどうか,
        発射角度を直角に曲げるかどうか,
        発射角度を指定するかどうか,
        発射角度が自機依存かどうか,
        発射角度が自機狙いかどうか,
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
            ,
            "trueなら、発射角度を直角に曲げる。"
            ,
            "trueなら、発射角度を「発射角度」に固定する。"
            ,
            "「発射角度を指定するかどうか」がfalseなら有効。" + "\n" +
            "falseなら、発射角度をランダムにする。"
            ,
            "「発射角度が自機依存かどうか」が有効であり、その値がtrueなら有効。" + "\n" +
            "trueなら発射角度を自機狙いに、falseなら自機外しにする。"
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
        弾源円半径,
        発射位置のブレ範囲の円の半径,
        発射角度,
    }


    public override string[] GetFloatExps2()
    {
        return new string[] {
            "前に発射してから次に発射するまでの時間。単位は秒。"
            ,
            "弾の速さ。（今は2以上20以下の偶数にする）"
            ,
            "「発射位置」で指定した位置から、これだけ離れた位置から発射する。"
            ,
            "発射位置を、この半径の円の中にランダムでブレさせる。"
            ,
            "一周を1とした時の値。例えば直角なら4分の1。"
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



//public override string[] GetBoolExps()
//{
//    return new string[] {
//            "発射平均位置を指定するかどうか"
//            + "\n\n" +
//            "falseなら、「発射平均位置」をゼロベクトルに固定する。"
//            ,
//            "発射中心位置を円内にブレさせるかどうか"
//            + "\n\n" +
//            "falseなら「発射位置のずれの半径」を0に固定する。"
//            ,
//            "発射角度を直角に曲げるかどうか"
//            ,
//            "発射角度を指定するかどうか"
//            ,
//            "発射角度が自機依存かどうか"
//            ,
//            "発射角度が自機狙いかどうか"
//        };
//}


//public override string[] GetIntExps()
//{
//    return new string[] {
//            "弾のインデックス"
//            ,
//            "way数"
//        };
//}


//public override string[] GetFloatExps()
//{
//    return new string[] {
//            "発射間隔"
//            ,
//            "弾速"
//            ,
//            "弾源円半径"
//            ,
//            "発射位置のブレ範囲の円の半径"
//            ,
//            "発射角度"
//        };
//}


//public override string[] GetVector3Exps()
//{
//    return new string[] {
//            "発射平均位置"
//        };
//}


//public override string[] GetBoolExps12()
//{
//    string[] exp12 = new string[GetBoolExps2().Length];
//    string[] exp2 = GetBoolExps2();

//    for (int i = 0; i < exp12.Length; i++)
//    {
//        exp12[i] = ((BOOL)System.Enum.ToObject(typeof(BOOL),i)).ToString() + "\n\n" + exp2[i];
//    }

//    return exp12;
//}


//public override string[] GetIntExps12()
//{
//    string[] exp12 = new string[GetIntExps2().Length];
//    string[] exp2 = GetIntExps2();

//    for (int i = 0; i < exp12.Length; i++)
//    {
//        exp12[i] = ((INT)System.Enum.ToObject(typeof(INT), i)).ToString() + "\n\n" + exp2[i];
//    }

//    return exp12;
//}


//public override string[] GetFloatExps12()
//{
//    string[] exp12 = new string[GetFloatExps2().Length];
//    string[] exp2 = GetFloatExps2();

//    for (int i = 0; i < exp12.Length; i++)
//    {
//        exp12[i] = ((FLOAT)System.Enum.ToObject(typeof(FLOAT), i)).ToString() + "\n\n" + exp2[i];
//    }

//    return exp12;
//}


//public override string[] GetVector3Exps12()
//{
//    string[] exp12 = new string[GetVector3Exps2().Length];
//    string[] exp2 = GetVector3Exps2();

//    for (int i = 0; i < exp12.Length; i++)
//    {
//        exp12[i] = ((VECTOR3)System.Enum.ToObject(typeof(VECTOR3), i)).ToString() + "\n\n" + exp2[i];
//    }

//    return exp12;
//}
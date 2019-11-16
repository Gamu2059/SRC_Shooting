#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アセット用単位弾幕パラメータのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/UD Fields", fileName = "OMN", order = 0)]
[System.Serializable]
public class UDField : ScriptableObject
{

    /// <summary>
    /// 単位弾幕の種類。
    /// </summary>
    [SerializeField, Tooltip("単位弾幕の種類。")]
    private E_U_D m_EUD;

    /// <summary>
    /// 単位弾幕の種類の説明。
    /// </summary>
    [SerializeField, TextArea(0, 100), Tooltip("単位弾幕の種類の説明。")]
    private string m_EUDExp;

    /// <summary>
    /// パラメータ。
    /// </summary>
    [SerializeField, Tooltip("パラメータ。")]
    private ExpFields m_ExpFields;


    /// <summary>
    /// 純粋な単位弾幕パラメータを取得する。
    /// </summary>
    public UDParams GetUDParams()
    {
        return new UDParams(m_EUD, m_ExpFields.GetBoolArray(), m_ExpFields.GetintArray(), m_ExpFields.GetFloatArray(), m_ExpFields.GetVector3Array());
    }


    /// <summary>
    /// 初期化して、各単位弾幕の説明を表示する。
    /// </summary>
    [ContextMenu("Reset")]
    public void Reset()
    {
        foreach (E_U_D eUD in System.Enum.GetValues(typeof(E_U_D)))
        {
            m_EUDExp += eUD;
            m_EUDExp += "\n\n";
            m_EUDExp += EUDS.EUDToExpObject(eUD).GetExp();
            m_EUDExp += "\n\n\n";
        }
    }


    /// <summary>
    /// 入力した単位弾幕の種類に応じて、各パラメータの説明を表示する。
    /// </summary>
    [ContextMenu("EUDを反映させる")]
    public void ReflectEUD()
    {
        Debug.Log("ReflectEUD");

        m_EUDExp = m_EUD + "\n\n" + EUDS.EUDToExpObject(m_EUD).GetExp();

        ExpAbstract expAbstract = EUDS.EUDToExpObject(m_EUD);

        m_ExpFields = new ExpFields(expAbstract.GetBoolExps12(), expAbstract.GetIntExps12(), expAbstract.GetFloatExps12(), expAbstract.GetVector3Exps12());
    }
}



//expBools = new ExpBool[6];

//expBools[0] = new ExpBool("発射平均位置を指定するかどうか");
//expBools[1] = new ExpBool("発射中心位置を円内にブレさせるかどうか");
//expBools[2] = new ExpBool("発射角度を直角に曲げるかどうか");
//expBools[3] = new ExpBool("発射角度を指定するかどうか");
//expBools[4] = new ExpBool("発射角度が自機依存かどうか");
//expBools[5] = new ExpBool("発射角度が自機狙いかどうか");


//string[] boolExps = new string[6];
//boolExps[0] =
//    "発射平均位置を指定するかどうか"
//    + "\n\n" +
//    "falseなら、「発射平均位置」をゼロベクトルに固定する。";

//boolExps[1] =
//    "発射中心位置を円内にブレさせるかどうか"
//    + "\n\n" +
//    "falseなら「発射位置のずれの半径」を0に固定する。";

//boolExps[2] = "発射角度を直角に曲げるかどうか";
//boolExps[3] = "発射角度を指定するかどうか";
//boolExps[4] = "発射角度が自機依存かどうか";
//boolExps[5] = "発射角度が自機狙いかどうか";

//string[] intExps = new string[2];
//intExps[0] = "弾のインデックス";
//intExps[1] = "way数";

//string[] floatExps = new string[5];
//floatExps[0] = "発射間隔";
//floatExps[1] = "弾速";
//floatExps[2] = "弾源円半径";
//floatExps[3] = "発射位置のブレ範囲の円の半径";
//floatExps[4] = "発射角度";

//string[] Vector3Exps = new string[1];
//Vector3Exps[0] = "発射平均位置";


///// <summary>
///// 単位弾幕の種類の列挙型を、単位弾幕の意味の文字列に変換する
///// </summary>
//public string EUDToExpString(E_U_D eUD)
//{
//    switch (eUD)
//    {
//        case E_U_D.OMN:
//            return "omn";

//        case E_U_D.WAY:
//            return "way";

//        // 追加しやすいようにしている（この部分は変えない）
//        default:
//            return "";
//    }
//}


///// <summary>
///// 単位弾幕の種類の列挙型を、単位弾幕の意味のオブジェクトそのものに変換する
///// </summary>
//public ExpAbstract EUDToExpObject(E_U_D eUD)
//{
//    switch (eUD)
//    {
//        case E_U_D.OMN:
//            return new Omn();

//        case E_U_D.WAY:
//            return new Way();

//        case E_U_D.SWR:
//            return new Swr();

//        // 追加しやすいようにしている（この部分は変えない）
//        default:
//            return new Omn();
//    }
//}



//[Space()]
//[Header("Bool Params")]

//[SerializeField, Tooltip(
//    "発射平均位置を指定するかどうか"
//    + "\n\n" +
//    "falseなら、「発射平均位置」をゼロベクトルに固定する。"
//    )]
//public bool m_IsShotAvePosDefined;

//[SerializeField, Tooltip(
//    "発射中心位置を円内にブレさせるかどうか"
//    + "\n\n" +
//    "falseなら「発射位置のずれの半径」を0に固定する。"
//    )]
//public bool m_IsBulletSourceBlurRadiusDefined;

//[SerializeField, Tooltip("発射角度を直角に曲げるかどうか")]
//public bool m_IsTurnRightAngle;

//[SerializeField, Tooltip("発射角度を指定するかどうか")]
//public bool m_IsShotAngleDefined;

//[SerializeField, Tooltip("発射角度が自機依存かどうか")]
//public bool m_PlayerDependent;

//[SerializeField, Tooltip("発射角度が自機狙いかどうか")]
//public bool m_IsAimingAtPlayer;


//[Space()]
//[Header("Int Params")]

//[SerializeField, Tooltip("弾のインデックス")]
//public int m_BulletIndex;

//[SerializeField, Tooltip("way数")]
//public int m_Way;


//[Space()]
//[Header("Float Params")]

//[SerializeField, Tooltip("発射間隔")]
//public float m_ShotInterval;

//[SerializeField, Tooltip("弾速")]
//public float m_BulletSpeed;

//[SerializeField, Tooltip("弾源円半径")]
//public float m_BulletSourceRadius;

//[SerializeField, Tooltip("発射位置のブレ範囲の円の半径")]
//public float m_BulletSourceBlurRadius;

//[SerializeField, Tooltip("発射角度")]
//public float m_ShotAngle;


//[Space()]
//[Header("Vector3 Params")]

//[SerializeField, Tooltip("発射平均位置")]
//public Vector3 m_ShotAvePos;


//[SerializeField, Tooltip("あ")]
//public ExpBool[] expBools;

//[SerializeField, Tooltip("あ")]
//public ExpInt[] expInts;


//public UDParams GetFields()
//{
//    bool[] boolParams = new bool[]
//    {m_IsShotAvePosDefined,m_IsBulletSourceBlurRadiusDefined,m_IsTurnRightAngle,m_IsShotAngleDefined,m_PlayerDependent,m_IsAimingAtPlayer};

//    int[] intParams = new int[]
//    {m_BulletIndex,m_Way};

//    float[] floatParams = new float[]
//    {m_ShotInterval,m_BulletSpeed,m_BulletSourceRadius,m_BulletSourceBlurRadius,m_ShotAngle};

//    Vector3[] Vector3Params = new Vector3[]
//    {m_ShotAvePos};

//    UDParams uDParams = new UDParams(E_U_D.OMN, boolParams, intParams, floatParams, Vector3Params);

//    return uDParams;
//}
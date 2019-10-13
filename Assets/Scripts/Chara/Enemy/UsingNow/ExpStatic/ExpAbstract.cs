using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bool型のパラメータの名前を管理するクラスの抽象クラス。（abstractにする）
/// </summary>
public class ExpAbstract : object
{

    /// <summary>
    /// その単位弾幕全体の説明を取得する。
    /// </summary>
    public virtual string GetExp()
    {
        return "";
    }


    /// <summary>
    /// bool型のパラメータの説明。（ここから4つはabstractにする）
    /// </summary>
    public virtual string[] GetBoolExps2()
    {
        return new string[] { };
    }
    public virtual string[] GetIntExps2()
    {
        return new string[] { };
    }
    public virtual string[] GetFloatExps2()
    {
        return new string[] { };
    }
    public virtual string[] GetVector3Exps2()
    {
        return new string[] { };
    }


    /// <summary>
    /// bool型のパラメータの名前と説明。（ここから4つはvirtualを外す）
    /// </summary>
    public virtual string[] GetBoolExps12()
    {
        string[] exp12 = new string[GetBoolExps2().Length];
        string[] exp2 = GetBoolExps2();

        for (int i = 0; i < exp12.Length; i++)
        {
            exp12[i] = (System.Enum.ToObject(GetEnumBool(), i)).ToString() + "\n\n" + exp2[i];
        }

        return exp12;
    }


    public virtual string[] GetIntExps12()
    {
        string[] exp12 = new string[GetIntExps2().Length];
        string[] exp2 = GetIntExps2();

        for (int i = 0; i < exp12.Length; i++)
        {
            exp12[i] = (System.Enum.ToObject(GetEnumInt(), i)).ToString() + "\n\n" + exp2[i];
        }

        return exp12;
    }


    public virtual string[] GetFloatExps12()
    {
        string[] exp12 = new string[GetFloatExps2().Length];
        string[] exp2 = GetFloatExps2();

        for (int i = 0; i < exp12.Length; i++)
        {
            exp12[i] = (System.Enum.ToObject(GetEnumFloat(), i)).ToString() + "\n\n" + exp2[i];
        }

        return exp12;
    }


    public virtual string[] GetVector3Exps12()
    {
        string[] exp12 = new string[GetVector3Exps2().Length];
        string[] exp2 = GetVector3Exps2();

        for (int i = 0; i < exp12.Length; i++)
        {
            exp12[i] = (System.Enum.ToObject(GetEnumVector3(), i)).ToString() + "\n\n" + exp2[i];
        }

        return exp12;
    }


    /// <summary>
    /// bool型のパラメータ名。（ここから4つはabstractにする）
    /// </summary>
    public virtual System.Type GetEnumBool()
    {
        return typeof(E_U_D);
    }

    public virtual System.Type GetEnumInt()
    {
        return typeof(E_U_D);
    }

    public virtual System.Type GetEnumFloat()
    {
        return typeof(E_U_D);
    }

    public virtual System.Type GetEnumVector3()
    {
        return typeof(E_U_D);
    }
}



//public static string m_BoolExps;
//public static string m_IntExps;
//public static string m_FloatExps;
//public static string m_Vector3Exps;


//public virtual string[] GetBoolExps12()
//{
//    return new string[] { };
//}
//public virtual string[] GetIntExps12()
//{
//    return new string[] { };
//}
//public virtual string[] GetFloatExps12()
//{
//    return new string[] { };
//}
//public virtual string[] GetVector3Exps12()
//{
//    return new string[] { };
//}


//public enum BOOL
//{

//}

//public enum INT
//{

//}

//public enum FLOAT
//{

//}

//public enum VECTOR3
//{

//}
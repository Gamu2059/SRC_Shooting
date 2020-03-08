using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 値型のラッパークラス。
/// </summary>
[System.Serializable]
public class Boxing1<T> : object
{
    // そのものの値
    [SerializeField]
    public T m_Value;
    public T Value {
        set
        {
            m_Value = value;
        }
        get
        {
            return m_Value;
        }
    }


    public Boxing1(T value)
    {
        Value = value;
    }


    public Boxing1(Boxing1<T> boxing)
    {
        Value = boxing.Value;
    }
}




//public void SetValue(T value)
//{
//    Value = value;
//}


//public T GetValue()
//{
//    return Value;
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpInt : object
{
    [SerializeField, TextArea(0, 100), Tooltip("意味")]
    public string m_Exp;

    [SerializeField, Tooltip("値")]
    public int m_Int;


    public ExpInt(string exp)
    {
        m_Exp = exp;
    }


    public ExpInt(string exp,int value)
    {
        m_Exp = exp;
        m_Int = value;
    }
}
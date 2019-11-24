using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpBool : object
{
    [SerializeField,TextArea(0,100), Tooltip("意味")]
    public string m_Exp;

    [SerializeField, Tooltip("値")]
    public bool m_Bool;


    public ExpBool(string exp)
    {
        m_Exp = exp;
    }


    public ExpBool(string exp,bool value)
    {
        m_Exp = exp;
        m_Bool = value;
    }
}
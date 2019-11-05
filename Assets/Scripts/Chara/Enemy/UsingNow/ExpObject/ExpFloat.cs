using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpFloat : object
{
    [SerializeField, TextArea(0, 100), Tooltip("意味")]
    public string m_Exp;

    [SerializeField, Tooltip("値")]
    public float m_Float;


    public ExpFloat(string exp)
    {
        m_Exp = exp;
    }
}
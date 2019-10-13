using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpVector3 : object
{
    [SerializeField, TextArea(0, 100), Tooltip("意味")]
    public string m_Exp;

    [SerializeField, Tooltip("値")]
    public Vector3 m_Vector3;


    public ExpVector3(string exp)
    {
        m_Exp = exp;
    }
}
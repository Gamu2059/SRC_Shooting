using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の演算とVector2型の演算をまとめたクラス。
/// </summary>
[System.Serializable]
public class Vector2Float : object
{
    
    /// <summary>
    /// Vector2型の値
    /// </summary>
    [SerializeField]
    public OperationVector2Base m_Vector2;

    /// <summary>
    /// float型の値
    /// </summary>
    [SerializeField]
    public OperationFloatBase m_Float;
}

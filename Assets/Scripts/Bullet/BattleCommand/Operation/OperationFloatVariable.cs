using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の変数を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/variable", fileName = "OperationFloatVariable", order = 0)]
[System.Serializable]
public class OperationFloatVariable : OperationFloatBase
{

    /// <summary>
    /// 値
    /// </summary>
    //[SerializeField]
    //private float m_Value
    public float Value { get; set; }


    public override float GetResultFloat()
    {
        return Value;
    }


    ///// <summary>
    ///// 値を書き換える
    ///// </summary>
    //public void SetValueFloat(float value)
    //{
    //    Value = value;
    //}
}

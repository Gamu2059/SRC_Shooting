using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bool値のfalseを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/constant/bool/false", fileName = "ConstantBoolFalse", order = 0)]
[System.Serializable]
public class ConstantBoolFalse : OperationBoolBase
{

    public override bool GetResultBool()
    {
        return false;
    }
}
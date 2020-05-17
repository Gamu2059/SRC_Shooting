using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bool値のtrueを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/constant/bool/true", fileName = "ConstantBoolTrue", order = 0)]
[System.Serializable]
public class ConstantBoolTrue : OperationBoolBase
{

    public override bool GetResultBool()
    {
        return true;
    }
}
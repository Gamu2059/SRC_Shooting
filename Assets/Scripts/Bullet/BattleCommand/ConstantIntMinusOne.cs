using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int値の-1を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/constant/int/minusOne", fileName = "ConstantIntMinusOne", order = 0)]
[System.Serializable]
public class ConstantIntMinusOne : OperationIntBase
{

    public override int GetResultInt()
    {
        return -1;
    }
}
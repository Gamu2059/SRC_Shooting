using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int値の0を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/constant/int/zero", fileName = "ConstantIntZero", order = 0)]
[System.Serializable]
public class ConstantIntZero : OperationIntBase
{

    public override int GetResultInt()
    {
        return 0;
    }
}
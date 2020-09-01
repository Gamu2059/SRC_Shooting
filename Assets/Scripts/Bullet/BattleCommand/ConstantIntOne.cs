using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int値の1を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/constant/int/one", fileName = "ConstantIntOne", order = 0)]
[System.Serializable]
public class ConstantIntOne : OperationIntBase
{

    public override int GetResultInt()
    {
        return 1;
    }
}
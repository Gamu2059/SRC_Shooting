using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int値の2を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/constant/int/two", fileName = "ConstantIntTwo", order = 0)]
[System.Serializable]
public class ConstantIntTwo : OperationIntBase
{

    public override int GetResultInt()
    {
        return 2;
    }
}
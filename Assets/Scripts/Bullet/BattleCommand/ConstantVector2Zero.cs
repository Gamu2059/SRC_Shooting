using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2値のゼロベクトルを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/constant/Vector2/zero", fileName = "ConstantVector2Zero", order = 0)]
[System.Serializable]
public class ConstantVector2Zero : OperationVector2Base
{

    public override Vector2 GetResultVector2()
    {
        return Vector2.zero;
    }
}
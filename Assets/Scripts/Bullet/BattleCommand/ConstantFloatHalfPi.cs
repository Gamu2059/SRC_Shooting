using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float値のπ/2を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/constant/float/halfPi", fileName = "ConstantFloatHalfPi", order = 0)]
[System.Serializable]
public class ConstantFloatHalfPi : OperationFloatBase
{

    public override float GetResultFloat()
    {
        return Mathf.PI / 2;
    }
}
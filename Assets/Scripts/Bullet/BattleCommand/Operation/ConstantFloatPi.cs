using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float値のπを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/constant/float/pi", fileName = "ConstantFloatPi", order = 0)]
[System.Serializable]
public class ConstantFloatPi : OperationFloatBase
{

    public override float GetResultFloat()
    {
        return Mathf.PI;
    }
}
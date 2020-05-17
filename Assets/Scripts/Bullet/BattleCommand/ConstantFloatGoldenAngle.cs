using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float値の黄金角を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/constant/float/goldenAngle", fileName = "ConstantFloatGoldenAngle", order = 0)]
[System.Serializable]
public class ConstantFloatGoldenAngle : OperationFloatBase
{

    public override float GetResultFloat()
    {
        return 2.3999632297286580233396222686929F;
    }
}
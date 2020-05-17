using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float値の2πを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/constant/float/twoPi", fileName = "ConstantFloatTwoPi", order = 0)]
[System.Serializable]
public class ConstantFloatTwoPi : OperationFloatBase
{

    public override float GetResultFloat()
    {
        return Mathf.PI * 2;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 0から2πまでのランダムなfloat値を取得するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/random/float/rangeTwoPi", fileName = "RandomFloatRangeTwoPi", order = 0)]
[System.Serializable]
public class RandomFloatRangeTwoPi : OperationFloatBase
{

    public override float GetResultFloat()
    {
        return Random.Range(0, 2 * Mathf.PI);
    }
}
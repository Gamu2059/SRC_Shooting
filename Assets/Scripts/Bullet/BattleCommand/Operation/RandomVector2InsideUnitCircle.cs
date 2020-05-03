using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単位円内のランダムなVector2値を取得するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/random/Vector2/insideUnitCircle", fileName = "RandomVector2InsideUnitCircle", order = 0)]
[System.Serializable]
public class RandomVector2InsideUnitCircle : OperationVector2Base
{

    public override Vector2 GetResultVector2()
    {
        return Random.insideUnitCircle;
    }
}
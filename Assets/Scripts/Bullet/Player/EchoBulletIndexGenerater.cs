using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射されるEcho弾にインデックスを与えるクラス
/// </summary>
public class EchoBulletIndexGenerater : MonoBehaviour
{
    private const int INDEX_MAX_VALUE = 10000;

    private static int currentIndex = 0;

    public static int GenerateBulletIndex()
    {
        currentIndex++;

        if(currentIndex >= INDEX_MAX_VALUE)
        {
            currentIndex = 0;
            return currentIndex;
        }
        else
        {
            return currentIndex;
        }
    }
}

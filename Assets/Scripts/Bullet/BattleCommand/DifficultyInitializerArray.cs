#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度によって変わる値を初期化するオブジェクトの配列を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/difficultyInitializerArray", fileName = "DifficultyInitializerArray", order = 0)]
[System.Serializable]
public class DifficultyInitializerArray : DifficultyInitializerBase
{

    [SerializeField, Tooltip("オブジェクトの配列")]
    private DifficultyInitializerBase[] m_Array;


    /// <summary>
    /// 初期化する
    /// </summary>
    public override void Setup()
    {
        foreach (DifficultyInitializerBase difficultyInitializer in m_Array)
        {
            difficultyInitializer.Setup();
        }
    }
}

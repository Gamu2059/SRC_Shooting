#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度によって変わる値を、現在の難易度によって初期化するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/difficultyInitializer", fileName = "DifficultyInitializer", order = 0)]
[System.Serializable]
public class DifficultyInitializer : DifficultyInitializerBase
{

    [SerializeField, Tooltip("難易度によって変わるfloat型の演算オブジェクトの配列")]
    private E_DIFFICULTY m_Difficulty;

    [SerializeField, Tooltip("難易度によって変わるfloat型の演算オブジェクトの配列")]
    private OperationFloatDifficultyBase[] m_Float;

    [SerializeField, Tooltip("難易度によって変わるint型の演算オブジェクトの配列")]
    private OperationIntDifficultyBase[] m_Int;

    [SerializeField, Tooltip("難易度によって変わるVector2型の演算オブジェクトの配列")]
    private OperationVector2DifficultyBase[] m_Vector2;


    /// <summary>
    /// 初期化する
    /// </summary>
    public override void Setup()
    {
        E_DIFFICULTY? difficulty = null;

        if (m_Float != null)
        {
            foreach (OperationFloatDifficultyBase operation in m_Float)
            {
                operation.Setup(difficulty);
            }
        }

        if (m_Int != null)
        {
            foreach (OperationIntDifficultyBase operation in m_Int)
            {
                operation.Setup(difficulty);
            }
        }

        if (m_Vector2 != null)
        {
            foreach (OperationVector2DifficultyBase operation in m_Vector2)
            {
                operation.Setup(difficulty);
            }
        }
    }
}

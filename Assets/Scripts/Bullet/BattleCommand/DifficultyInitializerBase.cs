using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度によって変わる値を初期化するためのクラスの抽象クラス。
/// </summary>
[System.Serializable]
public abstract class DifficultyInitializerBase : ScriptableObject
{
    /// <summary>
    /// 初期化する
    /// </summary>
    public abstract void Setup();
}

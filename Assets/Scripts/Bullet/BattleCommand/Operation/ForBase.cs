using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// for文を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class ForBase : ScriptableObject
{

    /// <summary>
    /// 攻撃が始まる前の初期化をする
    /// </summary>
    public abstract void Setup();

    /// <summary>
    /// 初期値を設定する
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 条件判定をする
    /// </summary>
    public abstract bool IsTrue();

    /// <summary>
    /// 操作をする
    /// </summary>
    public abstract void Process();
}

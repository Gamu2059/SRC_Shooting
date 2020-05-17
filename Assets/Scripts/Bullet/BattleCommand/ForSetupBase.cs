using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃が始まる前の初期化の処理だけを表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class ForSetupBase : ScriptableObject
{

    /// <summary>
    /// 攻撃が始まる前の初期化をする
    /// </summary>
    public abstract void Setup();
}

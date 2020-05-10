using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の発射を表すクラスの抽象クラス。
/// </summary>
[System.Serializable]
public abstract class BulletShotParamBase : ScriptableObject
{
    /// <summary>
    /// 最初に一度だけ行われる初期化の処理
    /// </summary>
    public abstract void OnStarts();

    /// <summary>
    /// 最初に一度だけ行われる初期化の処理
    /// </summary>
    public abstract void OnUpdates(CommandCharaController owner, E_COMMON_SOUND shotSE);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 衝突処理に関連するインタフェース
/// </summary>
public interface IColliderProcess
{
    /// <summary>
    /// 衝突フラグをクリアする。
    /// </summary>
    void ClearColliderFlag();

    /// <summary>
    /// 衝突情報を更新する。
    /// </summary>
    void UpdateCollider();

    /// <summary>
    /// 実際の衝突処理を呼び出す。
    /// </summary>
    void ProcessCollision();
}

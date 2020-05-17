using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハッキングの難易度を表すクラス。（一時的なもの）
/// </summary>
public class DanmakuDifficulty : object
{
    /// <summary>
    /// 難易度を取得する
    /// </summary>
    public static E_DIFFICULTY GetDifficulty()
    {
        return DataManager.Instance.Difficulty;
    }
}

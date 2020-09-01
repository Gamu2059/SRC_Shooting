using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハッキングの難易度を表すクラス。（一時的なもの）
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/difficulty", fileName = "DanmakuDifficulty", order = 0)]
[System.Serializable]
public class DanmakuDifficulty : ScriptableObject
{
    /// <summary>
    /// 難易度を取得する
    /// </summary>
    public static E_DIFFICULTY GetDifficulty()
    {
        return DataManager.Instance.Difficulty;
    }
}

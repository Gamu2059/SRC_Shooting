using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 汎用音の列挙型
/// </summary>
public enum E_COMMON_SOUND
{
    /// <summary>
    /// 決定SE
    /// </summary>
    SYSTEM_OK,

    /// <summary>
    /// フォーカス移動SE
    /// </summary>
    SYSTEM_CURSOR,

    /// <summary>
    /// キャンセルSE
    /// </summary>
    SYSTEM_CANCEL,

    /// <summary>
    /// リザルトUIのスライドSE
    /// </summary>
    SYSTEM_SLIDE,

    /// <summary>
    /// シューティングパート開始ボタンSE
    /// </summary>
    SYSTEM_START,

    /// <summary>
    /// ハッキングモードへ遷移する時のSE
    /// </summary>
    HACK_START,

    /// <summary>
    /// ハッキングモードから戻ってくる時のSE
    /// </summary>
    HACK_END,

    /// <summary>
    /// プレイヤーのエナジーストックが1つ増えた時のSE
    /// </summary>
    POWER_UP,

    /// <summary>
    /// ステージのゲームクリアBGM
    /// </summary>
    GAME_CLEAR,

    /// <summary>
    /// ゲームオーバーBGM
    /// </summary>
    GAME_OVER,
}

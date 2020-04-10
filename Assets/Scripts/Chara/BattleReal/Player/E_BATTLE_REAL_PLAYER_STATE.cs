using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BATTLE_REAL_PLAYER_STATE
{
    /// <summary>
    /// リアルモードがGAMEステートでない時に遷移するデフォルトステート
    /// </summary>
    NON_GAME,

    /// <summary>
    /// リアルモードがGAMEステートの時に遷移するデフォルトステート
    /// </summary>
    GAME,

    /// <summary>
    /// リアルモードがGAMEステートの時、かつチャージ中の時に遷移するステート
    /// </summary>
    CHARGE,

    /// <summary>
    /// チャージショットを放つ瞬間だけ遷移するステート
    /// </summary>
    CHARGE_SHOT,

    /// <summary>
    /// シーケンスによる自動制御を受けるステート
    /// </summary>
    SEQUENCE,

    /// <summary>
    /// 撃破された瞬間だけ遷移するステート
    /// </summary>
    DEAD,
}

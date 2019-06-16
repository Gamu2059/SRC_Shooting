using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// スマッシャーボム(レーザー)の傾いていく方向を決める
/// </summary>
public enum E_BOMB_ROTATE_DIR
{
    /// <summary>
    /// 左に傾いていく
    /// angle = -1 * anglespeed * Time.deltaTime
    /// </summary>
    LEFT = -1,

    /// <summary>
    /// 右に傾いていく
    /// angle = 1 * anglespeed * Time.deltaTime
    /// </summary>
    RIGHT = 1,
}

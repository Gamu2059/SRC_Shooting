using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラの種類
/// </summary>
public enum E_CAMERA_TYPE
{
    /// <summary>
    /// 背景を投影するカメラ
    /// </summary>
    BACK_CAMERA,

    /// <summary>
    /// キャラや弾などを投影するカメラ
    /// </summary>
    FRONT_CAMERA,
}
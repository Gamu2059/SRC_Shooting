using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラ制御のパラメータ
/// </summary>
[System.Serializable]
public struct ControlCameraParam
{
    [Tooltip("カメラタイプ")]
    public E_CAMERA_TYPE CameraType;

    [Tooltip("カメラのタイムラインパラメータ")]
    public TimelineParam CameraTimelineParam;
}

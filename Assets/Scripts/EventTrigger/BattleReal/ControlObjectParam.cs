using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクト制御のパラメータ
/// </summary>
[System.Serializable]
public struct ControlObjectParam
{
    [Tooltip("プレハブを使うかどうか")]
    public bool UsePlayableObjectPrefab;

    [Tooltip("プレハブ")]
    public BattleRealPlayableBase PlayableObjectPrefab;

    [Tooltip("EventManagerに登録されているオブジェクトの名前")]
    public string RegisteredPlayableName;

    [Tooltip("オブジェクトのタイムラインパラメータ")]
    public TimelineParam ObjectTimelineParam;
}

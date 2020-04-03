using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクト制御のパラメータ
/// </summary>
[System.Serializable]
public struct ControlObjectParam
{
    [Tooltip("操作対象のオブジェクトのプレハブ")]
    public BattleRealSequenceObjectController SequenceObjectPrefab;

    [Tooltip("オブジェクトを操作する制御内容")]
    public SequenceGroup RootGroup;
}
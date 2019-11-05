using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクト制御のパラメータ
/// </summary>
[System.Serializable]
public struct ControlObjectParam
{
    [Tooltip("操作対象のオブジェクトの名前")]
    public string Name;

    [Tooltip("オブジェクトの操作タイプ")]
    public E_CONTROL_OBJECT_TYPE ControlObjectType;
}
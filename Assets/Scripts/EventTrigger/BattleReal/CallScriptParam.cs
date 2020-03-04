using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スクリプト呼び出しのパラメータ
/// </summary>
[System.Serializable]
public struct CallScriptParam
{
    [Tooltip("スクリプト名(name spaceを含む)")]
    public string ScriptName;

    [Tooltip("スクリプトに渡す引数")]
    public ArgumentVariable[] ScriptArguments;
}

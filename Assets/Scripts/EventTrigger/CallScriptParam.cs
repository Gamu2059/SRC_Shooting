using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スクリプト呼び出しのパラメータ
/// </summary>
public struct CallScriptParam
{
    [Tooltip("スクリプト名")]
    public string ScriptName;

    [Tooltip("スクリプトに渡す引数")]
    public ArgumentVariable[] ScriptArguments;
}

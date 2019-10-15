using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リアルモードのカメラコントローラ。
/// </summary>
[RequireComponent(typeof(Camera))]
public class BattleRealCameraController : BattleRealPlayableBase
{
    public Camera Camera { get; private set; }

    public override void OnInitialize()
    {
        base.OnInitialize();

        Camera = GetComponent<Camera>();
    }
}

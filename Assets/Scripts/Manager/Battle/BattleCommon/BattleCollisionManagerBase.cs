using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 衝突処理を管理する基底クラス。
/// </summary>
public abstract class BattleCollisionManagerBase : ControllableObject
{
    public Material CollisionMaterial { get; private set; }

    public override void OnInitialize()
    {
        base.OnInitialize();

        var shader = Shader.Find("Custom/CollisionRender");
        CollisionMaterial = new Material(shader);
        CollisionMaterial.hideFlags = HideFlags.HideAndDontSave;
        CollisionMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
    }
}

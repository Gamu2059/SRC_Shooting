#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハッキングシールドのエフェクトクラス。
/// </summary>
public class BattleRealHackingShield : BattleCommonEffectController
{
    [SerializeField]
    private bool m_ForwardToPlayer;

    public override void OnInitialize()
    {
        base.OnInitialize();

        if (!m_ForwardToPlayer)
        {
            return;
        }

        var player = BattleRealPlayerManager.Instance.Player;
        var camera = BattleRealCameraManager.Instance.GetFrontCamera();
        if (Owner == null || player == null || camera == null)
        {
            DestroyEffect(true);
            return;
        }

        var delta = player.transform.position - Owner.position;
        delta.y = 0;
        transform.position = Owner.position + delta / 2f;
        transform.LookAt(camera.transform.position);
        var angles = transform.eulerAngles;
        angles.z = 0;
        transform.eulerAngles = angles;
    }
}

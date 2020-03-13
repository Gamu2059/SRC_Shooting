#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponIndicator : ControllableMonoBehavior
{
    private const string SELECT_TO_BOMB = "select_to_bomb";
    private const string SELECT_TO_LASER = "select_to_laser";

    [SerializeField]
    private Animator m_IconAnimator;

    private bool m_SetupCallback;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_SetupCallback = false;
    }

    public override void OnFinalize()
    {
        if (BattleRealPlayerManager.Instance.Player != null)
        {
            BattleRealPlayerManager.Instance.Player.OnChangeWeaponType -= OnChangeWeaponType;
        }

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (BattleRealPlayerManager.Instance.Player != null && !m_SetupCallback)
        {
            m_SetupCallback = true;
            BattleRealPlayerManager.Instance.Player.OnChangeWeaponType += OnChangeWeaponType;
        }
    }

    #endregion

    private void OnChangeWeaponType(bool isLaserType)
    {
        if (isLaserType)
        {
            // レーザータイプになったのなら
            m_IconAnimator.Play(SELECT_TO_LASER, 0);
        }
        else
        {
            // ボムタイプになったのなら
            m_IconAnimator.Play(SELECT_TO_BOMB, 0);
        }
    }
}

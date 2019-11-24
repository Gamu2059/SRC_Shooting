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

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        BattleRealPlayerManager.Instance.OnChangeWeaponType += OnChangeWeaponType;
    }

    public override void OnFinalize()
    {
        BattleRealPlayerManager.Instance.OnChangeWeaponType -= OnChangeWeaponType;

        base.OnFinalize();
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

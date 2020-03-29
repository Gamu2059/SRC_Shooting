using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルリアルのボスUIを統括するクラス。
/// </summary>
public class BattleRealBossUI : ControllableMonoBehavior
{
    #region Field Inspector

    [SerializeField]
    private IconGageIndicator m_HpGage;

    [SerializeField]
    private IconGageIndicator m_DownGage;

    [SerializeField]
    private RemainingHackingNumIndicator m_RemainingHackingNum;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_HpGage.OnInitialize();
        m_DownGage.OnInitialize();
        m_RemainingHackingNum.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_RemainingHackingNum.OnFinalize();
        m_DownGage.OnFinalize();
        m_HpGage.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_HpGage.OnUpdate();
        m_DownGage.OnUpdate();
        m_RemainingHackingNum.OnUpdate();
    }

    #endregion


}

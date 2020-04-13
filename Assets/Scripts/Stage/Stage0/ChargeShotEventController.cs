using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チャージショットアクションのコールバックでイベント変数を操作する。<br/>
/// Key : EventVarName<br/>
/// Value : trueにするイベントBOOL変数
/// </summary>
public class ChargeShotEventController : EventControllableScript
{
    private const string EVENT_VAR_NAME = "EventVarName";

    private string m_EventVarName;
    private bool m_IsShotLaser;
    private bool m_IsShotBomb;

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_ParamSet.ApplyStringParam(EVENT_VAR_NAME, ref m_EventVarName);
        m_IsShotLaser = false;
        m_IsShotBomb = false;
        DataManager.Instance.BattleData.ConsumeEnergyStockAction += OnConsumeEnergyStock;
    }

    public override void OnFinalize()
    {
        DataManager.Instance.BattleData.ConsumeEnergyStockAction -= OnConsumeEnergyStock;

        var player = BattleRealPlayerManager.Instance.Player;
        player.ChangeStateAction -= OnChangeStatePlayer;

        base.OnFinalize();
    }

    private void OnConsumeEnergyStock()
    {
        var player = BattleRealPlayerManager.Instance.Player;
        if (player.IsLaserType)
        {
            m_IsShotLaser = true;
        }
        else
        {
            m_IsShotBomb = true;
        }

        if (m_IsShotLaser && m_IsShotBomb)
        {
            DataManager.Instance.BattleData.ConsumeEnergyStockAction -= OnConsumeEnergyStock;
            player.ChangeStateAction += OnChangeStatePlayer;
        }
    }

    private void OnChangeStatePlayer(E_BATTLE_REAL_PLAYER_STATE state)
    {
        if (state == E_BATTLE_REAL_PLAYER_STATE.GAME)
        {
            var player = BattleRealPlayerManager.Instance.Player;
            player.ChangeStateAction -= OnChangeStatePlayer;
            BattleRealEventManager.Instance.SetBool(m_EventVarName, true);
            DestroyScript();
        }
    }
}

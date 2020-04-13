using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チャージショットを検出してイベント変数を操作する。<br/>
/// Key : EventVarName<br/>
/// Value : trueにするイベントBOOL変数
/// </summary>
public class EnergyStockIncreaseEventController : EventControllableScript
{
    private const string EVENT_VAR_NAME = "EventVarName";

    private string m_EventVarName;

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_ParamSet.ApplyStringParam(EVENT_VAR_NAME, ref m_EventVarName);
        DataManager.Instance.BattleData.IncreaseEnergyStockAction += OnIncreaseEnergyStock;
    }

    public override void OnFinalize()
    {
        DataManager.Instance.BattleData.IncreaseEnergyStockAction -= OnIncreaseEnergyStock;
        base.OnFinalize();
    }

    private void OnIncreaseEnergyStock()
    {
        BattleRealEventManager.Instance.SetBool(m_EventVarName, true);
        DataManager.Instance.BattleData.IncreaseEnergyStockAction -= OnIncreaseEnergyStock;
        DestroyScript();
    }
}

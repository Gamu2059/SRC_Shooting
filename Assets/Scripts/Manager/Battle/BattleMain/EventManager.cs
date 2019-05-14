using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleMainのイベントトリガを管理するマネージャ。
/// </summary>
public class EventManager : BattleSingletonMonoBehavior<EventManager>
{
    public const string GAME_START_TIME_PERIOD_NAME = "Game Start";

    #region Field Inspector

    /// <summary>
    /// イベント変数やタイムピリオドのセット
    /// </summary>
    [SerializeField]
    private EventTriggerParamSet m_ParamSet;

    [SerializeField]
    private EventTriggerCondition m_Condition;


    #endregion

    #region Field

    private Dictionary<string, int> m_IntVariables;
    private Dictionary<string, float> m_FloatVariables;
    private Dictionary<string, bool> m_BoolVariables;
    private Dictionary<string, EventTriggerTimePeriod> m_TimePeriods;

    private EventTriggerTimePeriod m_GameStartTimePeriod;

    #endregion



    public override void OnInitialize()
    {
        base.OnInitialize();

        m_IntVariables = new Dictionary<string, int>();
        m_FloatVariables = new Dictionary<string, float>();
        m_BoolVariables = new Dictionary<string, bool>();
        m_TimePeriods = new Dictionary<string, EventTriggerTimePeriod>();

        if (m_ParamSet == null)
        {
            Debug.LogWarning("EventTriggerParamSetがありません");
            return;
        }

        foreach(var variable in m_ParamSet.GetVariables())
        {
            switch(variable.Type)
            {
                case E_EVENT_TRIGGER_VARIABLE_TYPE.INT:
                    m_IntVariables.Add(variable.Name, variable.IntInitValue);
                    break;
                case E_EVENT_TRIGGER_VARIABLE_TYPE.FLOAT:
                    m_FloatVariables.Add(variable.Name, variable.FloatInitValue);
                    break;
                case E_EVENT_TRIGGER_VARIABLE_TYPE.BOOL:
                    m_BoolVariables.Add(variable.Name, variable.BoolInitValue);
                    break;
            }
        }

        m_GameStartTimePeriod = new EventTriggerTimePeriod();
        m_TimePeriods.Add(GAME_START_TIME_PERIOD_NAME, m_GameStartTimePeriod);

        foreach(var periodName in m_ParamSet.GetTimePeriodNames())
        {
            m_TimePeriods.Add(periodName, new EventTriggerTimePeriod());
        }
    }

    public override void OnFinalize()
    {
        m_IntVariables = null;
        m_FloatVariables = null;
        m_BoolVariables = null;
        m_TimePeriods = null;

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();


    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        foreach(var period in m_TimePeriods.Values)
        {
            period.OnFixedUpdate();
        }
    }



    /// <summary>
    /// int型イベント変数の値を取得する。
    /// 名前が存在しなければデフォルト値を返す。
    /// </summary>
    public int GetInt(string name, int _default = 0)
    {
        if (m_IntVariables == null || !m_IntVariables.ContainsKey(name))
        {
            return _default;
        }

        return m_IntVariables[name];
    }

    /// <summary>
    /// float型イベント変数の値を取得する。
    /// 名前が存在しなければデフォルト値を返す。
    /// </summary>
    public float GetFloat(string name, float _default = 0f)
    {
        if (m_FloatVariables == null || !m_FloatVariables.ContainsKey(name))
        {
            return _default;
        }

        return m_FloatVariables[name];
    }

    /// <summary>
    /// bool型イベント変数の値を取得する。
    /// 名前が存在しなければデフォルト値を返す。
    /// </summary>
    public bool GetBool(string name, bool _default = false)
    {
        if (m_BoolVariables == null || !m_BoolVariables.ContainsKey(name))
        {
            return _default;
        }

        return m_BoolVariables[name];
    }

    /// <summary>
    /// int型イベント変数の値を設定する。
    /// 名前が存在しなければ無視する。
    /// </summary>
    public void SetInt(string name, int value)
    {
        if (m_IntVariables == null || !m_IntVariables.ContainsKey(name))
        {
            return;
        }

        m_IntVariables[name] = value;
    }

    /// <summary>
    /// float型イベント変数の値を設定する。
    /// 名前が存在しなければ無視する。
    /// </summary>
    public void SetFloat(string name, float value)
    {
        if (m_FloatVariables == null || !m_FloatVariables.ContainsKey(name))
        {
            return;
        }

        m_FloatVariables[name] = value;
    }

    /// <summary>
    /// bool型イベント変数の値を設定する。
    /// 名前が存在しなければ無視する。
    /// </summary>
    public void SetBool(string name, bool value)
    {
        if (m_BoolVariables == null || !m_BoolVariables.ContainsKey(name))
        {
            return;
        }

        m_BoolVariables[name] = value;
    }
}

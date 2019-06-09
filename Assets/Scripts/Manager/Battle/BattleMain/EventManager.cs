using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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


    #endregion

    #region Field

    private Dictionary<string, int> m_IntVariables;
    private Dictionary<string, float> m_FloatVariables;
    private Dictionary<string, bool> m_BoolVariables;
    private Dictionary<string, EventTriggerTimePeriod> m_TimePeriods;

    private List<EventTriggerParamSet.EventTriggerParam> m_EventParams;
    private List<EventTriggerParamSet.EventTriggerParam> m_GotoDestroyEventParams;

    private EventTriggerTimePeriod m_GameStartTimePeriod;

    private List<EventContent> m_WaitExecuteParams;

    private List<EventControllableScript> m_UpdateScripts;

    private List<EventControllableScript> m_GotoDestroyScripts;

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

        foreach (var variable in m_ParamSet.GetVariables())
        {
            switch (variable.Type)
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

        foreach (var periodName in m_ParamSet.GetTimePeriodNames())
        {
            m_TimePeriods.Add(periodName, new EventTriggerTimePeriod());
        }

        m_EventParams = new List<EventTriggerParamSet.EventTriggerParam>();
        m_GotoDestroyEventParams = new List<EventTriggerParamSet.EventTriggerParam>();
        m_EventParams.AddRange(m_ParamSet.GetParams());

        m_WaitExecuteParams = new List<EventContent>();

        m_UpdateScripts = new List<EventControllableScript>();
        m_GotoDestroyScripts = new List<EventControllableScript>();
    }

    public override void OnFinalize()
    {
        m_IntVariables = null;
        m_FloatVariables = null;
        m_BoolVariables = null;
        m_TimePeriods = null;
        m_EventParams = null;
        m_GotoDestroyEventParams = null;

        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        m_GameStartTimePeriod.CountStart();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 条件判定
        for (int i = 0; i < m_EventParams.Count; i++)
        {
            var param = m_EventParams[i];
            if (IsMeetCondition(ref param.Condition))
            {
                RegistEvent(param.Contents);
                m_GotoDestroyEventParams.Add(m_EventParams[i]);
            }
        }

        DestroyEventTrigger();

        // イベント実行
        foreach(var param in m_WaitExecuteParams)
        {
            ExecuteEvent(param);
        }

        m_WaitExecuteParams.Clear();

        // スクリプト実行
        foreach(var script in m_UpdateScripts)
        {
            if (script == null)
            {
                continue;
            }

            if (script.GetCycle() == E_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                script.OnStart();
                script.SetCycle(E_OBJECT_CYCLE.STANDBY_UPDATE);
            }

            script.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        // スクリプト実行
        foreach (var script in m_UpdateScripts)
        {
            if (script == null)
            {
                continue;
            }

            script.OnLateUpdate();
        }

        DestroyScript();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        foreach (var period in m_TimePeriods.Values)
        {
            period.OnFixedUpdate();
        }

        // スクリプト実行
        foreach (var script in m_UpdateScripts)
        {
            if (script == null)
            {
                continue;
            }

            script.OnFixedUpdate();
        }
    }

    /// <summary>
    /// EventParamを追加する。
    /// </summary>
    public void AddEventParam(EventTriggerParamSet.EventTriggerParam param)
    {
        var condition = param.Condition;
        if (!condition.IsSingleCondition && condition.Conditions == null)
        {
            return;
        }

        var contents = param.Contents;
        if (contents == null || contents.Length < 1)
        {
            return;
        }

        m_EventParams.Add(param);
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

    private void DestroyEventTrigger()
    {
        int count = m_GotoDestroyEventParams.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var param = m_GotoDestroyEventParams[idx];

            m_GotoDestroyEventParams.RemoveAt(idx);
            m_EventParams.Remove(param);
        }

        m_GotoDestroyEventParams.Clear();
    }

    /// <summary>
    /// 条件を満たしているかどうかを判定する。
    /// </summary>
    public bool IsMeetCondition(ref EventTriggerCondition condition)
    {
        bool result = false;
        if (condition.IsSingleCondition)
        {
            switch (condition.VariableType)
            {
                case EventTriggerCondition.E_VARIABLE_TYPE.INT:
                    result = CompareInt(condition);
                    break;
                case EventTriggerCondition.E_VARIABLE_TYPE.FLOAT:
                    result = CompareFloat(condition);
                    break;
                case EventTriggerCondition.E_VARIABLE_TYPE.BOOL:
                    result = CompareBool(condition);
                    break;
                case EventTriggerCondition.E_VARIABLE_TYPE.TIME_PERIOD:
                    result = CompareTimePeriod(condition);
                    break;
            }
        }
        else
        {
            // 複数条件の時の初期値
            result = condition.MultiConditionType == EventTriggerCondition.E_MULTI_CONDITION_TYPE.AND;

            for (int i = 0; i < condition.Conditions.Length; i++)
            {
                bool isMeet = IsMeetCondition(ref condition.Conditions[i]);

                if (condition.MultiConditionType == EventTriggerCondition.E_MULTI_CONDITION_TYPE.OR && isMeet)
                {
                    result = true;
                    break;
                }
                else if (condition.MultiConditionType == EventTriggerCondition.E_MULTI_CONDITION_TYPE.AND && !isMeet)
                {
                    result = false;
                    break;
                }
            }
        }

        if (condition.IsReverse)
        {
            result = !result;
        }

        return result;
    }

    /// <summary>
    /// int型の比較演算を行う。
    /// </summary>
    private bool CompareInt(EventTriggerCondition condition)
    {
        if (m_IntVariables == null || !m_IntVariables.ContainsKey(condition.VariableName))
        {
            return false;
        }

        var value = m_IntVariables[condition.VariableName];

        switch (condition.CompareType)
        {
            case EventTriggerCondition.E_COMPARE_TYPE.EQUAL:
                return value == condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.NOT_EQUAL:
                return value != condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.LESS_THAN:
                return value < condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.LESS_THAN_EQUAL:
                return value <= condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.MORE_THAN:
                return value > condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.MORE_THAN_EQUAL:
                return value >= condition.CompareValue;
        }

        return false;
    }

    /// <summary>
    /// float型の比較演算を行う。
    /// </summary>
    private bool CompareFloat(EventTriggerCondition condition)
    {
        if (m_FloatVariables == null || !m_FloatVariables.ContainsKey(condition.VariableName))
        {
            return false;
        }

        var value = m_FloatVariables[condition.VariableName];

        switch (condition.CompareType)
        {
            case EventTriggerCondition.E_COMPARE_TYPE.EQUAL:
                return value == condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.NOT_EQUAL:
                return value != condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.LESS_THAN:
                return value < condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.LESS_THAN_EQUAL:
                return value <= condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.MORE_THAN:
                return value > condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.MORE_THAN_EQUAL:
                return value >= condition.CompareValue;
        }

        return false;
    }

    /// <summary>
    /// bool型の比較演算を行う。
    /// </summary>
    private bool CompareBool(EventTriggerCondition condition)
    {
        if (m_BoolVariables == null || !m_BoolVariables.ContainsKey(condition.VariableName))
        {
            return false;
        }

        var value = m_BoolVariables[condition.VariableName];

        switch (condition.BoolCompareType)
        {
            case EventTriggerCondition.E_BOOL_COMPARE_TYPE.EQUAL:
                return value == condition.BoolCompareValue;
            case EventTriggerCondition.E_BOOL_COMPARE_TYPE.NOT_EQUAL:
                return value != condition.BoolCompareValue;
        }

        return false;
    }

    /// <summary>
    /// タイムピリオドの比較演算を行う。
    /// </summary>
    private bool CompareTimePeriod(EventTriggerCondition condition)
    {
        if (m_TimePeriods == null || !m_TimePeriods.ContainsKey(condition.VariableName))
        {
            return false;
        }

        var period = m_TimePeriods[condition.VariableName];
        var value = period.GetPeriod();
        switch (condition.CompareType)
        {
            case EventTriggerCondition.E_COMPARE_TYPE.EQUAL:
                return value == condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.NOT_EQUAL:
                return value != condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.LESS_THAN:
                return value < condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.LESS_THAN_EQUAL:
                return value <= condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.MORE_THAN:
                return value > condition.CompareValue;
            case EventTriggerCondition.E_COMPARE_TYPE.MORE_THAN_EQUAL:
                return value >= condition.CompareValue;
        }
        return false;
    }

    /// <summary>
    /// イベントを登録する。
    /// </summary>
    private void RegistEvent(EventContent[] contents)
    {
        if (contents == null)
        {
            return;
        }

        for(int i=0;i<contents.Length;i++)
        {
            var content = contents[i];
            if (content.ExecuteTiming == EventContent.E_EXECUTE_TIMING.IMMEDIATE)
            {
                m_WaitExecuteParams.Add(content);
            } else
            {
                var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, content.DelayExecuteTime, ()=> {
                    m_WaitExecuteParams.Add(content);
                });

                BattleMainTimerManager.Instance.RegistTimer(timer);
            }
        }

    }

    /// <summary>
    /// イベントを実行する。
    /// </summary>
    public void ExecuteEvent(EventContent eventContent)
    {
        switch(eventContent.EventType)
        {
            case EventContent.E_EVENT_TYPE.APPER_ENEMY:
                EnemyCharaManager.Instance.CreateEnemyFromEnemyParam(eventContent.ApperEnemyIndex);
                break;
            case EventContent.E_EVENT_TYPE.CONTROL_CAMERA:
                break;
            case EventContent.E_EVENT_TYPE.CONTROL_OBJECT:
                break;
            case EventContent.E_EVENT_TYPE.CONTROL_BGM:
                ExecuteControlBgm(eventContent.ControlBgmParam);
                break;
            case EventContent.E_EVENT_TYPE.OPERATE_VARIABLE:
                break;
            case EventContent.E_EVENT_TYPE.OPERATE_TIME_PERIOD:
                break;
            case EventContent.E_EVENT_TYPE.CALL_SCRIPT:
                ExecuteCallScript(eventContent.ScriptName, eventContent.ScriptArguments);
                break;
        }
    }

    private void ExecuteControlBgm(ControlBgmParam param)
    {
        if (param.ControlType == ControlBgmParam.E_BGM_CONTROL_TYPE.PLAY)
        {
            FadeAudioManager.Instance.PlayBGM(param.BgmClip, param.FadeOutDuration, param.FadeInStartOffset, param.FadeInDuration);
        }
        else
        {
            FadeAudioManager.Instance.StopBGM(param.FadeOutDuration);
        }
    }

    private void ExecuteCallScript(string scriptName, ArgumentVariable[] args)
    {
        Type type = Type.GetType(scriptName);

        if (type == null || !type.IsSubclassOf(typeof(EventControllableScript)))
        {
            return;
        }

        var script = (EventControllableScript)Activator.CreateInstance(type);
        m_UpdateScripts.Add(script);
        script.SetCycle(E_OBJECT_CYCLE.STANDBY_UPDATE);
        script.OnInitialize();
    }

    public void CheckDestroyScript(EventControllableScript script)
    {
        if (script == null || !m_UpdateScripts.Contains(script) || m_GotoDestroyScripts.Contains(script))
        {
            return;
        }

        m_GotoDestroyScripts.Add(script);
        script.SetCycle(E_OBJECT_CYCLE.STANDBY_DESTROYED);
    }

    private void DestroyScript()
    {
        foreach(var script in m_GotoDestroyScripts)
        {
            m_UpdateScripts.Remove(script);
            script.OnFinalize();
        }

        m_GotoDestroyScripts.Clear();
    }
}

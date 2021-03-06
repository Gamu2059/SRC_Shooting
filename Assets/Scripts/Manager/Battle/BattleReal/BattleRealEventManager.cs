﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// BattleRealのイベントトリガを管理するマネージャ。
/// </summary>
public class BattleRealEventManager : Singleton<BattleRealEventManager>
{
    #region Readonly or Const Field

    private const string BOSS_DEFEAT_INT_NAME = "Boss Defeat";
    private const string BOSS_RESCUE_INT_NAME = "Boss Rescue";

    private const string BATTLE_LOADED_TIME_PRERIOD_NAME = "Battle Loaded";
    private const string GAME_START_TIME_PERIOD_NAME = "Game Start";

    private readonly Dictionary<E_GENERAL_INT_VARIABLE, string> m_GeneralIntNames = new Dictionary<E_GENERAL_INT_VARIABLE, string>()
    {
        { E_GENERAL_INT_VARIABLE.BOSS_DEFEAT, BOSS_DEFEAT_INT_NAME },
        { E_GENERAL_INT_VARIABLE.BOSS_RESCUE, BOSS_RESCUE_INT_NAME },
    };

    private readonly Dictionary<E_GENERAL_TIME_PERIOD, string> m_GeneralTimePeriodNames = new Dictionary<E_GENERAL_TIME_PERIOD, string>()
    {
        { E_GENERAL_TIME_PERIOD.BATTLE_LOADED, BATTLE_LOADED_TIME_PRERIOD_NAME },
        { E_GENERAL_TIME_PERIOD.GAME_START, GAME_START_TIME_PERIOD_NAME },
    };

    #endregion

    #region Field

    private BattleRealEventTriggerParamSet m_ParamSet;

    private Dictionary<string, int> m_IntVariables;
    private Dictionary<string, float> m_FloatVariables;
    private Dictionary<string, bool> m_BoolVariables;
    private Dictionary<string, EventTriggerTimePeriod> m_TimePeriods;

    private List<BattleRealEventTriggerParam> m_EventParams;
    private List<BattleRealEventTriggerParam> m_GotoDestroyEventParams;

    private List<BattleRealEventContent> m_WaitExecuteParams;

    private List<EventControllableScript> m_UpdateScripts;

    private List<EventControllableScript> m_GotoDestroyScripts;

    #endregion

    #region Closed Callback

    private Action<E_BATTLE_REAL_STATE> RequestChangeStateBattleRealManager { get; set; }
    private Action<ShowCutsceneParam> ShowCutsceneAction { get; set; }
    private Action<ShowTalkParam> ShowTalkAction { get; set; }
    private Action<ShowDialogParam> ShowDialogAction { get; set; }
    private Action GameClearWithoutHackingCompleteAction { get; set; }
    private Action GameClearWithHackingCompleteAction { get; set; }
    private Action GameOverAction { get; set; }

    #endregion

    public static BattleRealEventManager Builder(BattleRealManager realManager, BattleRealEventTriggerParamSet param)
    {
        var manager = Create();
        manager.SetParam(param);
        manager.SetCallback(realManager);
        manager.OnInitialize();
        return manager;
    }

    private void SetParam(BattleRealEventTriggerParamSet param)
    {
        m_ParamSet = param;
    }

    private void SetCallback(BattleRealManager manager)
    {
        RequestChangeStateBattleRealManager += manager.RequestChangeState;
        ShowCutsceneAction += manager.ShowCutscene;
        ShowTalkAction += manager.ShowTalk;
        ShowDialogAction += manager.ShowDialog;
        GameClearWithoutHackingCompleteAction += manager.GameClearWithoutHackingComplete;
        GameClearWithHackingCompleteAction += manager.GameClearWithHackingComplete;
        GameOverAction += manager.GameOver;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        if (m_ParamSet == null)
        {
            Debug.LogWarning("EventTriggerParamSetがありません");
            return;
        }

        InitVariable();
        InitTimePeriod();

        m_EventParams = new List<BattleRealEventTriggerParam>();
        m_GotoDestroyEventParams = new List<BattleRealEventTriggerParam>();
        m_EventParams.AddRange(m_ParamSet.Params);

        m_WaitExecuteParams = new List<BattleRealEventContent>();

        m_UpdateScripts = new List<EventControllableScript>();
        m_GotoDestroyScripts = new List<EventControllableScript>();
    }

    private void InitVariable()
    {
        m_IntVariables = new Dictionary<string, int>();
        m_FloatVariables = new Dictionary<string, float>();
        m_BoolVariables = new Dictionary<string, bool>();

        // 組み込み系の追加 BattleDataに元データを移した
        //m_IntVariables.Add(BOSS_DEFEAT_INT_NAME, 0);
        //m_IntVariables.Add(BOSS_RESCUE_INT_NAME, 0);

        foreach (var variable in m_ParamSet.Variables)
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
    }

    private void InitTimePeriod()
    {
        m_TimePeriods = new Dictionary<string, EventTriggerTimePeriod>();

        // 組み込み系の追加
        m_TimePeriods.Add(BATTLE_LOADED_TIME_PRERIOD_NAME, new EventTriggerTimePeriod());
        m_TimePeriods.Add(GAME_START_TIME_PERIOD_NAME, new EventTriggerTimePeriod());

        foreach (var periodName in m_ParamSet.TimePeriodNames)
        {
            m_TimePeriods.Add(periodName, new EventTriggerTimePeriod());
        }
    }

    public override void OnFinalize()
    {
        GameOverAction = null;
        GameClearWithHackingCompleteAction = null;
        GameClearWithoutHackingCompleteAction = null;
        ShowDialogAction = null;
        ShowTalkAction = null;
        ShowCutsceneAction = null;
        RequestChangeStateBattleRealManager = null;

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
        CountStartTimePeriod(BATTLE_LOADED_TIME_PRERIOD_NAME);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 条件判定
        for (int i = 0; i < m_EventParams.Count; i++)
        {
            var param = m_EventParams[i];
            if (IsMeetRootCondition(ref param.Condition))
            {
                AddEvent(param.Contents);
                if (!param.DontDestroy)
                {
                    m_GotoDestroyEventParams.Add(m_EventParams[i]);
                }
            }
        }

        DestroyEventTrigger();

        // イベント実行
        m_WaitExecuteParams.ForEach(p => ExecuteEvent(p));
        m_WaitExecuteParams.Clear();

        // スクリプト実行
        foreach (var script in m_UpdateScripts)
        {
            if (script == null)
            {
                continue;
            }

            if (script.GetCycle() == E_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                script.OnStart();
                script.SetCycle(E_OBJECT_CYCLE.UPDATE);
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

    #endregion

    #region General Variable TimePeriod

    public string GetGeneralIntName(E_GENERAL_INT_VARIABLE type)
    {
        return m_GeneralIntNames != null && m_GeneralIntNames.ContainsKey(type) ? m_GeneralIntNames[type] : null;
    }

    public string GetGeneralTimePeriodName(E_GENERAL_TIME_PERIOD type)
    {
        return m_GeneralTimePeriodNames != null && m_GeneralTimePeriodNames.ContainsKey(type) ? m_GeneralTimePeriodNames[type] : null;
    }

    #endregion

    #region Variable TimePeriod Operation

    /// <summary>
    /// int型イベント変数が存在するかどうか。
    /// 存在する場合はtrueを返す。
    /// </summary>
    public bool ExistInt(string name)
    {
        switch (name)
        {
            case BOSS_DEFEAT_INT_NAME:
            case BOSS_RESCUE_INT_NAME:
                return true;
        }

        var exist = m_IntVariables != null && m_IntVariables.ContainsKey(name);
        if (!exist)
        {
            Debug.LogWarningFormat("int variable not exist. name : {0}", name);
        }

        return exist;
    }

    /// <summary>
    /// float型イベント変数が存在するかどうか。
    /// 存在する場合はtrueを返す。
    /// </summary>
    public bool ExistFloat(string name)
    {
        var exist = m_FloatVariables != null && m_FloatVariables.ContainsKey(name);
        if (!exist)
        {
            Debug.LogWarningFormat("float variable not exist. name : {0}", name);
        }

        return exist;
    }

    /// <summary>
    /// bool型イベント変数が存在するかどうか。
    /// 存在する場合はtrueを返す。
    /// </summary>
    public bool ExistBool(string name)
    {
        var exist = m_BoolVariables != null && m_BoolVariables.ContainsKey(name);
        if (!exist)
        {
            Debug.LogWarningFormat("bool variable not exist. name : {0}", name);
        }

        return exist;
    }

    /// <summary>
    /// タイムピリオドが存在するかどうか。
    /// 存在する場合はtrueを返す。
    /// </summary>
    public bool ExistTimePeriod(string name)
    {
        var exist = m_TimePeriods != null && m_TimePeriods.ContainsKey(name);
        if (!exist)
        {
            Debug.LogWarningFormat("time period not exist. name : {0}", name);
        }

        return exist;
    }

    /// <summary>
    /// int型イベント変数の値を取得する。
    /// 名前が存在しなければデフォルト値を返す。
    /// </summary>
    public int GetInt(string name, int _default = 0)
    {
        switch (name)
        {
            case BOSS_DEFEAT_INT_NAME:
            case BOSS_RESCUE_INT_NAME:
                return GetBattleDataInt(name, _default);
        }

        return ExistInt(name) ? m_IntVariables[name] : _default;
    }

    private int GetBattleDataInt(string name, int _default = 0)
    {
        if (DataManager.Instance == null || DataManager.Instance.BattleData == null)
        {
            return _default;
        }

        var battleData = DataManager.Instance.BattleData;
        switch (name)
        {
            case BOSS_DEFEAT_INT_NAME:
                return battleData.BossDefeatCountInChapter.Value;
            case BOSS_RESCUE_INT_NAME:
                return battleData.BossRescueCountInChapter.Value;
        }

        return _default;
    }

    /// <summary>
    /// float型イベント変数の値を取得する。
    /// 名前が存在しなければデフォルト値を返す。
    /// </summary>
    public float GetFloat(string name, float _default = 0f)
    {
        return ExistFloat(name) ? m_FloatVariables[name] : _default;
    }

    /// <summary>
    /// bool型イベント変数の値を取得する。
    /// 名前が存在しなければデフォルト値を返す。
    /// </summary>
    public bool GetBool(string name, bool _default = false)
    {
        return ExistBool(name) ? m_BoolVariables[name] : _default;
    }

    /// <summary>
    /// int型イベント変数の値を設定する。
    /// 名前が存在しなければ無視する。
    /// </summary>
    public void SetInt(string name, int value)
    {
        switch (name)
        {
            case BOSS_DEFEAT_INT_NAME:
            case BOSS_RESCUE_INT_NAME:
                SetBattleDataInt(name, value);
                return;
        }

        if (ExistInt(name))
        {
            m_IntVariables[name] = value;
        }
    }

    private void SetBattleDataInt(string name, int value)
    {
        if (DataManager.Instance == null || DataManager.Instance.BattleData == null)
        {
            return;
        }

        var battleData = DataManager.Instance.BattleData;
        switch (name)
        {
            case BOSS_DEFEAT_INT_NAME:
                battleData.BossDefeatCountInChapter.Value = value;
                break;
            case BOSS_RESCUE_INT_NAME:
                battleData.BossRescueCountInChapter.Value = value;
                break;
        }
    }

    /// <summary>
    /// float型イベント変数の値を設定する。
    /// 名前が存在しなければ無視する。
    /// </summary>
    public void SetFloat(string name, float value)
    {
        if (ExistFloat(name))
        {
            m_FloatVariables[name] = value;
        }
    }

    /// <summary>
    /// bool型イベント変数の値を設定する。
    /// 名前が存在しなければ無視する。
    /// </summary>
    public void SetBool(string name, bool value)
    {
        if (ExistBool(name))
        {
            m_BoolVariables[name] = value;
        }
    }

    /// <summary>
    /// int型イベント変数を計算する。
    /// 名前が存在しなければ無視する。
    /// </summary>
    public void CalcInt(string name, int value, E_OPERAND_TYPE operandType)
    {
        if (!ExistInt(name))
        {
            return;
        }

        var v = GetInt(name);

        switch (operandType)
        {
            case E_OPERAND_TYPE.ADD:
                SetInt(name, v + value);
                break;
            case E_OPERAND_TYPE.SUB:
                SetInt(name, v - value);
                break;
            case E_OPERAND_TYPE.MUL:
                SetInt(name, v * value);
                break;
            case E_OPERAND_TYPE.DIV:
                if (value == 0)
                {
                    Debug.LogWarningFormat("{0} : value is 0. cannot calc div.", GetType().Name);
                    return;
                }
                SetInt(name, v / value);
                break;
            case E_OPERAND_TYPE.MOD:
                if (value == 0)
                {
                    Debug.LogWarningFormat("{0} : value is 0. cannot calc mod.", GetType().Name);
                    return;
                }
                SetInt(name, v % value);
                break;
            case E_OPERAND_TYPE.SUBSTITUTE:
                SetInt(name, value);
                break;
        }
    }

    /// <summary>
    /// float型イベント変数を計算する。
    /// 名前が存在しなければ無視する。
    /// </summary>
    public void CalcFloat(string name, float value, E_OPERAND_TYPE operandType)
    {
        if (!ExistFloat(name))
        {
            return;
        }

        var v = GetFloat(name);

        switch (operandType)
        {
            case E_OPERAND_TYPE.ADD:
                SetFloat(name, v + value);
                break;
            case E_OPERAND_TYPE.SUB:
                SetFloat(name, v - value);
                break;
            case E_OPERAND_TYPE.MUL:
                SetFloat(name, v * value);
                break;
            case E_OPERAND_TYPE.DIV:
                if (value == 0)
                {
                    Debug.LogWarningFormat("{0} : value is 0. cannot calc div.", GetType().Name);
                    return;
                }
                SetFloat(name, v / value);
                break;
            case E_OPERAND_TYPE.MOD:
                if (value == 0)
                {
                    Debug.LogWarningFormat("{0} : value is 0. cannot calc mod.", GetType().Name);
                    return;
                }
                SetFloat(name, v % value);
                break;
            case E_OPERAND_TYPE.SUBSTITUTE:
                SetFloat(name, value);
                break;
        }
    }

    /// <summary>
    /// bool型イベント変数を計算する。
    /// 名前が存在しなければ無視する。
    /// </summary>
    public void CalcBool(string name, bool value, E_BOOL_OPERAND_TYPE operandType)
    {
        if (!ExistBool(name))
        {
            return;
        }

        var v = GetBool(name);

        switch (operandType)
        {
            case E_BOOL_OPERAND_TYPE.OR:
                SetBool(name, v || value);
                break;
            case E_BOOL_OPERAND_TYPE.AND:
                SetBool(name, v && value);
                break;
            case E_BOOL_OPERAND_TYPE.XOR:
                SetBool(name, (v && !value) || (!v && value));
                break;
            case E_BOOL_OPERAND_TYPE.SUBSTITUTE:
                SetBool(name, value);
                break;
            case E_BOOL_OPERAND_TYPE.NOR:
                SetBool(name, !(v || value));
                break;
            case E_BOOL_OPERAND_TYPE.NAND:
                SetBool(name, !(v && value));
                break;
            case E_BOOL_OPERAND_TYPE.XNOR:
                SetBool(name, !((v && !value) || (!v && value)));
                break;
            case E_BOOL_OPERAND_TYPE.NOT:
                SetBool(name, !value);
                break;
        }
    }

    /// <summary>
    /// 指定したタイムピリオドを取得する。
    /// 名前が存在しなければnullを返す。
    /// </summary>
    public EventTriggerTimePeriod GetTimePeriod(string name)
    {
        return ExistTimePeriod(name) ? m_TimePeriods[name] : null;
    }

    /// <summary>
    /// 指定したタイムピリオドをカウントし始める。
    /// 名前が存在しなければ無視する。
    /// </summary>
    public void CountStartTimePeriod(string name)
    {
        if (ExistTimePeriod(name))
        {
            m_TimePeriods[name].CountStart();
        }
    }

    /// <summary>
    /// int型イベント変数の比較演算を行う。
    /// </summary>
    private bool CompareInt(ref EventTriggerCondition condition)
    {
        var name = condition.UseGeneralIntVariable ? GetGeneralIntName(condition.GeneralIntVariable) : condition.VariableName;
        if (!ExistInt(name))
        {
            return false;
        }

        var v = GetInt(name);

        switch (condition.CompareType)
        {
            case E_COMPARE_TYPE.EQUAL:
                return v == condition.CompareValue;
            case E_COMPARE_TYPE.NOT_EQUAL:
                return v != condition.CompareValue;
            case E_COMPARE_TYPE.LESS_THAN:
                return v < condition.CompareValue;
            case E_COMPARE_TYPE.LESS_THAN_EQUAL:
                return v <= condition.CompareValue;
            case E_COMPARE_TYPE.MORE_THAN:
                return v > condition.CompareValue;
            case E_COMPARE_TYPE.MORE_THAN_EQUAL:
                return v >= condition.CompareValue;
        }

        return false;
    }

    /// <summary>
    /// float型イベント変数の比較演算を行う。
    /// </summary>
    private bool CompareFloat(ref EventTriggerCondition condition)
    {
        var name = condition.VariableName;
        if (!ExistFloat(name))
        {
            return false;
        }

        var v = GetFloat(name);

        switch (condition.CompareType)
        {
            case E_COMPARE_TYPE.EQUAL:
                return v == condition.CompareValue;
            case E_COMPARE_TYPE.NOT_EQUAL:
                return v != condition.CompareValue;
            case E_COMPARE_TYPE.LESS_THAN:
                return v < condition.CompareValue;
            case E_COMPARE_TYPE.LESS_THAN_EQUAL:
                return v <= condition.CompareValue;
            case E_COMPARE_TYPE.MORE_THAN:
                return v > condition.CompareValue;
            case E_COMPARE_TYPE.MORE_THAN_EQUAL:
                return v >= condition.CompareValue;
        }

        return false;
    }

    /// <summary>
    /// bool型イベント変数の比較演算を行う。
    /// </summary>
    private bool CompareBool(ref EventTriggerCondition condition)
    {
        var name = condition.VariableName;
        if (!ExistBool(name))
        {
            return false;
        }

        var v = GetBool(name);

        switch (condition.BoolCompareType)
        {
            case E_BOOL_COMPARE_TYPE.EQUAL:
                return v == condition.BoolCompareValue;
            case E_BOOL_COMPARE_TYPE.NOT_EQUAL:
                return v != condition.BoolCompareValue;
        }

        return false;
    }

    /// <summary>
    /// タイムピリオドの比較演算を行う。
    /// </summary>
    private bool CompareTimePeriod(ref EventTriggerCondition condition)
    {
        var name = condition.UseGeneralTimePeriod ? GetGeneralTimePeriodName(condition.GeneralTimePeriod) : condition.VariableName;
        if (!ExistTimePeriod(name))
        {
            return false;
        }

        var period = GetTimePeriod(name);
        if (period == null || !period.IsStart)
        {
            return false;
        }

        var v = period.GetPeriod();
        switch (condition.CompareType)
        {
            case E_COMPARE_TYPE.EQUAL:
                return v == condition.CompareValue;
            case E_COMPARE_TYPE.NOT_EQUAL:
                return v != condition.CompareValue;
            case E_COMPARE_TYPE.LESS_THAN:
                return v < condition.CompareValue;
            case E_COMPARE_TYPE.LESS_THAN_EQUAL:
                return v <= condition.CompareValue;
            case E_COMPARE_TYPE.MORE_THAN:
                return v > condition.CompareValue;
            case E_COMPARE_TYPE.MORE_THAN_EQUAL:
                return v >= condition.CompareValue;
        }

        return false;
    }

    #endregion

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
    public bool IsMeetRootCondition(ref EventTriggerRootCondition rootCondition)
    {
        bool result = false;
        if (!rootCondition.IsMultiCondition)
        {
            result = IsMeetCondition(ref rootCondition.SingleCondition);
        }
        else
        {
            // 複数条件の時の初期値
            result = rootCondition.MultiConditionType == E_MULTI_CONDITION_TYPE.AND;

            for (int i = 0; i < rootCondition.MultiConditions.Length; i++)
            {
                bool isMeet = IsMeetCondition(ref rootCondition.MultiConditions[i]);

                if (rootCondition.MultiConditionType == E_MULTI_CONDITION_TYPE.OR && isMeet)
                {
                    result = true;
                    break;
                }
                else if (rootCondition.MultiConditionType == E_MULTI_CONDITION_TYPE.AND && !isMeet)
                {
                    result = false;
                    break;
                }
            }
        }

        if (rootCondition.IsReverse)
        {
            result = !result;
        }

        return result;
    }

    private bool IsMeetCondition(ref EventTriggerCondition condition)
    {
        bool result = false;
        switch (condition.VariableType)
        {
            case E_VARIABLE_TYPE.INT:
                result = CompareInt(ref condition);
                break;
            case E_VARIABLE_TYPE.FLOAT:
                result = CompareFloat(ref condition);
                break;
            case E_VARIABLE_TYPE.BOOL:
                result = CompareBool(ref condition);
                break;
            case E_VARIABLE_TYPE.TIME_PERIOD:
                result = CompareTimePeriod(ref condition);
                break;
        }

        if (condition.IsReverse)
        {
            result = !result;
        }

        return result;
    }

    /// <summary>
    /// EventParamを追加する。
    /// </summary>
    public void AddEventParam(BattleRealEventTriggerParam[] parameters)
    {
        if (parameters == null)
        {
            return;
        }

        foreach (var parameter in parameters)
        {
            AddEventParam(parameter);
        }
    }

    /// <summary>
    /// EventParamを追加する。
    /// </summary>
    public void AddEventParam(BattleRealEventTriggerParam parameter)
    {
        if (parameter == null)
        {
            return;
        }

        var condition = parameter.Condition;
        if (condition.IsMultiCondition && condition.MultiConditions == null)
        {
            return;
        }

        var contents = parameter.Contents;
        if (contents == null || contents.Length < 1)
        {
            return;
        }

        m_EventParams.Add(parameter);
    }

    /// <summary>
    /// イベントを登録する。
    /// </summary>
    public void AddEvent(BattleRealEventContent[] contents)
    {
        if (contents == null)
        {
            return;
        }

        for (int i = 0; i < contents.Length; i++)
        {
            AddEvent(contents[i]);
        }
    }

    /// <summary>
    /// イベントを登録する。
    /// </summary>
    public void AddEvent(BattleRealEventContent content)
    {
        if (content.ExecuteTiming == BattleRealEventContent.E_EXECUTE_TIMING.IMMEDIATE)
        {
            // 待機リストに追加せず即時実行
            ExecuteEvent(content);
        }
        else
        {
            var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, content.DelayExecuteTime, () =>
            {
                m_WaitExecuteParams.Add(content);
            });

            BattleRealTimerManager.Instance.RegistTimer(timer);
        }
    }

    #region Execute Event

    /// <summary>
    /// イベントを実行する。
    /// </summary>
    public void ExecuteEvent(BattleRealEventContent eventContent)
    {
        if (eventContent.IsPassExecute)
        {
            Debug.LogWarningFormat("イベントの実行をパスしました。 event content name : {0}", eventContent.ContentName);
            return;
        }

        switch (eventContent.EventType)
        {
            case BattleRealEventContent.E_EVENT_TYPE.APPEAR_ENEMY_GROUP:
                if (eventContent.UseEnemyGroupParamArray)
                {
                    ExecuteApperEnemyGroup(eventContent.EnemyGroupParams);
                }
                else
                {
                    ExecuteApperEnemyGroup(eventContent.EnemyGroupParam);
                }
                break;
            case BattleRealEventContent.E_EVENT_TYPE.MOVE_PLAYER_BY_SEQUENCE:
                ExecuteMovePlayerBySequence(eventContent.MovePlayerSequenceGroup);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.RESTRICT_PLAYER_POSITION:
                ExecuteRestrictPlayerPosition();
                break;
            case BattleRealEventContent.E_EVENT_TYPE.CONTROL_CAMERA:
                ExecuteControlCamera(eventContent.ControlCameraParams);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.CONTROL_OBJECT:
                ExecuteControlObject(eventContent.ControlObjectParams);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.CONTROL_BGM:
                ExecuteControlBgm(eventContent.ControlBgmParams);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.CONTROL_TELOP_UI:
                ExecuteControlTelop(eventContent.ControlTelopParam);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.SHOW_CUTSCENE:
                ExecuteShowCutscene(eventContent.ShowCutsceneParam);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.SHOW_TALK:
                ExecuteShowTalk(eventContent.ShowTalkParam);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.SHOW_DIALOG:
                ExecuteShowDialog(eventContent.ShowDialogParam);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.CHANGE_BATTLE_REAL_STATE:
                ExecuteChangeBattleRealState(eventContent.ChangeState);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.FADE:
                ExecuteFade(eventContent.FadeParam);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.OPERATE_VARIABLE:
                ExecuteOperateVariable(eventContent.OperateVariableParams);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.OPERATE_TIME_PERIOD:
                ExecuteOperateTimePeriod(eventContent.CountStartTimePeriodNames);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.CALL_SCRIPT:
                ExecuteCallScript(eventContent.CallScriptParams);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.GAME_START:
                ExecuteGameStart();
                break;
            case BattleRealEventContent.E_EVENT_TYPE.GOTO_BOSS_EVENT:
                // 何もしない
                break;
            case BattleRealEventContent.E_EVENT_TYPE.BOSS_BATTLE_START:
                // 何もしない
                break;
            case BattleRealEventContent.E_EVENT_TYPE.GAME_CLEAR_WITHOUT_HACKING_COMPLETE:
                ExecuteGameClearWithoutHackingComplete();
                break;
            case BattleRealEventContent.E_EVENT_TYPE.GAME_CLEAR_WITH_HACKING_COMPLETE:
                ExecuteGameClearWithHackingComplete();
                break;
            case BattleRealEventContent.E_EVENT_TYPE.GAME_OVER:
                ExecuteGameOver();
                break;
            case BattleRealEventContent.E_EVENT_TYPE.EXECUTE_OTHER_EVENT:
                ExecuteOtherEvents(eventContent.ExecuteEvents);
                break;
            case BattleRealEventContent.E_EVENT_TYPE.CLEAN_DONT_DESTROY_EVENT:
                ExecuteCleanDontDestroyEvent();
                break;
            case BattleRealEventContent.E_EVENT_TYPE.RETIRE_ENEMY:
                ExecuteRetireAllEnemy(eventContent.IsOnlyRetireNotBossEnemy);
                break;
        }
    }

    /// <summary>
    /// 敵を出現させる。
    /// </summary>
    private void ExecuteApperEnemyGroup(BattleRealEnemyGroupParam enemyGroupParam)
    {
        BattleRealEnemyGroupManager.Instance.CreateEnemyGroup(enemyGroupParam);
    }

    /// <summary>
    /// 敵を出現させる。
    /// </summary>
    private void ExecuteApperEnemyGroup(BattleRealEnemyGroupParam[] enemyGroupParams)
    {
        if (enemyGroupParams == null)
        {
            return;
        }

        foreach (var param in enemyGroupParams)
        {
            BattleRealEnemyGroupManager.Instance.CreateEnemyGroup(param);
        }
    }

    private void ExecuteMovePlayerBySequence(SequenceGroup sequenceGroup)
    {
        BattleRealPlayerManager.Instance.MovePlayerBySequence(sequenceGroup);
    }

    private void ExecuteRestrictPlayerPosition()
    {
        BattleRealPlayerManager.Instance.RestrictPlayerPosition();
    }

    /// <summary>
    /// カメラを制御する。
    /// </summary>
    private void ExecuteControlCamera(ControlCameraParam[] controlCameraParams)
    {
        foreach (var param in controlCameraParams)
        {
            var camera = BattleRealCameraManager.Instance.GetCameraController(param.CameraType);
            if (camera != null)
            {
                camera.BuildSequence(param.SequenceGroup);
            }
        }
    }

    /// <summary>
    /// シーケンスオブジェクトを制御する。
    /// </summary>
    private void ExecuteControlObject(ControlObjectParam[] controlObjectParams)
    {
        foreach (var param in controlObjectParams)
        {
            BattleRealSequenceObjectManager.Instance.CreateSequenceObject(param.SequenceObjectPrefab, param.RootGroup);
        }
    }

    /// <summary>
    /// BGMを制御する。
    /// </summary>
    private void ExecuteControlBgm(ControlSoundParam[] controlBgmParams)
    {
        foreach (var param in controlBgmParams)
        {
            switch (param.ControlType)
            {
                case ControlSoundParam.E_SOUND_CONTROL_TYPE.PLAY:
                    AudioManager.Instance.Play(param.PlaySoundParam);
                    break;
                case ControlSoundParam.E_SOUND_CONTROL_TYPE.STOP:
                    AudioManager.Instance.Stop(param.StopSoundGroup);
                    break;
                case ControlSoundParam.E_SOUND_CONTROL_TYPE.CONTROL_AISAC:
                    AudioManager.Instance.OperateAisac(param.OperateAisacParam);
                    break;
                case ControlSoundParam.E_SOUND_CONTROL_TYPE.STOP_ALL_BGM:
                    AudioManager.Instance.StopAllBgm();
                    break;
                case ControlSoundParam.E_SOUND_CONTROL_TYPE.STOP_ALL_SE:
                    AudioManager.Instance.StopAllSe();
                    break;
                case ControlSoundParam.E_SOUND_CONTROL_TYPE.STOP_ALL_BGM_AND_SE:
                    AudioManager.Instance.StopAllBgm();
                    AudioManager.Instance.StopAllSe();
                    break;
            }
        }
    }

    /// <summary>
    /// リアルモードのテロップUIを制御する。
    /// </summary>
    private void ExecuteControlTelop(ControlTelopParam controlTelopParam)
    {
        switch (controlTelopParam.TargetTelopType)
        {
            case E_TELOP_TYPE.START_TELOP:
                BattleRealUiManager.Instance.PlayStartTelop();
                break;
            //case E_TELOP_TYPE.WARNING_TELOP:
            //    BattleRealUiManager.Instance.PlayWarningTelop();
            //    break;
            //case E_TELOP_TYPE.GAME_CLEAR_TELOP:
            //    BattleRealUiManager.Instance.PlayClearTelop();
            //    break;
        }
    }

    /// <summary>
    /// カットシーンを表示する。
    /// </summary>
    private void ExecuteShowCutscene(ShowCutsceneParam showCutsceneParam)
    {
        ShowCutsceneAction?.Invoke(showCutsceneParam);
    }

    /// <summary>
    /// 会話を表示する。
    /// </summary>
    private void ExecuteShowTalk(ShowTalkParam showTalkParam)
    {
        ShowTalkAction?.Invoke(showTalkParam);
    }

    /// <summary>
    /// ダイアログを表示する。
    /// </summary>
    private void ExecuteShowDialog(ShowDialogParam showDialogParam)
    {
        ShowDialogAction?.Invoke(showDialogParam);
    }

    private void ExecuteChangeBattleRealState(E_BATTLE_REAL_STATE state)
    {
        RequestChangeStateBattleRealManager?.Invoke(state);
    }

    /// <summary>
    /// フェードを制御する。
    /// </summary>
    private void ExecuteFade(FadeParam fadeParam)
    {
        if (FadeManager.Instance == null)
        {
            return;
        }

        FadeManager.Instance.Fade(fadeParam);
    }

    /// <summary>
    /// 変数を操作する。
    /// </summary>
    private void ExecuteOperateVariable(OperateVariableParam[] operateVariableParams)
    {
        foreach (var param in operateVariableParams)
        {
            switch (param.VariableType)
            {
                case E_EVENT_TRIGGER_VARIABLE_TYPE.INT:
                    if (!ExistInt(param.VariableName))
                    {
                        Debug.LogError("該当する変数がありません。 type : int, name : " + param.VariableName);
                        break;
                    }

                    var intValue = (int)param.OperandValue;
                    if (param.OperandValueType == E_OPERAND_VALUE_TYPE.VARIABLE)
                    {
                        intValue = GetInt(param.OperandValueName, (int)param.OperandValue);
                    }

                    CalcInt(param.VariableName, intValue, param.OperandType);
                    break;

                case E_EVENT_TRIGGER_VARIABLE_TYPE.FLOAT:
                    if (!ExistFloat(param.VariableName))
                    {
                        Debug.LogError("該当する変数がありません。 type : float, name : " + param.VariableName);
                        break;
                    }

                    var floatValue = param.OperandValue;
                    if (param.OperandValueType == E_OPERAND_VALUE_TYPE.VARIABLE)
                    {
                        floatValue = GetFloat(param.OperandValueName, param.OperandValue);
                    }

                    CalcFloat(param.VariableName, floatValue, param.OperandType);
                    break;

                case E_EVENT_TRIGGER_VARIABLE_TYPE.BOOL:
                    if (!ExistBool(param.VariableName))
                    {
                        Debug.LogError("該当する変数がありません。 type : bool, name : " + param.VariableName);
                        break;
                    }

                    var boolValue = param.BoolOperandValue;
                    if (param.OperandValueType == E_OPERAND_VALUE_TYPE.VARIABLE)
                    {
                        boolValue = GetBool(param.OperandValueName, param.BoolOperandValue);
                    }

                    CalcBool(param.VariableName, boolValue, param.BoolOperandType);
                    break;
            }
        }
    }

    /// <summary>
    /// タイムピリオドを操作する。
    /// </summary>
    private void ExecuteOperateTimePeriod(string[] timePeriodNames)
    {
        foreach (var name in timePeriodNames)
        {
            if (!ExistTimePeriod(name))
            {
                Debug.LogError("該当するタイムピリオドがありません。 TimePeriodName : " + name);
                break;
            }

            CountStartTimePeriod(name);
        }
    }

    /// <summary>
    /// 任意のスクリプトを実行する。
    /// </summary>
    private void ExecuteCallScript(CallScriptParam[] callScriptParams)
    {
        foreach (var param in callScriptParams)
        {
            Type type = Type.GetType(param.ScriptName);

            if (type == null || !type.IsSubclassOf(typeof(EventControllableScript)))
            {
                break;
            }

            var script = (EventControllableScript)Activator.CreateInstance(type);
            m_UpdateScripts.Add(script);
            script.SetArguments(param.ScriptArguments);
            script.SetCycle(E_OBJECT_CYCLE.STANDBY_UPDATE);
            script.OnInitialize();
        }
    }

    /// <summary>
    /// スクリプトを破棄する。
    /// </summary>
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
        foreach (var script in m_GotoDestroyScripts)
        {
            m_UpdateScripts.Remove(script);
            script.OnFinalize();
        }

        m_GotoDestroyScripts.Clear();
    }

    /// <summary>
    /// ゲーム開始イベントを発行する。
    /// </summary>
    private void ExecuteGameStart()
    {
        CountStartTimePeriod(GAME_START_TIME_PERIOD_NAME);
    }

    /// <summary>
    /// ゲームクリアイベントを発行する。
    /// </summary>
    private void ExecuteGameClearWithoutHackingComplete()
    {
        GameClearWithoutHackingCompleteAction?.Invoke();
    }

    private void ExecuteGameClearWithHackingComplete()
    {
        GameClearWithHackingCompleteAction?.Invoke();
    }

    /// <summary>
    /// ゲームオーバーイベントを発行する。
    /// </summary>
    private void ExecuteGameOver()
    {
        GameOverAction?.Invoke();
    }

    /// <summary>
    /// イベントを無条件で発生させる。
    /// </summary>
    private void ExecuteOtherEvents(BattleRealEventTriggerParam[] events)
    {
        if (events == null)
        {
            return;
        }

        foreach (var e in events)
        {
            if (e == null)
            {
                continue;
            }

            AddEvent(e.Contents);
        }
    }

    /// <summary>
    /// DontDestroyフラグが付いているイベントをリストから削除する。
    /// ただし、完全に削除されることが保証されるのはこれを呼び出したタイミングから2フレーム後。
    /// </summary>
    private void ExecuteCleanDontDestroyEvent()
    {
        m_GotoDestroyEventParams.AddRange(m_EventParams.Where(p => p.DontDestroy));
    }

    /// <summary>
    /// 敵を退場させる。
    /// </summary>
    private void ExecuteRetireAllEnemy(bool isOnlyRetireNotBossEnemy)
    {
        if (isOnlyRetireNotBossEnemy)
        {
            BattleRealEnemyManager.Instance.RetireAllNotBossEnemy();
        }
        else
        {
            BattleRealEnemyManager.Instance.RetireAllEnemy();
        }
    }

    #endregion
}

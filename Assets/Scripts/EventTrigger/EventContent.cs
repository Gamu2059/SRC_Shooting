using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EventTriggerの実行内容。
/// </summary>
[System.Serializable]
public struct EventContent
{
    public enum E_EVENT_TYPE
    {
        /// <summary>
        /// 敵の出現制御
        /// </summary>
        APPER_ENEMY,

        /// <summary>
        /// カメラ制御
        /// </summary>
        CONTROL_CAMERA,

        /// <summary>
        /// オブジェクト制御
        /// </summary>
        CONTROL_OBJECT,

        /// <summary>
        /// BGM制御
        /// </summary>
        CONTROL_BGM,

        /// <summary>
        /// イベント変数制御
        /// </summary>
        OPERATE_VARIABLE,

        /// <summary>
        /// タイムピリオド制御
        /// </summary>
        OPERATE_TIME_PERIOD,

        /// <summary>
        /// 任意のスクリプトの呼び出し
        /// </summary>
        CALL_SCRIPT,
    }

    public enum E_EXECUTE_TIMING
    {
        IMMEDIATE,
        DELAY,
    }

    [Header("共通パラメータ")]

    [Tooltip("イベントタイプ")]
    public E_EVENT_TYPE EventType;

    [Tooltip("イベントの実行タイミング")]
    public E_EXECUTE_TIMING ExecuteTiming;

    [Tooltip("実行タイミングをDELAYにしている場合、何秒後に実行するか")]
    public float DelayExecuteTime;

    [Header("APPER_ENEMYのパラメータ")]

    [Tooltip("出現させる敵のリストでのインデックス")]
    public int ApperEnemyIndex;

    [Header("CONTROL_CAMERAのパラメータ")]

    public TimelineParam CameraTimelineParam;

    [Header("CONTROL_OBJECTのパラメータ")]

    public bool UsePrefab;

    public GameObject ControllableObject;

    public TimelineParam ObjectTimelineParam;

    [Header("CONTROL_BGM")]

    public ControlBgmParam ControlBgmParam;

    [Header("OPERATE_VARIABLE")]

    public OperateVariableParam OperateVariableParam;

    [Header("OPERATE_TIME_PERIOD")]

    public OperateTimePeriodParam OperateTimePeriodParam;

    [Header("CALL_SCRIPT")]

    public string ScriptName;

    public ArgumentVariable[] ScriptArguments;
}

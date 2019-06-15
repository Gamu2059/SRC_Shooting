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
        APPEAR_ENEMY,

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

        /// <summary>
        /// ゲーム開始
        /// </summary>
        GAME_START,

        /// <summary>
        /// ゲームクリア
        /// </summary>
        GAME_CLEAR,
    }

    /// <summary>
    /// 実行タイミング
    /// </summary>
    public enum E_EXECUTE_TIMING
    {
        /// <summary>
        /// 遅延を待たずに即座に実行する
        /// </summary>
        IMMEDIATE,

        /// <summary>
        /// 遅延を待つ
        /// </summary>
        DELAY,
    }

    [Header("共通パラメータ")]

    [Tooltip("イベントタイプ")]
    public E_EVENT_TYPE EventType;

    [Tooltip("イベントの実行タイミング")]
    public E_EXECUTE_TIMING ExecuteTiming;

    [Tooltip("実行タイミングをDELAYにしている場合、何秒後に実行するか")]
    public float DelayExecuteTime;

    [Header("APPEAR_ENEMYのパラメータ")]

    [Tooltip("出現させる敵のリストでのインデックス")]
    public int AppearEnemyIndex;

    [Header("CONTROL_CAMERAのパラメータ")]

    [Tooltip("カメラタイプ")]
    public E_CAMERA_TYPE CameraType;

    [Tooltip("カメラのタイムラインパラメータ")]
    public TimelineParam CameraTimelineParam;

    [Header("CONTROL_OBJECTのパラメータ")]

    [Tooltip("プレハブを使うかどうか")]
    public bool UsePlayableObjectPrefab;

    [Tooltip("プレハブ")]
    public BattleMainPlayableBase PlayableObjectPrefab;

    [Tooltip("EventManagerに登録されているオブジェクトの名前")]
    public string RegisteredPlayableName;

    [Tooltip("オブジェクトのタイムラインパラメータ")]
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

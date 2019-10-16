using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EventTriggerの実行内容。
/// </summary>
[System.Serializable]
public struct BattleRealEventContent
{
    public enum E_EVENT_TYPE
    {
        /// <summary>
        /// 敵グループの出現制御
        /// </summary>
        APPEAR_ENEMY_GROUP,

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

        /// <summary>
        /// ゲームオーバー
        /// </summary>
        GAME_OVER,
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

    [Header("APPEAR_ENEMY")]

    public int AppearEnemyIndex;

    [Header("CONTROL_CAMERA")]

    public ControlCameraParam[] ControlCameraParams;

    [Header("CONTROL_OBJECT")]

    public ControlObjectParam[] ControlObjectParams;

    [Header("CONTROL_BGM")]

    public ControlBgmParam[] ControlBgmParams;

    [Header("OPERATE_VARIABLE")]

    public OperateVariableParam[] OperateVariableParams;

    [Header("OPERATE_TIME_PERIOD")]

    public string[] CountStartTimePeriodNames;

    [Header("CALL_SCRIPT")]

    public CallScriptParam[] CallScriptParams;
}

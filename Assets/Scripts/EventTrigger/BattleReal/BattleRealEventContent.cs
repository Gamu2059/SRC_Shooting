using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのEventの実行内容。
/// </summary>
[Serializable]
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
        /// ボスイベントへ遷移する
        /// </summary>
        GOTO_BOSS_EVENT,

        /// <summary>
        /// ボス戦開始
        /// </summary>
        BOSS_BATTLE_START,

        /// <summary>
        /// ゲームクリア(救出なし)
        /// </summary>
        GAME_CLEAR_WITHOUT_HACKING_COMPLETE,

        /// <summary>
        /// ゲームオーバー
        /// </summary>
        GAME_OVER,

        /// <summary>
        /// ゲームクリア(救出あり)
        /// </summary>
        GAME_CLEAR_WITH_HACKING_COMPLETE,

        /// <summary>
        /// カットシーンを表示する
        /// </summary>
        SHOW_CUTSCENE,

        /// <summary>
        /// 会話を表示する
        /// </summary>
        SHOW_TALK,

        /// <summary>
        /// ダイアログを表示する
        /// </summary>
        SHOW_DIALOG,

        /// <summary>
        /// リアルモードのステートを変更する
        /// </summary>
        CHANGE_BATTLE_REAL_STATE,

        /// <summary>
        /// フェードを制御する
        /// </summary>
        FADE,

        /// <summary>
        /// プレイヤーをシーケンスコントローラで制御する
        /// </summary>
        MOVE_PLAYER_BY_SEQUENCE,

        /// <summary>
        /// プレイヤーの移動位置をフィールド内に制限する
        /// </summary>
        RESTRICT_PLAYER_POSITION,
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

    public string ContentName;

    [Header("共通パラメータ")]

    [Tooltip("実行をパスするかどうか")]
    public bool IsPassExecute;

    [Tooltip("イベントタイプ")]
    public E_EVENT_TYPE EventType;

    [Tooltip("イベントの実行タイミング")]
    public E_EXECUTE_TIMING ExecuteTiming;

    [Tooltip("実行タイミングをDELAYにしている場合、何秒後に実行するか")]
    public float DelayExecuteTime;

    [Header("APPEAR_ENEMY")]

    public BattleRealEnemyGroupParam EnemyGroupParam;
    //public BattleRealEnemyGroupGenerateParamSet EnemyGroupGenerateParamSet;

    [Header("MOVE_PLAYER_BY_SEQUENCE")]

    public SequenceGroup MovePlayerSequenceGroup;

    [Header("CONTROL_CAMERA")]

    public ControlCameraParam[] ControlCameraParams;

    [Header("CONTROL_OBJECT")]

    public ControlObjectParam[] ControlObjectParams;

    [Header("CONTROL_BGM")]

    public ControlSoundParam[] ControlBgmParams;

    [Header("Show Cutscene")]

    public ShowCutsceneParam ShowCutsceneParam;

    [Header("Show Talk")]

    public ShowTalkParam ShowTalkParam;

    [Header("Show Dialog")]

    public ShowDialogParam ShowDialogParam;

    [Header("Change Battle Real State")]

    public E_BATTLE_REAL_STATE ChangeState;

    [Header("Fade")]

    public FadeParam FadeParam;

    [Header("OPERATE_VARIABLE")]

    public OperateVariableParam[] OperateVariableParams;

    [Header("OPERATE_TIME_PERIOD")]

    public string[] CountStartTimePeriodNames;

    [Header("CALL_SCRIPT")]

    public CallScriptParam[] CallScriptParams;
}
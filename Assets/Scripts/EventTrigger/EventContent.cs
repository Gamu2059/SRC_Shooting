using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EventTriggerの実行内容。
/// </summary>
[System.Serializable]
public class EventContent
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングに関するデータを管理する。
/// データ受け渡しの機能しか持たないのでstate classである。
/// </summary>
public static class HackingDataHolder
{
    /// <summary>
    /// ハッキングモードの生成に必要なデータ。
    /// </summary>
    public static BattleHackingLevelParamSet HackingLevelParamSet;

    /// <summary>
    /// ハッキングモードでの結果データ。
    /// </summary>
    public static bool HackingResultData;

    /// <summary>
    /// ハッキングが成功したかどうか。
    /// </summary>
    public static bool IsHackingSuccess = false;
}

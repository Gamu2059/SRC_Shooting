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
    public static BattleHackingLevelParamSet HackingLevelParamSet = null;

    /// <summary>
    /// 前回から持ち越したハッキングモードのボスのダメージ。
    /// </summary>
    public static int CarryOverHackingBossDamage = 0;

    /// <summary>
    /// ハッキングが成功したかどうか。
    /// </summary>
    public static bool IsHackingSuccess = false;

    /// <summary>
    /// ハッキング対象の名前。
    /// </summary>
    public static string HackingTargetName = null;
}

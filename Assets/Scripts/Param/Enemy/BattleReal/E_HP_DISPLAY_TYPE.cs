using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// いつHPゲージを表示させるか
/// </summary>
public enum E_HP_DISPLAY_TYPE
{
    /// <summary>
    /// 初めから表示
    /// <summary>
    FROM_BEGIN,

    /// <summary>
    /// 初めて攻撃を受けたら
    /// <summary>
    ATTACKED_FOR_THE_FIRST_TIME,

    /// <summary>
    /// 表示されない
    /// <summary>
    DISABLED,
}

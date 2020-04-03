using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

/// <summary>
/// ページ内文章をクリアする拡張コマンド。<br/>
/// もしかした既存で存在するかもしれない。<br/>
/// <br/>
/// Command : ClearPage<br/>
/// Param : なし
/// </summary>
public class ClearPageCommand : AdvCommand
{
    public ClearPageCommand(StringGridRow row) : base(row)
    {

    }

    public override void DoCommand(AdvEngine engine)
    {
        engine.Page.RemakeText();
    }
}

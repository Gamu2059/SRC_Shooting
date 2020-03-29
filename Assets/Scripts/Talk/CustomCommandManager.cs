using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

/// <summary>
/// 独自拡張コマンドを追加するためのマネージャ。
/// </summary>
public class CustomCommandManager : AdvCustomCommandManager
{
    public override void OnBootInit()
    {
        Utage.AdvCommandParser.OnCreateCustomCommandFromID += OnCreateCustomCommand;
    }

    public override void OnClear() { }

    public void OnCreateCustomCommand(string id, StringGridRow row, AdvSettingDataManager dataManager, ref AdvCommand command)
    {
        switch(id)
        {
            case "ClearPage":
                command = new ClearPageCommand(row);
                break;
            case "AutoBrPage":
                command = new AutoBrPageCommand(row);
                break;
        }
    }
}

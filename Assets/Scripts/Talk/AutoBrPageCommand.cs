using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

public class AutoBrPageCommand : AdvCommand
{
    private bool m_UseAutoBrPageChange;
    private bool m_AutoBrChange;
    private bool m_UseAutoSpeedChange;
    private float m_AutoSpeed;

    public AutoBrPageCommand(StringGridRow row) : base(row)
    {
        var autoBrPage = ParseCellOptional<string>(AdvColumnName.Arg1, null);
        switch(autoBrPage)
        {
            case "ON":
            case "On":
            case "on":
                m_UseAutoBrPageChange = true;
                m_AutoBrChange = true;
                break;
            case "OFF":
            case "Off":
            case "off":
                m_UseAutoBrPageChange = true;
                m_AutoBrChange = false;
                break;
            default:
                m_UseAutoBrPageChange = false;
                m_AutoBrChange = false;
                break;
        }

        var autoSpeed = ParseCellOptional<float>(AdvColumnName.Arg2, -1f);
        if (autoSpeed < 0f)
        {
            m_UseAutoSpeedChange = false;
        }
        else
        {
            m_AutoSpeed = autoSpeed;
        }
    }

    public override void DoCommand(AdvEngine engine)
    {
        if (m_UseAutoBrPageChange)
        {
            engine.Config.IsAutoBrPage = m_AutoBrChange;
        }
        if (m_UseAutoSpeedChange)
        {
            engine.Config.AutoBrPageSpeed = m_AutoSpeed;
        }
    }
}

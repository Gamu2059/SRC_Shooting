using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackingLogIndicator : ControllableMonoBehavior
{
    [SerializeField]
    private Text m_LogText;

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public void ShowLog(string targetName)
    {

    }

    public void ShowResult(bool isSuccess)
    {

    }
}

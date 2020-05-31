using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using Rewired;
using System;

/// <summary>
/// 独自実装のBackButton
/// </summary>
public class BackButtonFromCancel : MonoBehaviour
{
    [SerializeField, ActionIdProperty(typeof(RewiredConsts.Action))]
    private int m_BackActionId;

    [SerializeField]
    private string m_BackEventName = "Back";

    private Player m_Player;

    private void Start()
    {
        m_Player = ReInput.players.GetPlayer(0);
        m_Player.AddInputEventDelegate(OnButtonDown, UpdateLoopType.Update);
    }

    private void OnDestroy()
    {
        m_Player.ClearInputEventDelegates();
        m_Player = null;
    }

    private void OnButtonDown(InputActionEventData e)
    {
        if (e.actionId == m_BackActionId && e.GetButtonDown())
        {
            GameEventMessage.SendEvent(m_BackEventName);
        }
    }
}

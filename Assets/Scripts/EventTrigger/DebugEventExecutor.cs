using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugEventExecutor : MonoBehaviour
{
    [Serializable]
    private struct Data
    {
        public KeyCode Code;
        public BattleRealEventContent Event;
    }

    [SerializeField]
    private Data[] m_Events;

    private void Update()
    {
        if (BattleRealEventManager.Instance == null)
        {
            return;
        }

        foreach(var data in m_Events)
        {
            if (Input.GetKeyDown(data.Code))
            {
                BattleRealEventManager.Instance.ExecuteEvent(data.Event);
            }
        }
    }
}

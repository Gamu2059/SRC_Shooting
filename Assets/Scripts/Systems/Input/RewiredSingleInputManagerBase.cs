using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public abstract class RewiredSingleInputManagerBase<T> : Singleton<T> where T : RewiredSingleInputManagerBase<T>, new()
{
    #region Define

    private class ButtonData
    {
        public E_REWIRED_INPUT_STATE Value = E_REWIRED_INPUT_STATE.NONE;
    }

    private class AxisData
    {
        public float Value = 0f;
    }

    #endregion

    #region Field

    private Dictionary<string, ButtonData> m_Buttons;
    private Dictionary<string, AxisData> m_Axises;
    private Player m_Player;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_Buttons = new Dictionary<string, ButtonData>();
        m_Axises = new Dictionary<string, AxisData>();
    }

    public override void OnFinalize()
    {
        if (m_Axises != null)
        {
            m_Axises.Clear();
            m_Axises = null;
        }
        if (m_Buttons != null)
        {
            m_Buttons.Clear();
            m_Buttons = null;
        }
        if (m_Player != null)
        {
            m_Player = null;
        }
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        foreach (var name in m_Buttons.Keys)
        {
            m_Buttons[name].Value = GetButtonAction(name);
        }
        foreach (var name in m_Axises.Keys)
        {
            m_Axises[name].Value = GetAxisAction(name);
        }
    }

    #endregion

    public void SetPlayer(Player player)
    {
        m_Player = player;
    }

    public void RegisterButton(string name)
    {
        if (m_Buttons != null && !m_Buttons.ContainsKey(name))
        {
            m_Buttons.Add(name, new ButtonData());
        }
    }

    public void RemoveButton(string name)
    {
        if (m_Buttons != null && m_Buttons.ContainsKey(name))
        {
            m_Buttons.Remove(name);
        }
    }

    public E_REWIRED_INPUT_STATE GetButton(string name)
    {
        if (m_Buttons == null)
        {
            return E_REWIRED_INPUT_STATE.NONE;
        }

        return m_Buttons[name].Value;
    }

    public void RegisterAxis(string name)
    {
        if (m_Axises != null && !m_Axises.ContainsKey(name))
        {
            m_Axises.Add(name, new AxisData());
        }
    }

    public void RemoveAxis(string name)
    {
        if (m_Axises != null && m_Axises.ContainsKey(name))
        {
            m_Axises.Remove(name);
        }
    }

    public float GetAxis(string name)
    {
        if (m_Axises == null)
        {
            return 0f;
        }

        return m_Axises[name].Value;
    }

    private E_REWIRED_INPUT_STATE GetButtonAction(string name)
    {
        if (m_Player == null)
        {
            return E_REWIRED_INPUT_STATE.NONE;
        }

        if (m_Player.GetButtonDown(name))
        {
            return E_REWIRED_INPUT_STATE.DOWN;
        }
        else if (m_Player.GetButton(name))
        {
            return E_REWIRED_INPUT_STATE.STAY;
        }
        else if (m_Player.GetButtonUp(name))
        {
            return E_REWIRED_INPUT_STATE.UP;
        }

        return E_REWIRED_INPUT_STATE.NONE;
    }

    private float GetAxisAction(string name)
    {
        if (m_Player == null)
        {
            return 0f;
        }

        return m_Player.GetAxis(name);
    }

    public void SetActionCategory(string category, bool isEnable)
    {
        if (m_Player == null)
        {
            m_Player.controllers.maps.SetMapsEnabled(isEnable, category);
        }
    }
}

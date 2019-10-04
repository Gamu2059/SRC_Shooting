using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManagerBase : ControllableObject
{
    private class ButtonData
    {
        public E_INPUT_STATE Value = E_INPUT_STATE.NONE;
    }

    private class AxisData
    {
        public float Value = 0f;
    }

    private Dictionary<string, ButtonData> m_Buttons;
    private Dictionary<string, AxisData> m_Axises;

    private LinkedList<string> m_RegisterButtons;
    private LinkedList<string> m_RegisterAxises;

    private LinkedList<string> m_RemoveButtons;
    private LinkedList<string> m_RemoveAxises;

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_Buttons = new Dictionary<string, ButtonData>();
        m_Axises = new Dictionary<string, AxisData>();
        m_RegisterButtons = new LinkedList<string>();
        m_RegisterAxises = new LinkedList<string>();
        m_RemoveButtons = new LinkedList<string>();
        m_RemoveAxises = new LinkedList<string>();
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
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        foreach (var name in m_RegisterButtons)
        {
            m_Buttons.Add(name, new ButtonData());
        }
        foreach (var name in m_RegisterAxises)
        {
            m_Axises.Add(name, new AxisData());
        }
        m_RegisterButtons.Clear();
        m_RegisterAxises.Clear();

        foreach (var name in m_Buttons.Keys)
        {
            m_Buttons[name].Value = GetButtonAction(name);
        }
        foreach (var name in m_Axises.Keys)
        {
            m_Axises[name].Value = GetAxisAction(name);
        }

        foreach (var name in m_RemoveButtons)
        {
            m_Buttons.Remove(name);
        }
        foreach (var name in m_RemoveAxises)
        {
            m_Axises.Remove(name);
        }
        m_RemoveButtons.Clear();
        m_RemoveAxises.Clear();
    }

    public void RegisterButton(string name)
    {
        if (m_Buttons == null || m_RegisterButtons == null)
        {
            return;
        }

        if (m_Buttons.ContainsKey(name) || m_RegisterButtons.Contains(name))
        {
            return;
        }

        m_RegisterButtons.AddLast(name);
    }

    public void RemoveButton(string name)
    {
        if (m_Buttons == null || m_RemoveButtons == null)
        {
            return;
        }

        if (!m_Buttons.ContainsKey(name) || m_RemoveButtons.Contains(name))
        {
            return;
        }

        m_RemoveButtons.AddLast(name);
    }

    public E_INPUT_STATE GetButton(string name)
    {
        if (m_Buttons == null)
        {
            return E_INPUT_STATE.NONE;
        }

        return m_Buttons[name].Value;
    }

    public void RegisterAxis(string name)
    {
        if (m_Axises == null || m_RegisterAxises == null)
        {
            return;
        }

        if (m_Axises.ContainsKey(name) || m_RegisterAxises.Contains(name))
        {
            return;
        }

        m_RegisterAxises.AddLast(name);
    }

    public void RemoveAxis(string name)
    {
        if (m_Axises == null || m_RemoveAxises == null)
        {
            return;
        }

        if (!m_Axises.ContainsKey(name) || m_RemoveAxises.Contains(name))
        {
            return;
        }

        m_RemoveAxises.AddLast(name);
    }

    public float GetAxis(string name)
    {
        if (m_Axises == null)
        {
            return 0f;
        }

        return m_Axises[name].Value;
    }

    public E_INPUT_STATE GetButtonAction(string name)
    {
        if (Input.GetButtonDown(name))
        {
            return E_INPUT_STATE.DOWN;
        }
        else if (Input.GetButton(name))
        {
            return E_INPUT_STATE.STAY;
        }
        else if (Input.GetButtonUp(name))
        {
            return E_INPUT_STATE.UP;
        }

        return E_INPUT_STATE.NONE;
    }

    public float GetAxisAction(string name)
    {
        return Input.GetAxis(name);
    }
}

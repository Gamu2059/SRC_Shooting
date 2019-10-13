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

    protected void RegisterButton(string name)
    {
        if (m_Buttons != null && !m_Buttons.ContainsKey(name))
        {
            m_Buttons.Add(name, new ButtonData());
        }
    }

    protected void RemoveButton(string name)
    {
        if (m_Buttons != null && m_Buttons.ContainsKey(name))
        {
            m_Buttons.Remove(name);
        }
    }

    public E_INPUT_STATE GetButton(string name)
    {
        if (m_Buttons == null)
        {
            return E_INPUT_STATE.NONE;
        }

        return m_Buttons[name].Value;
    }

    protected void RegisterAxis(string name)
    {
        if (m_Axises != null && !m_Axises.ContainsKey(name))
        {
            m_Axises.Add(name, new AxisData());
        }
    }

    protected void RemoveAxis(string name)
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

    private E_INPUT_STATE GetButtonAction(string name)
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

    private float GetAxisAction(string name)
    {
        return Input.GetAxis(name);
    }
}

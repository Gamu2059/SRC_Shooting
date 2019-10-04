using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputExtension
{
    public enum E_INPUT_STATE
    {
        NONE,
        DOWN,
        STAY,
        UP,
    }

    public static E_INPUT_STATE GetButtonAction(string name)
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

    public static float GetAxisAction(string name)
    {
        return Input.GetAxis(name);
    }
}
